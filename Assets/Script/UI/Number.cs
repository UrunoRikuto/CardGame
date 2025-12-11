using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{
    /// <summary>
    /// 数字を設定する
    /// </summary>
    /// <param name="number">0～9までの一桁の数字</param>
    public void SetNumber(int number)
    {
        if(number < 0 || number > 9)
        {
            Debug.LogError("CostNumber: SetNumberに不正な値が設定されました。0～9の範囲で指定してください。");
            return;
        }

        // Imageコンポーネントの取得
        UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
        // スプライトの設定
        image.sprite = UISpriteManager.instance.NumberSprites[number];

    }
}
