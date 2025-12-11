using UnityEngine;
using UnityEngine.UI;

public class CostButton : MonoBehaviour
{
    // コストボタンがアクティブかどうか
    [SerializeField]
    private bool IsActive = true;

    [SerializeField]
    private Sprite ActiveSprite;
    [SerializeField]
    private Sprite InactiveSprite;

    /// <summary>
    /// コストボタンがアクティブかどうかを取得するメソッド
    /// </summary>
    public void ResetActiveFlag()
    {
        IsActive = true;
        GetComponent<Image>().sprite = ActiveSprite;
    }

    /// <summary>
    /// コストボタンを使用するメソッド
    /// </summary>
    public void UseCost()
    {
        IsActive = false;
        GetComponent<Image>().sprite = InactiveSprite;
    }
}
