/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：FieldCardUI.cs
* 
* 概要：フィールド上のカードのUI管理
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// フィールド上のカードのUI管理
/// </summary>
public class FieldCardUI : MonoBehaviour
{
    // UIのImage
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
        AttackUI = transform.GetChild(0).GetComponent<Image>();
        HpUI = transform.GetChild(1).GetComponent<Image>();

        // UIのテクスチャを設定
        // 攻撃力
        SetAttackNum = cardData.cardAttack;
        AttackUI.sprite = UISpriteManager.instance.AttackSprites[SetAttackNum - 1];
        // 体力
        SetHpNum = cardData.cardLife;
        HpUI.sprite = UISpriteManager.instance.HpSprites[SetHpNum - 1];
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
