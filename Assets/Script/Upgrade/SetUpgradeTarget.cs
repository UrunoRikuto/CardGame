/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：SetUpgradeTarget.cs
* 
* 概要：強化のターゲット設定
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using UnityEngine;

/// <summary>
/// 戦闘のターゲット設定
/// </summary>
public class SetUpgradeTarget : MonoBehaviour
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
    /// UpgradePointerがトリガーに入ったとき
    /// </summary>
    /// <param name="collision">衝突したオブジェクトのCollider2Dコンポーネント</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // タグにUpgradePointerがなかった場合処理しない
        if (collision.transform.tag != "UpgradePointer") return;

        // ターゲットに設定できない場合は処理を抜ける
        if (!m_bTarget)
        {
            Debug.Log("ターゲットに設定できません");
            return;
        }

        // 強化システムを取得
        UpgradeSystem upgradeSystem = GameObject.Find("MainSystem").GetComponent<UpgradeSystem>();

        // 相手をターゲットにすることはできない
        if (upgradeSystem.m_UpgraderParent != transform.parent.parent)
        {
            Debug.Log("相手をターゲットにすることはできない");
            return;
        }

        // 対象に設定する
        upgradeSystem.TargetCard = transform;
    }

    /// <summary>
    /// UpgradePointerがトリガーから出たとき
    /// </summary>
    /// <param name="collision">衝突したオブジェクトのCollider2Dコンポーネント</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // タグにUpgradePointerがなかった場合処理しない
        if (collision.transform.tag != "UpgradePointer") return;

        // 強化システムを取得して対象から外す
        UpgradeSystem upgradeSystem = GameObject.Find("MainSystem").GetComponent<UpgradeSystem>();
        upgradeSystem.TargetCard = null;
    }
}
