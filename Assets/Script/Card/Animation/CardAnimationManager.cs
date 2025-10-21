using UnityEngine;

/// <summary>
///　カードのアニメーション管理クラス
/// </summary>
public class CardAnimationManager : MonoBehaviour
{
    [SerializeField, Header("プレイヤーの親オブジェクト")]
    private Transform m_PlayerParent;

    [SerializeField, Header("敵の親オブジェクト")]
    private Transform m_EnemyParent;

    [SerializeField, Header("カードのアニメーションのPrefab")]
    private GameObject[] m_CardAnimationPrefab;

    /// <summary>
    /// カードを引くアニメーションの再生
    /// </summary>
    public void PlayDrawCardAnimation()
    {
        // ゲームシステムの取得
        GameSystem gameSystem = GameObject.Find("MainSystem").GetComponent<GameSystem>();

        // 設定する親オブジェクト
        Transform parent = null;
        switch (gameSystem.m_CurrentGameState)
        {
            case GameSystem.GameState.PlayerTurn:
                parent = m_PlayerParent;
                break;
            case GameSystem.GameState.EnemyTurn:
                parent = m_EnemyParent;
                break;
        }

        // 作成するアニメーションオブジェクト
        GameObject animationObj = null;
        // 引く前のカード枚数を取得
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).CompareTag("HandCard"))
            {
                // 手札の子オブジェクトの枚数を取得
                int HandCardChildCount = parent.GetChild(i).childCount;

                // 手札の最大枚数を超えている場合は処理を抜ける
                if (parent.GetChild(i).GetComponent<HandCard>().MAX_HANDCARD_SIZE <= HandCardChildCount)
                {
                    return;
                }
                break;
            }
        }

        // プレイヤーのカードを引くアニメーションを再生
        // CardAnimationManagerの子オブジェクトとして生成
        animationObj = Instantiate(m_CardAnimationPrefab[0], transform);
        animationObj.name = "DrawCardAnimation";

        // 次のカードのテクスチャを取得
        Sprite nextSprite = null;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).CompareTag("Deck"))
            {
                DeckData deckData = parent.GetChild(i).GetComponent<DeckData>();
                nextSprite = deckData.NextCardTex;
                break;
            }
        }
        animationObj.GetComponent<DrawCardAnimation>().InitSetting(parent, nextSprite);
    }

    /// <summary>
    /// カードを召喚するアニメーションの再生
    /// </summary>
    /// <param name="fieldObjectInfo">フィールドオブジェクトを生成するのに必要な情報</param>
    /// <param name="In_Sprite">表示するカードのテクスチャ</param>
    public void PlaySummonCardAnimation(SummonCardAnimation.FieldObjectInfo fieldObjectInfo, Sprite In_Sprite)
    {
        // 作成するアニメーションオブジェクト
        GameObject animationObj = null;
        // カードを召喚するアニメーションを再生
        // CardAnimationManagerの子オブジェクトとして生成
        animationObj = Instantiate(m_CardAnimationPrefab[1], transform);
        animationObj.name = "SummonCardAnimation";
        animationObj.GetComponent<SummonCardAnimation>().InitSetting(fieldObjectInfo, In_Sprite);
    }
}
