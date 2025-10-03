using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    // アニメーションの種類
    public enum AnimationType
    {
        /// <summary>
        /// 未設定
        /// </summary>
        None,
        /// <summary>
        /// デッキから手札へ
        /// </summary>
        DeckToHand,
        /// <summary>
        /// 手札からフィールドへ
        /// </summary>
        HandToField,
        /// <summary>
        /// 強化
        /// </summary>
        Upgrade,
        /// <summary>
        /// 死亡
        /// </summary>
        Death,
    }
    public AnimationType m_AnimationType { get; set; } = AnimationType.None;

    // アニメーションの進捗時間
    public float m_fAnimationTimer { get; set; } = 0.0f;
    // アニメーションの最大時間
    public float m_fAnimationMaxTimer { get; set; } = 1.0f;

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        switch(m_AnimationType)
        {
            case AnimationType.DeckToHand:
                DecktoHand_Update();
                break;
            case AnimationType.HandToField:
                HandtoHand_Update();
                break;
            case AnimationType.Upgrade:
                Upgrade_Update();
                break;
            case AnimationType.Death:
                Death_Update();
                break;
            default:
                break;
        }

        // アニメーションの進捗時間が最大時間に達したらオブジェクトを破棄
        if (m_fAnimationTimer >= m_fAnimationMaxTimer)
        {
            Destroy(this);
        }
        else
        {
            m_fAnimationTimer += Time.deltaTime;
        }
    }

    /// <summary>
    /// デッキから手札へのアニメーションの更新処理
    /// </summary>
    private void DecktoHand_Update()
    {

    }
    /// <summary>
    /// 手札からフィールドへのアニメーションの更新処理
    /// </summary>
    private void HandtoHand_Update()
    {

    }
    /// <summary>
    /// 強化のアニメーションの更新処理
    /// </summary>
    private void Upgrade_Update()
    {

    }
    /// <summary>
    /// 死亡のアニメーションの更新処理
    /// </summary>
    private void Death_Update()
    {

    }
}
