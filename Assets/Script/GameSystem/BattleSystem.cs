/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：BattleSystem.cs
* 
* 概要：戦闘システム
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using UnityEngine;

/// <summary>
/// 戦闘システム
/// </summary>
public class BattleSystem : MonoBehaviour
{
    // 戦闘に参加するカードデータ(0:攻撃側、1:防御側)
    public CardData[] m_BattleCardData = new CardData[2];

    // 戦闘対象に設定するLeader
    public Leader m_TargetLeader;

    // 攻撃側の親オブジェクト
    public Transform m_AttackerParent;
    // 攻撃側のカードオブジェクト
    public Transform m_Attacker;

    // 防御側の親オブジェクト
    public Transform m_DefenderParent;
    // 防御側のカードオブジェクト
    public Transform m_Defender;

    /// <summary>
    /// 戦闘処理
    /// </summary>
    /// <returns>戦闘が行われたかどうか</returns>
    public bool Battle()
    {
        // 戦闘を行ったかどうか
        bool IsBattle = false;

        // 戦闘対象のリーダーが設定されていたら
        if(m_TargetLeader != null)
        {
            // リーダーの体力を減らす
            m_TargetLeader.m_nHp -= (int)m_BattleCardData[0].cardAttack;

            if(m_TargetLeader.m_nHp <= 0)
            {
                // ゲームシステムの取得
                GameSystem gameSystem = transform.GetComponent<GameSystem>();
                // ゲームエンド
                gameSystem.SetEndGame();
            }

            IsBattle = true;
        }

        // まだ戦闘を行っていなかったら
        // 防御側のカードデータが設定されていたら
        if (m_BattleCardData[0] != null && m_BattleCardData[1] != null && !IsBattle)
        {
            // それぞれのカードの攻撃力とライフを比較して、ダメージを与える
            m_BattleCardData[0].cardLife -= m_BattleCardData[1].cardAttack;
            m_BattleCardData[1].cardLife -= m_BattleCardData[0].cardAttack;

            // ライフが0以下になった場合は0に補正する
            if (m_BattleCardData[0].cardLife <= 0)
            {
                m_BattleCardData[0].cardLife = 0;
                // 死亡時効果の発動
                m_Attacker.GetComponent<CardAbility>().OnDead();
                // 死亡フラグを立てる
                m_BattleCardData[0].DeadFlag = true;

            }
            if (m_BattleCardData[1].cardLife <= 0)
            {
                m_BattleCardData[1].cardLife = 0;
                // 死亡時効果の発動
                m_Defender.GetComponent<CardAbility>().OnDead();
                // 死亡フラグを立てる
                m_BattleCardData[1].DeadFlag = true;
            }

            IsBattle = true;
        }

        ClearTarget();
        return IsBattle;
    }

    /// <summary>
    /// 戦闘対象のクリア
    /// </summary>
    private void ClearTarget()
    {
        m_TargetLeader = null;
        m_AttackerParent = null;
        m_BattleCardData[0] = null;
        m_BattleCardData[1] = null;
    }
}
