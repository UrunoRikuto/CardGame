using System.Collections.Generic;
using UnityEngine;

public class CardAnimationManager : MonoBehaviour
{
    List<GameObject> m_AnimationList = new List<GameObject>();

    [SerializeField]
    [Tooltip("アニメーションの最大時間リスト")]
    private int[] AnimationMaxTimeList;

    /// <summary>
    /// アニメーションオブジェクトを追加
    /// </summary>
    public void AddAnimation(CardAnimation.AnimationType In_type)
    {
        // アニメーションオブジェクトを生成
        GameObject AnimationObject = new GameObject("CardAnimationObj");

        // CardAnimationコンポーネントを追加
        CardAnimation AnimationComponent = AnimationObject.AddComponent<CardAnimation>();

        // タイプ別に処理
        switch (In_type)
        {
            case CardAnimation.AnimationType.DeckToHand:
                break;
            case CardAnimation.AnimationType.HandToField:
                break;
            case CardAnimation.AnimationType.Upgrade:
                break;
            case CardAnimation.AnimationType.Death:
                break;
        }

        // CardAnimationコンポーネントを追加
        m_AnimationList.Add(AnimationObject);
    }
}
