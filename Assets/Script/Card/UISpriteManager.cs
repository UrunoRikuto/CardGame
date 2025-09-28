/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：UISpriteManager.cs
* 
* 概要：UIで使用するスプライト素材の管理
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using UnityEngine;

/// <summary>
/// UIで使用するスプライト素材の管理
/// </summary>
public class UISpriteManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static UISpriteManager instance { private set; get; }

    // 管理するスプライト素材
    [Header("設定するコスト用スプライト素材")]
    public Sprite[] CostSprites;
    [Header("設定する攻撃力用スプライト素材")]
    public Sprite[] AttackSprites;
    [Header("設定する体力用スプライト素材")]
    public Sprite[] HpSprites;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
        // シングルトンの設定
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
