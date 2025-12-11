/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：CostManager.cs
* 
* 概要：コスト管理
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// コスト管理
/// </summary>
public class CostManager : MonoBehaviour
{
    [SerializeField,Header("コストの最大増加値")]
    private int MaxCostCount = 10;

    /// <summary>
    /// 現在の所持コスト数
    /// </summary>
    private int CostCount = 0;

    /// <summary>
    /// 現在の最大コスト数
    /// </summary>
    private int CurrentMaxCostCount = 0;


    [SerializeField,Header("数字プレハブ")]
    private GameObject NumberPrefab;

    /// <summary>
    /// 現在の所持コスト数表示UI
    /// </summary>
    /// <param name="index">0:一の位、1:十の位</param>
    private GameObject[] CurrentCostCountSpriteUI = new GameObject[2];
    [SerializeField, Header("現在の所持コスト数表示UIの一の位の表示位置")]
    private Vector3 CurrentCostCountSpriteUIOnePos;
    [SerializeField, Header("現在の所持コスト数表示UIの十の位の表示位置")]
    private Vector3 CurrentCostCountSpriteUITenPos;

    /// <summary>
    /// 現在の最大コスト数表示UI
    /// </summary>
    /// <param name="index">0:一の位、1:十の位</param>
    private GameObject[] CurrentMaxCostCountSpriteUI = new GameObject[2];
    [SerializeField, Header("現在の最大コスト数表示UIの一の位の表示位置")]
    private Vector3 CurrentMaxCostCountSpriteUIOnePos;
    [SerializeField, Header("現在の最大コスト数表示UIの十の位の表示位置")]
    private Vector3 CurrentMaxCostCountSpriteUITenPos;


    /// <summary>
    /// 生成しているコストプレハブオブジェクトのリスト
    /// </summary>
    private List<GameObject> CostPrefabList = new List<GameObject>();

    [SerializeField,Header("コストスプライトプレハブ")]
    private GameObject CostPrefab;

    [SerializeField,Header("コストスプライトの初期位置")]
    private Vector3 CostObjFirstPos = new Vector3(-85.5f, 0.0f, 0);

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        // 初期コストを設定
        CurrentMaxCostCount = CostCount = 0;

        // コストUIのプレハブ生成
        // 自身の子オブジェクトとして生成
        for (int i = 0; i < 2; i++)
        { 
            // 所持コスト数表示UIの取得
            CurrentCostCountSpriteUI[i] = Instantiate(NumberPrefab, transform);
            CurrentCostCountSpriteUI[i].transform.SetParent(this.transform);

            // 最大コスト数表示UIの取得
            CurrentMaxCostCountSpriteUI[i] = Instantiate(NumberPrefab, transform);
            CurrentMaxCostCountSpriteUI[i].transform.SetParent(this.transform);
        }

        // 所持コスト数表示UIの位置調整
        CurrentCostCountSpriteUI[0].GetComponent<RectTransform>().localPosition = CurrentCostCountSpriteUIOnePos;
        CurrentCostCountSpriteUI[1].GetComponent<RectTransform>().localPosition = CurrentCostCountSpriteUITenPos;
        // 最大コスト数表示UIの位置調整
        CurrentMaxCostCountSpriteUI[0].GetComponent<RectTransform>().localPosition = CurrentMaxCostCountSpriteUIOnePos;
        CurrentMaxCostCountSpriteUI[1].GetComponent<RectTransform>().localPosition = CurrentMaxCostCountSpriteUITenPos;

        // 初期状態では十の位の表示オブジェクトを非表示にする
        CurrentCostCountSpriteUI[1].SetActive(false);
        CurrentMaxCostCountSpriteUI[1].SetActive(false);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        // 生成しているコストスプライトが現在の最大コスト数と異なる場合、プレハブを生成する
        if (CostPrefabList.Count != CurrentMaxCostCount)
        {
            // コストスプライトプレハブを生成し、リストに追加
            GameObject newCostObj = Instantiate(CostPrefab, transform);

            // 生成したコストスプライトプレハブを自身の子オブジェクトに設定
            newCostObj.transform.SetParent(this.transform);

            CostPrefabList.Add(newCostObj);

            // コストスプライトの位置を調整
            Vector3 newPos = CostObjFirstPos + new Vector3(20 * (CostPrefabList.Count - 1), 0, 0);
            newCostObj.GetComponent<RectTransform>().localPosition = newPos;
        }
    }
    /// <summary>
    /// コストを1増加させるメソッド
    /// </summary>
    public void AddCost()
    {
        // 最大コスト数が上限に達していない場合
        if (CostCount < MaxCostCount)
        {
            // コストを1増加させる
            CostCount = CurrentMaxCostCount += 1;

            // コストボタンオブジェクトをアクティブにする
            foreach (GameObject cost in CostPrefabList)
            {
                cost.GetComponent<CostButton>().ResetActiveFlag();
            }
        }

        // 所持コスト数表示UIを更新
        CurrentCostCountSpriteUI[0].GetComponent<Number>().SetNumber(CostCount % 10);
        CurrentCostCountSpriteUI[1].GetComponent<Number>().SetNumber(CostCount / 10);
        if (CostCount / 10 != 0)
        {
            CurrentCostCountSpriteUI[1].SetActive(true);
        }
        // 最大コスト数表示UIを更新
        CurrentMaxCostCountSpriteUI[0].GetComponent<Number>().SetNumber(CurrentMaxCostCount % 10);
        CurrentMaxCostCountSpriteUI[1].GetComponent<Number>().SetNumber(CurrentMaxCostCount / 10);
        if (CurrentMaxCostCount / 10 != 0)
        {
            CurrentMaxCostCountSpriteUI[1].SetActive(true);
        }
    }

    /// <summary>
    /// コストを使用する前に、コストが足りるか確認するメソッド
    /// </summary>
    /// <param name="cost">使用コスト数</param>
    /// <returns>可能かどうか</returns>
    public bool CanUseCost(int cost)
    {
        if(CostCount < cost)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// コストを使用するメソッド
    /// </summary>
    /// <param name="cost">使用コスト数</param>
    public void UseCost(int cost)
    {
        // この処理を呼び出した時点での最大所持コスト数を保存
        int CurrentMaxCost = CostCount;

        // コストの使用処理
        // 呼び出した時点の最大所持コスト数 - 使用するコスト数 < 現在の所持コスト数
        while (CurrentMaxCost - CostCount < cost)
        {
            // 使用するコスト分、コストスプライトを非表示にする
            CostPrefabList[CostCount - 1].GetComponent<CostButton>().UseCost();
            CostCount--;
        }

        // 所持コスト数表示UIを更新
        CurrentCostCountSpriteUI[0].GetComponent<Number>().SetNumber(CostCount % 10);
        CurrentCostCountSpriteUI[1].GetComponent<Number>().SetNumber(CostCount / 10);
        if (CostCount / 10 == 0)
        {
            CurrentCostCountSpriteUI[1].SetActive(false);
        }
    }
}
