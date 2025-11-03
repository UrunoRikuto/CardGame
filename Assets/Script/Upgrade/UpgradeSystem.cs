using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    [Header("使用するデータベース")]
    [SerializeField]
    private CardDatabase UpgradecardDatabase;
    // データベースのインスタンス
    private CardDatabase UpgradecardDataInstancebase;

    // 強化対象のオブジェクト
    public Transform TargetCard;

    // 強化者の親オブジェクト
    public Transform m_UpgraderParent;

    /// <summary>
    /// 強化処理
    /// </summary>
    public bool Upgrade()
    {
        if(TargetCard == null)
        {
            Debug.Log("強化対象が設定されていません");
            return false;
        }

        // データベースをインスタンス化
        UpgradecardDataInstancebase = Instantiate(UpgradecardDatabase);
        // 強化前のカードデータの取得
        CardData TargetCardData = TargetCard.GetComponent<CardInfo>().m_CardData;
        // 強化対象のカード名を取得
        string targetName = TargetCardData.cardName;
        // 使用するカードデータの取得
        CardData upgradeCardData = CardDatabase.GetCardData(targetName, UpgradecardDataInstancebase);

        // 攻撃を行うフラグを同期させる
        upgradeCardData.AttackFlag = TargetCardData.AttackFlag;
        // 攻撃力と体力は強化前のカードの値に+2加算する
        upgradeCardData.cardAttack = TargetCardData.cardAttack + 2;
        upgradeCardData.cardLife = TargetCardData.cardLife + 2;

        // 強化対象のカードに強化後のデータを反映
        TargetCard.GetComponent<CardInfo>().m_CardData = upgradeCardData;

        // 強化済みなのでターゲットに設定できないようにする
        TargetCard.GetComponent<SetUpgradeTarget>().m_bTarget = false;

        // 強化後のテクスチャに変更
        TargetCard.GetComponent<Image>().sprite = upgradeCardData.upgradeSprite;

        // 強化対象のクリア
        TargetCard = null;
        m_UpgraderParent = null;

        return true;
    }
}
