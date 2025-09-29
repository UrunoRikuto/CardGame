/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：HandCardUI.cs
* 
* 概要：手札のカードUI管理
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 手札のカードUI管理
/// </summary>
public class HandCardUI : MonoBehaviour
{
    // UIのImage
    // コスト
    private Image CostUI;
    // 設定しているコストテクスチャ番号
    private int SetCostNum;
    // 攻撃力
    private Image AttackUI;
    // 設定している攻撃力テクスチャ番号
    private int SetAttackNum;
    // 体力
    private Image HpUI;
    // 設定している体力テクスチャ番号
    private int SetHpNum;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        // カードデータの取得
        CardData cardData = GetComponent<CardInfo>().m_CardData;

        // UIの設定
        for(int i = 0; i < transform.childCount; i++)
        {
            // 子オブジェクトのタグを取得
            string childTag = transform.GetChild(i).tag;

            // タグによって処理を分ける
            switch(childTag)
            {
                case "Cost":
                    CostUI = transform.GetChild(i).GetComponent<Image>();
                    break;
                case "Attack":
                    AttackUI = transform.GetChild(i).GetComponent<Image>();
                    break;
                case "Hp":
                    HpUI = transform.GetChild(i).GetComponent<Image>();
                    break;
            }

            if (CostUI != null && AttackUI != null && HpUI != null) break;
        }

        // UIのテクスチャを設定
        // コスト
        if (cardData.cardCost != 0)
        {
            SetCostNum = cardData.cardCost;
            CostUI.sprite = UISpriteManager.instance.CostSprites[SetCostNum - 1];
        }
        // 攻撃力
        if (cardData.cardAttack != 0)
        {
            SetAttackNum = cardData.cardAttack;
            AttackUI.sprite = UISpriteManager.instance.AttackSprites[SetAttackNum - 1];
        }
        // 体力
        if (cardData.cardLife != 0)
        {
            SetHpNum = cardData.cardLife;
            HpUI.sprite = UISpriteManager.instance.HpSprites[SetHpNum - 1];
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // カードデータの取得
        CardData cardData = GetComponent<CardInfo>().m_CardData;

        // UIのテクスチャを更新
        // 変化があった場合のみ更新する
        // コスト
        if (SetCostNum != cardData.cardCost && cardData.cardCost != 0)
        {
            SetCostNum = cardData.cardCost;
            CostUI.sprite = UISpriteManager.instance.CostSprites[SetCostNum - 1];
        }
        // 攻撃力
        if (SetAttackNum != cardData.cardAttack && cardData.cardAttack != 0)
        {
            SetAttackNum = cardData.cardAttack;
            AttackUI.sprite = UISpriteManager.instance.AttackSprites[SetAttackNum - 1];
        }
        // 体力
        if (SetHpNum != cardData.cardLife && cardData.cardLife != 0)
        {
            SetHpNum = cardData.cardLife;
            HpUI.sprite = UISpriteManager.instance.HpSprites[SetHpNum - 1];
        }
    }
}
