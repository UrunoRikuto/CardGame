/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：CostManager.cs
* 
* 概要：コスト管理
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// コスト管理
/// </summary>
public class CostManager : MonoBehaviour
{
    [Header("コストの最大増加値")]
    [SerializeField]
    private int MaxCostCount = 10;

    // 現在の所持コスト数
    private int CostCount = 0;

    // 現在の最大コスト数
    private int CurrentMaxCostCount = 0;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        // 初期コストを設定
        CurrentMaxCostCount = CostCount = 0;

        // 所持コストの数を表示
        transform.GetComponent<TextMeshProUGUI>().text = "Cost: " + CostCount.ToString();
    }

    /// <summary>
    /// コストを1増加させるメソッド
    /// </summary>
    public void AddCost()
    {
        if(CostCount >= MaxCostCount)
        {
            return;
        }

        CostCount = CurrentMaxCostCount += 1;

        // 所持コストの数を表示
        transform.GetComponent<TextMeshProUGUI>().text = "Cost: " + CostCount.ToString();
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
        // コストを使用
        CostCount -= cost;
        // 所持コストの数を表示
        transform.GetComponent<TextMeshProUGUI>().text = "Cost: " + CostCount.ToString();
    }
}
