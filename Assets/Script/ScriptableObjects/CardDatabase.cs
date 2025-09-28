/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：CardDatabase.cs
* 
* 概要：カードデータベース
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using System;
using UnityEngine;


[Serializable]
public class CardData
{
    [Header("名前")]
    public string cardName;

    [Header("この情報のカードを使用するかどうかのフラグ")]
    public bool ActiveFlag = true;

    [Header("攻撃を行うかどうかのフラグ")]
    public bool AttackFlag = false;

    [Header("カードの種類")]
    public string cardType;

    [Header("種族")]
    public string cardRace;

    [Header("コスト")]
    public int cardCost;

    [Header("攻撃力")]
    public int cardAttack;

    [Header("体力・持続時間")]
    public int cardLife;

    [Header("効果の発動タイミング")]
    public CardAbility.ActivationTiming cardActivationTiming;

    [Header("効果のタイプ")]
    public CardAbility.AbilityType cardAbilityType;

    [Header("手札プレハブ")]
    public GameObject cardHandPrefab;

    [Header("フィールドプレハブ")]
    public GameObject cardFieldPrefab;
}

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Scriptable Objects/CardDatabase")]
public class CardDatabase : ScriptableObject
{
    public CardData[] cardDataBase;

    /// <summary>
    /// 名前でカードデータを取得する
    /// </summary>
    /// <param name="name">検索する名前</param>
    /// <param name="cardDatabase">使用するカードデータベース</param>
    /// <returns>対応するカードデータ</returns>
    public static CardData GetCardData(string name, CardDatabase cardDatabase)
    {
        // 名前でカードデータを検索する
        foreach (var cardData in cardDatabase.cardDataBase) 
        {
            if (cardData.cardName == name)
            {
                return cardData;
            }
        }

        return null;
    }
}
