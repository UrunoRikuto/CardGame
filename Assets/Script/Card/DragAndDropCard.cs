/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：DragAndDropCard.cs
* 
* 概要：手札カードのドラッグアンドドロップ
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 手札カードのドラッグアンドドロップ
/// </summary>
public class DragAndDropCard : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // ドラック開始前の親オブジェクト
    private Transform m_BeforeParentObj;
    // ドラック終了後の親オブジェクト
    public Transform m_AffterParentObj { set; get; }

    // ドラッグ中のカードデータ
    public CardData m_DragCardData;

    // コストマネージャーの参照
    private CostManager m_CostManager;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        // コストマネージャーの参照を取得
        if (m_CostManager == null)
        {
           transform.parent.parent.GetChild(0).TryGetComponent<CostManager>(out m_CostManager);
        }
    }

    /// <summary>
    /// ドラッグ開始時の処理
    /// </summary>
    /// <param name="eventData">ドラッグイベントのデータ</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ゲームの状態を取得
        GameSystem.GameState gameState = GameObject.Find("MainSystem").GetComponent<GameSystem>().m_CurrentGameState;

        // プレイヤーのターン中のみドラッグ可能
        if (transform.parent.parent.CompareTag("Player"))
        {
            if(gameState == GameSystem.GameState.EnemyTurn)
            {
                // 敵のターン中はドラッグできない
                Debug.Log("敵のターン中はドラッグできません。");
                return;
            }
        }
        // 敵のターン中のみドラッグ可能
        else if (transform.parent.parent.CompareTag("Enemy"))
        {
            if (gameState == GameSystem.GameState.PlayerTurn)
            {
                // プレイヤーのターン中はドラッグできない
                Debug.Log("プレイヤーのターン中はドラッグできません。");
                return;
            }
        }

        // ドラッグ開始時に親オブジェクトを保存
        m_BeforeParentObj = transform.parent;
        // 自身が親オブジェクトの何番目の子オブジェクトかを取得
        int index = transform.GetSiblingIndex();
        // カードのデータを取得
        CardData cardData = m_BeforeParentObj.GetChild(index).GetComponent<CardInfo>().m_CardData;

        // コストが足りるか確認
        if (m_CostManager.CanUseCost(cardData.cardCost))
        {
            // カードのデータを取得
            m_DragCardData = cardData;

            // ドラッグ中はキャンバスの子オブジェクトにする
            transform.SetParent(GameObject.Find("Canvas").transform);
        }
        else
        {
            // コストが足りない場合は親オブジェクトをクリア
            m_BeforeParentObj = null;
        }
    }

    /// <summary>
    /// ドラッグ中の処理
    /// </summary>
    /// <param name="eventData">ドラッグイベントのデータ</param>
    public void OnDrag(PointerEventData eventData)
    {
        // ドラック前の親オブジェクトがnullの場合は何もしない
        if (m_BeforeParentObj == null)
        {
            return;
        }

        // ドラッグ中はオブジェクトの位置をマウスの位置に合わせる
        Vector3 worldPosition;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            (RectTransform)transform.parent,
            eventData.position,
            eventData.pressEventCamera,
            out worldPosition))
        {
            transform.position = worldPosition;
        }
    }

    /// <summary>
    /// ドラッグ終了時の処理
    /// </summary>
    /// <param name="eventData">ドラッグイベントのデータ</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        // ドラック前の親オブジェクトがnullの場合は何もしない
        if (m_BeforeParentObj == null)
        {
            return;
        }

        // 親オブジェクトが変更されたかどうかのフラグ
        bool IsChangeParent = false;

        // 新しい親オブジェクトが設定されている場合
        if (m_AffterParentObj != null)
        {
            // 親オブジェクトが同じかどうか
            // Player or Enemy
            if (m_BeforeParentObj.parent.tag == m_AffterParentObj.parent.tag)
            {
                // コストを使用
                m_CostManager.UseCost(m_DragCardData.cardCost);

                switch(m_DragCardData.cardType)
                {
                    case "キャラクター":
                        // フィールドオブジェクトを作成する処理
                        GameObject CardObject = GameObject.Instantiate(m_DragCardData.cardFieldPrefab);
                        // 親をドラック後の親オブジェクトに設定
                        CardObject.transform.SetParent(m_AffterParentObj);

                        // スケールを設定
                        CardObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                        // カードデータの設定
                        CardInfo cardInfo = CardObject.GetComponent<CardInfo>();
                        cardInfo.m_CardData = m_DragCardData;

                        // 生成時発動の能力を発動
                        if (cardInfo.m_CardData.cardActivationTiming == CardAbility.ActivationTiming.Start)
                        {
                            CardObject.GetComponent<CardAbility>().Action();
                        }
                        break;
                    case "スペル":
                        // TODO: 能力を発動させる
                        break;
                }

                // 親オブジェクトが変更されたフラグを立てる
                IsChangeParent = true;
            }
        }

        // 親オブジェクトが変更されなかった場合
        if (!IsChangeParent)
        {
            // 元の親オブジェクトに戻す
            transform.SetParent(m_BeforeParentObj);
            // 位置を元の位置に戻す
            transform.localPosition = Vector3.zero;
            // カードデータの設定
            if(m_BeforeParentObj.CompareTag("FieldCard"))
                m_BeforeParentObj.GetComponent<CardInfo>().m_CardData = m_DragCardData;
        }
        else
        {
            // 元の手札オブジェクトを削除
            Destroy(gameObject);

        }
        // ドラッグ終了後にカードデータをクリア
        m_DragCardData = null;
        // 元の親オブジェクトをクリア
        m_BeforeParentObj = null;
        // 新しい親オブジェクトをクリア
        m_AffterParentObj = null;
    }
}
