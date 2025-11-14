/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：FieldCard.cs
* 
* 概要：フィールドカード管理
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using UnityEngine;

/// <summary>
/// フィールドカード
/// </summary>
public class FieldCard : MonoBehaviour
{
    [Header("フィールドカードの間隔")]
    [SerializeField]
    private Vector3 CardDistance = new Vector3(0, 0, 0);

    [Header("フィールドカードの最大枚数")]
    [SerializeField]
    private int MAX_FIELD_CARD_SIZE = 5;
    public int maxFieldCardSize { get { return MAX_FIELD_CARD_SIZE; } }

    // 移動処理を実行済みの子オブジェクト数
    private int MoveedchildCount = 0;

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        int childcount = transform.childCount;
        if(MoveedchildCount != childcount)
        {
            MoveFieldCard();
        }

        // フィールドカードの生存チェック
        for (int i = 0; i < transform.childCount; i++)
        {
            // カードデータの取得
            CardInfo cardInfo = transform.GetChild(i).GetComponent<CardInfo>();

            // カードの体力が0以下の場合、フィールドから削除
            if (cardInfo.m_CardData.DeadFlag)
            {
                Debug.Log("カードの体力が0以下になったため、フィールドから削除します: " + cardInfo.m_CardData.cardName);
                // 子オブジェクトの削除
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// フィールドカードの位置を更新
    /// </summary>
    private void MoveFieldCard()
    {
        // 自身に子オブジェクトがある場合は、子オブジェクトの位置を更新
        // 子オブジェクトの数を取得
        MoveedchildCount = transform.childCount;
        // 子オブジェクトが存在する場合
        if (MoveedchildCount > 0)
        {
            Vector3[] CardPos = new Vector3[MoveedchildCount];

            // 子オブジェクトの位置を更新する処理
            // 一番左端の位置を基準にして、カードの間隔を考慮して位置を設定
            // 一番左の位置を設定
            CardPos[0].x = CardDistance.x / 2 * (MoveedchildCount - 1) * -1.0f;
            CardPos[0].y = 0.0f; // Y座標は0に設定

            // 子オブジェクトが複数存在する場合
            if (MoveedchildCount > 1)
            {
                // 一番左の位置を基準にして、他のカードの位置を設定
                for (int i = 1; i < MoveedchildCount; i++)
                {
                    CardPos[i].x = CardPos[0].x + CardDistance.x * i;
                    CardPos[i].y = 0.0f; // Y座標は0に設定
                }
            }


            // 子オブジェクトの位置を更新
            for(int i = 0; i < MoveedchildCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                if (childTransform != null)
                {
                    // 子オブジェクトの位置を設定
                    childTransform.localPosition = CardPos[i];
                }
            }
        }
    }

    /// <summary>
    /// フィールドにあるカードの攻撃フラグを有効にする
    /// </summary>
    public void ActiveAttackFlag()
    {
        // 自身に子オブジェクトがある場合は、子オブジェクトの攻撃フラグを有効にする
        for (int i = 0; i < transform.childCount; i++)
        {
            // 子オブジェクトのCardInfoコンポーネントを取得
            CardInfo cardInfo = transform.GetChild(i).GetComponent<CardInfo>();
            // 攻撃フラグを有効にする
            cardInfo.m_CardData.AttackFlag = true;

            Debug.Log("カードの攻撃フラグを有効にしました: " + cardInfo.m_CardData.cardName);
        }
    }

    /// <summary>
    /// カードの能力処理
    /// </summary>
    public void AbilityAction()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            // 発動タイミングの取得
            CardAbility.ActivationTiming activationTiming = transform.GetChild(i).GetComponent<CardInfo>().m_CardData.cardActivationTiming;

            // 発動タイミングが常時以外の場合は処理しない
            if (activationTiming != CardAbility.ActivationTiming.Always) continue;

            // 子オブジェクトのCardAbilityコンポーネントを取得
            CardAbility cardAbility = transform.GetChild(i).GetComponent<CardAbility>();
            if (cardAbility != null)
            {
                cardAbility.Action();
            }
        }
    }

    /// <summary>
    /// フィールドカードにカードが衝突したときの処理
    /// </summary>
    /// <param name="collision">衝突したオブジェクトのCollider2Dコンポーネント</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TargetPointタグのオブジェクトは無視
        if (collision.CompareTag("TargetPoint")) return;
        // UpgradePointerタグのオブジェクトは無視
        if (collision.CompareTag("UpgradePointer")) return;

        // 登録しているカードデータの数を取得
        int CardDataCount = transform.childCount;

        // フィールドカードの最大枚数に達していない場合のみ追加処理を行う
        if (CardDataCount < MAX_FIELD_CARD_SIZE)
        {
            // 衝突したオブジェクトのDragAndDropコンポーネントを取得
            DragAndDropCard dragAndDrop = collision.gameObject.GetComponent<DragAndDropCard>();
            // 衝突したオブジェクトのカードデータを取得
            CardData cardData = collision.transform.GetComponent<CardInfo>().m_CardData;

            // カードデータが存在する場合のみ処理を行う
            if (cardData != null)
            {
                // 衝突したオブジェクトのカードデータをフィールドカードに追加
                dragAndDrop.m_AffterParentObj = transform;
            }
        }
    }

    /// <summary>
    /// フィールドカードからカードが離れたときの処理
    /// </summary>
    /// <param name="collision">衝突したオブジェクトのCollider2Dコンポーネント</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 衝突が終了した場合の処理
        DragAndDropCard dragAndDrop = collision.gameObject.GetComponent<DragAndDropCard>();
        if (dragAndDrop != null)
        {
            // ドラッグアンドドロップの親オブジェクトをnullに設定
            dragAndDrop.m_AffterParentObj = null;
        }
    }
}
