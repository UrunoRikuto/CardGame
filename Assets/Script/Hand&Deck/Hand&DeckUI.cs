using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandandDeckCountUI : MonoBehaviour
{
    [SerializeField]
    [Header("手札のオブジェクト")]
    private Transform HandObject;

    [SerializeField]
    [Header("山札オブジェクト")]
    private Transform DeckObject;

    void Start()
    {
        int HandCount = HandObject.childCount;
        int DeckCount = DeckObject.GetComponent<DeckData>().DeckCount;

        // 所持コストの数を表示
        transform.GetComponent<TextMeshProUGUI>().text = "Hand:" + HandCount.ToString() +"|"+ "Deck:" + DeckCount.ToString();
    }
    void Update()
    {
        int HandCount = HandObject.childCount;
        int DeckCount = DeckObject.GetComponent<DeckData>().DeckCount;

        // 所持コストの数を表示
        transform.GetComponent<TextMeshProUGUI>().text = "Hand:" + HandCount.ToString() + "|" + "Deck:" + DeckCount.ToString();
    }
}
