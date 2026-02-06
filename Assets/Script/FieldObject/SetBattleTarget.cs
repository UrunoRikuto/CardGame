/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：SetBattleTarget.cs
* 
* 概要：戦闘のターゲット設定
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using UnityEngine;

/// <summary>
/// 戦闘のターゲット設定
/// </summary>
public class SetBattleTarget : MonoBehaviour
{
    /// <summary>
    /// ターゲットに設定できるかどうか
    /// </summary>
    public bool m_bTarget { get; set; }

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        m_bTarget = true;
    }

    private static string GetSideTag(Transform t)
    {
        if (t == null) return null;

        // Player/Enemy のどちらかのルート(または途中)に居る想定。
        //それ以外はサイド不明として null。
        Transform cur = t;
        while (cur != null)
        {
            if (cur.CompareTag("Player")) return "Player";
            if (cur.CompareTag("Enemy")) return "Enemy";
            cur = cur.parent;
        }
        return null;
    }

    /// <summary>
    /// TargetPointがトリガーに入ったとき
    /// </summary>
    /// <param name="collision">衝突したオブジェクトのCollider2Dコンポーネント</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // タグにTargetPointがなかった場合処理しない
        if (!collision.CompareTag("TargetPoint")) return;

        // ターゲットに設定できない場合は処理を抜ける
        if (!m_bTarget)
        {
            Debug.Log("ターゲットに設定できません");
            return;
        }

        // 戦闘システムを取得
        BattleSystem battleSystem = GameObject.Find("MainSystem").GetComponent<BattleSystem>();

        // 自分自身や仲間(同じサイド)をターゲットにすることはできない
        //ルート親比較だと Canvas 等で構造が崩れた場合に誤判定するため、Player/Enemy タグで判定する。
        var attackerSide = GetSideTag(battleSystem.m_AttackerParent);
        var defenderSide = GetSideTag(transform);
        if (!string.IsNullOrEmpty(attackerSide) && attackerSide == defenderSide)
        {
            Debug.Log("自分自身や仲間をターゲットにすることはできません");
            return;
        }

        // 戦闘の防御側のカードデータを設定
        battleSystem.m_BattleCardData[1]
            = transform.GetComponent<CardInfo>().m_CardData;

        // 戦闘の防御側の親を設定
        battleSystem.m_DefenderParent = transform.parent;

        // 戦闘の防御側を設定
        battleSystem.m_Defender = transform;
    }

    /// <summary>
    /// TargetPointがトリガーから出たとき
    /// </summary>
    /// <param name="collision">衝突したオブジェクトのCollider2Dコンポーネント</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // タグにTargetPointがなかった場合処理しない
        if (!collision.CompareTag("TargetPoint")) return;

        // 戦闘システムを取得
        BattleSystem battleSystem = GameObject.Find("MainSystem").GetComponent<BattleSystem>();

        // 戦闘の非攻撃者側のカードデータをクリア
        battleSystem.m_BattleCardData[1] = null;

        // 戦闘の防御側の親をクリア
        battleSystem.m_DefenderParent = null;

        // 戦闘の防御側をクリア
        battleSystem.m_Defender = null;
    }
}
