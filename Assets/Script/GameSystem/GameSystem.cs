/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：GameSystem.cs
* 
* 概要：ゲームシステム
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームシステム
/// </summary>
public class GameSystem : MonoBehaviour
{
    [Header("ターン切り替えボタン")]
    [SerializeField]
    private Button m_TurnChangeButton;

    [Header("プレイヤーの山札スクリプト")]
    [SerializeField]
    private DeckData m_PlayerDeck;
    [Header("プレイヤーのフィールドスクリプト")]
    [SerializeField]
    private FieldCard m_PlayerField;
    [Header("プレイヤーのコストマネージャー")]
    [SerializeField]
    private CostManager m_PlayerCostManager;

    [Header("敵の山札スクリプト")]
    [SerializeField]
    private DeckData m_EnemyDeck;
    [Header("敵のフィールドスクリプト")]
    [SerializeField]
    private FieldCard m_EnemyField;
    [Header("敵のコストマネージャー")]
    [SerializeField]
    private CostManager m_EnemyCostManager;

    [SerializeField]
    [Header("最初に引くカードの枚数")]
    private int m_nFirstDrawCardNum = 5; // 初期ドロー枚数

    /// <summary>
    /// ゲーム状態の列挙型
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// ゲーム開始
        /// </summary>
        Start,
        /// <summary>
        /// プレイヤーのターン
        /// </summary>
        PlayerTurn,
        /// <summary>
        /// 敵のターン
        /// </summary>
        EnemyTurn,
        /// <summary>
        /// ゲーム終了
        /// </summary>
        End
    }

    [Header("現在のゲーム状態")]
    public GameState m_CurrentGameState;
    // 次のゲーム状態を保持する変数
    private GameState m_NextGameState;

    /// <summary>
    /// ターンフェーズの列挙型
    /// </summary>
    public enum TurnPhase
    {
        /// <summary>
        /// ドローフェーズ
        /// </summary>
        Draw,
        /// <summary>
        /// メインフェーズ
        /// </summary>
        Main,
    }
    // 現在のターンフェーズ
    private TurnPhase m_CurrentTurnPhase;

    // ターン数
    private int m_nTarnNum = 0;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        // ボタンに処理を設定
        if(m_TurnChangeButton)m_TurnChangeButton.onClick.AddListener(NextTurn); // ターン切り替えボタンにリスナーを追加
    }

    /// <summary>
    /// 毎フレームの更新処理
    /// </summary>
    private void Update()
    {
        // 現在のゲーム状態に応じた処理を実行
        switch (m_CurrentGameState)
        {
            case GameState.Start:
                // ゲーム開始時の処理
                StartGame();
                break;
            case GameState.PlayerTurn:
                // プレイヤーのターンの処理
                PlayerTurn();
                break;
            case GameState.EnemyTurn:
                // 敵のターンの処理
                EnemyTurn();
                break;
            case GameState.End:
                // ゲーム終了時の処理
                EndGame();
                break;
        }
    }

    /// <summary>
    /// ゲーム開始時の処理
    /// </summary>
    private void StartGame()
    {
        // 先攻後攻処理
        int randomValue = UnityEngine.Random.Range(0, 2); // 0または1をランダムに生成
        if(randomValue == 0)
        {
            m_NextGameState = GameState.PlayerTurn; // プレイヤーのターンから開始
            Debug.Log("プレイヤーの先攻です。");
        }
        else
        {
            m_NextGameState = GameState.EnemyTurn; // エネミーのターンから開始
            Debug.Log("敵の先攻です。");
        }
        // プレイヤーとエネミーの初期ドロー処理
        for (int i = 0; i < m_nFirstDrawCardNum; i++)
        {
            // プレイヤーの山札からカードを引く
            m_PlayerDeck.DrawCard();
            // エネミーの山札からカードを引く
            m_EnemyDeck.DrawCard();
        }

        // すべての処理が完了したら、次のゲーム状態に移行
        m_CurrentGameState = m_NextGameState;
    }

    /// <summary>
    /// プレイヤーのターンの処理
    /// </summary>
    private void PlayerTurn()
    {
        switch (m_CurrentTurnPhase)
        {
            case TurnPhase.Draw:
                // 今いるすべてのフィールドカードを攻撃可能状態にする
                m_PlayerField.ActiveAttackFlag();
                // プレイヤーの山札からカードを引く
                m_PlayerDeck.DrawCard();
                // コストを追加
                m_PlayerCostManager.AddCost();
                // 次のターンを敵のターンに設定
                m_NextGameState = GameState.EnemyTurn; 
                // ドローフェーズの処理が完了したらメインフェーズに移行
                m_CurrentTurnPhase = TurnPhase.Main;
                break;
            case TurnPhase.Main:
                m_PlayerField.AbilityAction();
                m_EnemyField.AbilityAction();
                // メインフェーズの処理
                // メインフェーズの処理が完了したらエンドフェーズに移行
                break;
        }

    }
    /// <summary>
    /// 敵のターンの処理
    /// </summary>
    private void EnemyTurn()
    {
        switch (m_CurrentTurnPhase)
        {
            case TurnPhase.Draw:
                // 今いるすべてのフィールドカードを攻撃可能状態にする
                m_EnemyField.ActiveAttackFlag();
                // 敵の山札からカードを引く
                m_EnemyDeck.DrawCard();
                // 敵のコストを追加
                m_EnemyCostManager.AddCost();
                // 次のターンをプレイヤーのターンに設定
                m_NextGameState = GameState.PlayerTurn; 
                // ドローフェーズの処理が完了したらメインフェーズに移行
                m_CurrentTurnPhase = TurnPhase.Main;
                break;
            case TurnPhase.Main:
                m_PlayerField.AbilityAction();
                m_EnemyField.AbilityAction();

                // メインフェーズの処理
                // メインフェーズの処理が完了したらエンドフェーズに移行
                break;
        }

    }

    /// <summary>
    /// ゲーム終了時の処理
    /// </summary>
    private void EndGame()
    {
        // ゲーム終了時の処理
        Debug.Log("ゲームが終了しました。");
        // ここでゲームの結果を表示したり、リセット処理を行ったりすることができます。
    }

    /// <summary>
    /// 次のターンに移行する処理
    /// </summary>
    private void NextTurn()
    {
        // ターン数をカウントアップ
        m_nTarnNum++;
        Debug.Log("ターン数: " + m_nTarnNum);

        // 次のゲーム状態に移行
        m_CurrentGameState = m_NextGameState;
        // ターンフェーズを初期化
        m_CurrentTurnPhase = TurnPhase.Draw;
    }
}
