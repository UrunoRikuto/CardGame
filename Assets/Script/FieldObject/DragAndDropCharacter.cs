/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：DragAndDropCharacter.cs
* 
* 概要：キャラクターのドラッグアンドドロップ
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// キャラクターのドラッグアンドドロップ
/// </summary>
public class DragAndDropCharacter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // 自身のRectTransformを取得
    private RectTransform StartTransform
    {
        get { return GetComponent<RectTransform>(); }
    }

    // ターゲットオブジェクトとそのRectTransform
    private GameObject TargetObject;

    // UIカメラ
    private Camera uiCamera;

    // 接線のスケール
    private float tangentScale = 50.0f;

    // 線分の数
    private int SegmentCount = 30;

    // LineRendererの参照
    private LineRenderer m_LineRenderer;

    /// <summary>
    /// ドラッグ開始時の処理
    /// </summary>
    /// <param name="eventData">ドラッグイベントのデータ</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // メインシステムオブジェクトを取得
        GameObject mainSystem = GameObject.Find("MainSystem");
        // 現在のゲーム状態を取得
        GameSystem.GameState gameState = mainSystem.GetComponent<GameSystem>().m_CurrentGameState;

        // プレイヤーとエネミーのターン制御
        if (transform.parent.parent.CompareTag("Player"))
        {
            if (gameState == GameSystem.GameState.EnemyTurn)
            {
                // 敵のターン中はドラッグできない
                Debug.Log("敵のターン中はドラッグできません。");
                return;
            }
        }
        else if (transform.parent.parent.CompareTag("Enemy"))
        {
            if (gameState == GameSystem.GameState.PlayerTurn)
            {
                // プレイヤーのターン中はドラッグできない
                Debug.Log("プレイヤーのターン中はドラッグできません。");
                return;
            }
        }

        // カードデータの取得
        CardData cardData = transform.GetComponent<CardInfo>().m_CardData;

        // 戦闘フラグが立っていない場合はドラッグできない
        if (!cardData.AttackFlag)
        {
            return;
        }

        // 戦闘システムを取得
        BattleSystem battleSystem = mainSystem.GetComponent<BattleSystem>();

        // 戦闘システムの攻撃者側の設定
        // カードデータを設定
        battleSystem.m_BattleCardData[0] = cardData;
        // 親オブジェクトを設定
        battleSystem.m_AttackerParent = transform.parent;
        // カードオブジェクトを設定
        battleSystem.m_Attacker = transform;


        // Canvasの子オブジェクトにターゲットオブジェクトを生成
        if (TargetObject == null)
        {
            // ターゲットオブジェクトの生成
            TargetObject = new GameObject("TargetPointer");
            // ターゲットオブジェクトにRectTransformを追加
            RectTransform rectTransform = TargetObject.AddComponent<RectTransform>();


            // 自身の親がPlayerの場合はEnemyのCanvasに、Enemyの場合はPlayerのCanvasに設定
            if (transform.parent.parent.CompareTag("Player"))
            {
                rectTransform.SetParent(GameObject.Find("Enemy").transform);
            }
            else if (transform.parent.parent.CompareTag("Enemy"))
            {
                rectTransform.SetParent(GameObject.Find("Player").transform);
            }

            // ターゲットオブジェクトの初期設定
            rectTransform.localScale = Vector3.one;
            rectTransform.sizeDelta = new Vector2(1, 1);

            // ターゲットオブジェクトにトリガー用のコライダーを追加
            TargetObject.AddComponent<BoxCollider2D>().isTrigger = true;
            // リジットボディを追加して重力を無効化
            TargetObject.AddComponent<Rigidbody2D>().gravityScale = 0.0f;
            // タグを設定
            TargetObject.tag = "TargetPoint";

            // Canvasの一番上に表示されるように設定
            rectTransform.SetAsLastSibling();

            // LineRendererの設定
            m_LineRenderer = GameObject.Find("Line").GetComponent<LineRenderer>();
            // 線の幅を設定
            m_LineRenderer.positionCount = SegmentCount;
            // ワールド座標を使用
            m_LineRenderer.useWorldSpace = true;

            // UIカメラの取得
            uiCamera = Camera.main;
        }
    }

    /// <summary>
    /// ドラッグ中の処理
    /// </summary>
    /// <param name="eventData">ドラッグイベントのデータ</param>
    public void OnDrag(PointerEventData eventData)
    {
        // ターゲットオブジェクトが存在しない場合は何もしない
        if (TargetObject == null) return;

        // ターゲットオブジェクトの位置をマウスの位置に合わせる
        Vector3 worldPosition;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            (RectTransform)transform.parent,
            eventData.position,
            eventData.pressEventCamera,
            out worldPosition))
        {
            TargetObject.transform.position = worldPosition;
        }

        // ターゲットオブジェクトのRectTransformの取得
        RectTransform rectTransform = TargetObject.GetComponent<RectTransform>();

        // ベジェ曲線の計算
        Vector3 p0 = WorldPosFromRect(StartTransform);
        Vector3 p1 = WorldPosFromRect(rectTransform);

        // uiオブジェクトの向きに合わせて接線を決める
        Vector3 m0 = StartTransform.right * tangentScale;
        Vector3 m1 = -rectTransform.right * tangentScale;

        // 線分の各ポイントを計算してLineRendererに設定
        for (int i = 0;i < SegmentCount;i++)
        {
            float t = i / (float)(SegmentCount - 1);
            Vector3 point = Hermite(p0, p1, m0, m1, t);
            m_LineRenderer.SetPosition(i, point);
        }
    }

    /// <summary>
    /// ドラッグ終了時の処理
    /// </summary>
    /// <param name="eventData">ドラッグイベントのデータ</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        // ターゲットオブジェクトが存在しない場合は何もしない
        if (TargetObject == null) return;

        // メインシステムオブジェクトを取得
        GameObject mainSystem = GameObject.Find("MainSystem");
        // 現在のゲーム状態を取得
        GameSystem.GameState gameState = mainSystem.GetComponent<GameSystem>().m_CurrentGameState;

        // プレイヤーとエネミーのターン制御
        if (transform.parent.parent.CompareTag("Player"))
        {
            if (gameState == GameSystem.GameState.EnemyTurn)
            {
                // 敵のターン中はドラッグできない
                Debug.Log("敵のターン中はドラッグできません。");
                return;
            }
        }
        else if (transform.parent.parent.CompareTag("Enemy"))
        {
            if (gameState == GameSystem.GameState.PlayerTurn)
            {
                // プレイヤーのターン中はドラッグできない
                Debug.Log("プレイヤーのターン中はドラッグできません。");
                return;
            }
        }

        // 戦闘システムを取得
        BattleSystem battleSystem = GameObject.Find("MainSystem").GetComponent<BattleSystem>();

        // バトルを開始
        if (battleSystem.Battle())
        {
            // カードデータの戦闘フラグをfalseに設定
            transform.GetComponent<CardInfo>().m_CardData.AttackFlag = false;
        }

        // ラインレンダラーの位置数をリセット
        m_LineRenderer.positionCount = 0;

        if (TargetObject != null)
        {
            Destroy(TargetObject); // ターゲットオブジェクトを削除
        }
    }

    /// <summary>
    /// RectTransformからワールド座標を取得
    /// </summary>
    /// <param name="rect">RectTransform</param>
    /// <returns>ワールド座標</returns>
    Vector3 WorldPosFromRect(RectTransform rect)
    {
        // World座標に変換
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, rect.position);
        Vector3 worldPos = uiCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, rect.position.z - uiCamera.transform.position.z));
        return worldPos;
    }

    /// <summary>
    /// ハーミット曲線の計算
    /// </summary>
    /// <param name="p0">開始点</param>
    /// <param name="p1">終了点</param>
    /// <param name="m0">開始点の接線ベクトル</param>
    /// <param name="m1">終了点の接線ベクトル</param>
    /// <param name="t">0から1の範囲のパラメータ。0がp0、1がp1に対応</param>
    /// <returns>計算されたハーミット曲線上の点</returns>
    Vector3 Hermite(Vector3 p0, Vector3 p1, Vector3 m0, Vector3 m1, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        float h00 = 2 * t3 - 3 * t2 + 1;
        float h10 = t3 - 2 * t2 + t;
        float h01 = -2 * t3 + 3 * t2;
        float h11 = t3 - t2;

        return h00 * p0 + h10 * m0 + h01 * p1 + h11 * m1;
    }
}
