/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：DeckData.cs
* 
* 概要：デッキデータ
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// デッキデータ
/// </summary>
public class DeckData : MonoBehaviour
{
    [Header("使用するデータベース")]
    [SerializeField]
    private CardDatabase cardDatabase;
    // データベースのインスタンス
    private CardDatabase cardDataInstancebase;

    [Header("手札のスクリプト")]
    [SerializeField]
    private HandCard HandCards;

    [Header("デッキの最大サイズ")]
    [SerializeField]
    private int MAX_DECK_SIZE = 20;

    [Header("デッキに設定するカードデータ")]
    [SerializeField]
    private List<CardData> deckCards = new List<CardData>();
    public int DeckCount { get { return deckCards.Count; } }

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        if (cardDatabase != null && cardDatabase.cardDataBase.Length > 0)
        {
            List<CardData> ActiveCardList = new List<CardData>();
            
            // デッキにカードデータをランダムに設定
            for (int i = 0; i < MAX_DECK_SIZE; i++)
            {
                cardDataInstancebase = Instantiate(cardDatabase);
                foreach (var CardData in cardDataInstancebase.cardDataBase)
                {
                    if (CardData.ActiveFlag)
                    {
                        ActiveCardList.Add(CardData);
                    }
                }

                int index = Random.Range(0, ActiveCardList.Count);

                CardData cardData = ActiveCardList[index]; // インスタンス化してデッキに追加

                deckCards.Add(cardData);
            }
        }
    }

    /// <summary>
    /// デッキからカードを1枚引く
    /// </summary>
    public void DrawCard()
    {
        // デッキの一番上のカードを取り出す
        CardData topCard = deckCards[deckCards.Count - 1];
        // デッキからカードを削除
        deckCards.RemoveAt(deckCards.Count - 1);
        // 手札にカードを追加
        if (!HandCards.AddCard(topCard))
        {
            // 手札に追加できなかった場合、デッキに戻す
            deckCards.Add(topCard);
        }
    }
}