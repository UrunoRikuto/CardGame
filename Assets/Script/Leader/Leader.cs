using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leader : MonoBehaviour
{
    /// <summary>
    /// 体力
    /// </summary>
    [Range(0, 20)]
    public int m_nHp;
    [SerializeField]
    public TextMeshProUGUI m_HpText;

    /// <summary>
    /// ターゲットに設定できるかどうか
    /// </summary>
    public bool m_bTarget { get; set; }

    private static string GetSideTag(Transform t)
    {
        if (t == null) return null;
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
    /// 初期化
    /// </summary>
    void Start()
    {
        m_nHp = 20;
        m_bTarget = true;
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // 体力を表示
        m_HpText.text = "HP:" + m_nHp.ToString();
    }

    /// <summary>
    /// 何かがトリガーに入ったとき
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

        // 自分自身/味方のリーダーを攻撃対象にしない
        var attackerSide = GetSideTag(battleSystem.m_AttackerParent);
        var leaderSide = GetSideTag(transform);
        if (!string.IsNullOrEmpty(attackerSide) && attackerSide == leaderSide)
        {
            Debug.Log("自分自身や味方リーダーをターゲットにすることはできません");
            return;
        }

        // 戦闘対象に自身を追加
        battleSystem.m_TargetLeader = this;
    }

    /// <summary>
    /// 何かがトリガーから出たとき
    /// </summary>
    /// <param name="collision">衝突したオブジェクトのCollider2Dコンポーネント</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // タグにTargetPointがなかった場合処理しない
        if (!collision.CompareTag("TargetPoint")) return;

        // 戦闘システムを取得
        BattleSystem battleSystem = GameObject.Find("MainSystem").GetComponent<BattleSystem>();

        // 戦闘対象のクリア
        battleSystem.m_TargetLeader = null;
    }
}
