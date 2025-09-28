/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：HandCard.cs
* 
* 概要：手札管理
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using UnityEngine;

/// <summary>
/// 手札管理
/// </summary>
public class HandCard : MonoBehaviour
{    
    [Header("手札の最大サイズ")]
    [SerializeField]
    private int MAX_HANDCARD_SIZE = 5;

    [Header("取り出し後の格納先")]
    [SerializeField]
    private GameObject HandCardsContainer;

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // カードの子オブジェクトの位置を重ならないように更新
        for (int i = 0; i < HandCardsContainer.transform.childCount; i++)
        {
            // 子オブジェクトを取得
            Transform child = HandCardsContainer.transform.GetChild(i);
            // 子オブジェクトの位置を調整
            float offsetX = 0.0f;
            // X軸方向に120.0fずつずらす
            float offsetXScale = 120.0f; 
            if (HandCardsContainer.transform.childCount % 2 == 0)
            {
                // 偶数の場合
                offsetX = ((i - HandCardsContainer.transform.childCount / 2) + 0.5f) * offsetXScale; 
            }
            else
            {
                // 奇数の場合
                offsetX = (i - (HandCardsContainer.transform.childCount / 2)) * offsetXScale; 
            }

            // 座標の設定
            child.localPosition = new Vector3(offsetX, 0, 0); // 例: X軸方向に0.5fずつずらす
        }
    }

    /// <summary>
    /// 手札にカードを追加するメソッド
    /// </summary>
    /// <param name="cardData">追加するカードデータ</param>
    public bool AddCard(CardData cardData)
    {
        if (HandCardsContainer.transform.childCount < MAX_HANDCARD_SIZE)
        {
            // プレハブを使ってゲームオブジェクトを生成
            GameObject CardObject = GameObject.Instantiate(cardData.cardHandPrefab);
            // 名前を設定
            CardObject.name = cardData.cardName;
            // 子オブジェクトに設定
            CardObject.transform.SetParent(HandCardsContainer.transform);
            // 現在の位置に配置
            CardObject.transform.localPosition = Vector3.zero;
            //手札の大きさ調整
            CardObject.transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);

            // カードデータを設定
            CardObject.GetComponent<CardInfo>().m_CardData = cardData;

            return true;
        }
        else
        {
            return false;
        }
    }
}
