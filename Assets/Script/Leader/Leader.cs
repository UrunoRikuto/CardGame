using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour
{
    /// <summary>
    /// 体力
    /// </summary>
    public float m_fHp;

    /// <summary>
    /// ターゲットに設定できるかどうか
    /// </summary>
    public bool m_bTarget { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        m_fHp = 100.0f;
        m_bTarget = true;
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {

    }

    /// <summary>
    /// 何かがトリガーに入ったとき
    /// </summary>
    /// <param name="collision">衝突したオブジェクトのCollider2Dコンポーネント</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // タグにTargetPointがなかった場合処理しない
        if (collision.transform.tag != "TargetPoint") return;

        // ターゲットに設定できない場合は処理を抜ける
        if (!m_bTarget)
        {
            Debug.Log("ターゲットに設定できません");
            return;
        }


        // 戦闘システムを取得
        BattleSystem battleSystem = GameObject.Find("MainSystem").GetComponent<BattleSystem>();

        // 戦闘対象に自信を追加する
        battleSystem.m_TargetLeader = this;

    }

    /// <summary>
    /// 何かがトリガーから出たとき
    /// </summary>
    /// <param name="collision">衝突したオブジェクトのCollider2Dコンポーネント</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // タグにTargetPointがなかった場合処理しない
        if (collision.transform.tag != "TargetPoint") return;

        // 戦闘システムを取得
        BattleSystem battleSystem = GameObject.Find("MainSystem").GetComponent<BattleSystem>();

        // 戦闘対象のクリア
        battleSystem.m_TargetLeader = null;
    }
}
