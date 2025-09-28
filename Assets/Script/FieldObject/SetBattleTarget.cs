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

    /// <summary>
    /// 何かがトリガーに入ったとき
    /// </summary>
    /// <param name="collision">衝突したオブジェクトのCollider2Dコンポーネント</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ターゲットに設定できない場合は処理を抜ける
        if (!m_bTarget)
        {
            Debug.Log("ターゲットに設定できません");
            return;
        }

        // 戦闘システムを取得
        BattleSystem battleSystem = GameObject.Find("MainSystem").GetComponent<BattleSystem>();

        // 自分自身や仲間をターゲットにすることはできない
        if (battleSystem.m_AttackerParent == transform.parent)
        {
            return;
        }

        // 戦闘の防御側のカードデータを設定
        battleSystem.m_BattleCardData[1]
            = transform.GetComponent<CardInfo>().m_CardData;
    }

    /// <summary>
    /// 何かがトリガーから出たとき
    /// </summary>
    /// <param name="collision">衝突したオブジェクトのCollider2Dコンポーネント</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 戦闘システムを取得
        BattleSystem battleSystem = GameObject.Find("MainSystem").GetComponent<BattleSystem>();

        // 戦闘の非攻撃者側のカードデータをクリア
        battleSystem.m_BattleCardData[1] = null;
    }
}
