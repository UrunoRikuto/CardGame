/*＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
* 
* file：CardAbility.cs
* 
* 概要：カードの能力
* 
＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝*/
using UnityEditor;
using UnityEngine;

/// <summary>
/// カードの能力
/// </summary>
public class CardAbility : MonoBehaviour
{
    /// <summary>
    /// カードの能力タイプ
    /// </summary>
    public enum AbilityType
    {
        /// <summary>
        /// なし(特に能力がない)
        /// </summary>
        None,
        /// <summary>
        /// 速攻(場に出た瞬間から相手のリーダーに攻撃可能)
        /// </summary>
        Rush,
        /// <summary>
        /// 守護(リーダーを相手の攻撃から守る)
        /// </summary>
        Guard,
        /// <summary>
        /// 召喚(指定したキャラクターを召喚)
        /// </summary>
        Summon,
        /// <summary>
        /// 強化(指定したバフをかける)
        /// </summary>
        Buff,
    }
    /// <summary>
    /// テキストからAbilityTypeを設定する
    /// </summary>
    /// <param name="abilityType">ファイルから読み込んだテキスト</param>
    /// <returns>対応したAbilityType</returns>
    public static AbilityType SetAbilityType(string abilityType)
    {
        switch (abilityType)
        {
            case "None":
                return AbilityType.None;
            case "速攻":
                return AbilityType.Rush;
            case "守護":
                return AbilityType.Guard;
            case "召喚":
                return AbilityType.Summon;
            case "強化":
                return AbilityType.Buff;
        }

        return AbilityType.None;
    }

    /// <summary>
    /// 能力の発動タイミング
    /// </summary>
    public enum ActivationTiming
    {
        /// <summary>
        /// なし(特に能力がない)
        /// </summary>
        None,
        /// <summary>
        /// 開始時(場に出た瞬間に発動)
        /// </summary>
        Start,
        /// <summary>
        /// 常時(場にいる間ずっと発動)
        /// </summary>
        Always,
        /// <summary>
        /// 任意(プレイヤーが好きなタイミングで発動)
        /// </summary>
        Any,
        /// <summary>
        /// 終了時(場から離れた瞬間に発動)
        /// </summary>
        End,
    }
    /// <summary>
    /// テキストからActivationTimingを設定する
    /// </summary>
    /// <param name="activationTiming">ファイルから読み込んだテキスト</param>
    /// <returns>対応したActivationTiming</returns>
    public static ActivationTiming SetActivationTiming(string activationTiming)
    {
        switch (activationTiming)
        {
            case "None":
                return ActivationTiming.None;
            case "生成":
                return ActivationTiming.Start;
            case "常時":
                return ActivationTiming.Always;
            case "任意":
                return ActivationTiming.Any;
            case "死亡":
                return ActivationTiming.End;
        }
        return ActivationTiming.None;
    }

    /// <summary>
    /// 能力の行動処理
    ///【死亡時】
    /// </summary>
    private void OnDestroy()
    {
        // カードデータを取得
        CardInfo cardInfo = GetComponent<CardInfo>();

        // 発動タイミングを取得
        ActivationTiming activationTiming = cardInfo.m_CardData.cardActivationTiming;

        // 能力タイプを取得
        AbilityType abilityType = cardInfo.m_CardData.cardAbilityType;

        // 能力に応じた処理を実行
        switch (abilityType)
        {
            case AbilityType.None:
                // 何もしない
                break;
            case AbilityType.Rush:
                // 何もしない
                break;
            case AbilityType.Guard:
                // SetTagetのターゲット設定を有効にする
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                    // 子オブジェクトを取得
                    Transform childTransform = transform.parent.GetChild(i);

                    // SetBattleTargetコンポーネントを取得してターゲット設定を有効にする
                    SetBattleTarget setBattleTarget = childTransform.GetComponentInChildren<SetBattleTarget>();
                    if (setBattleTarget != null)
                    {
                        setBattleTarget.m_bTarget = true;
                    }
                }
                // 自身のリーダーのターゲット設定を有効にする
                transform.parent.parent.GetComponentInChildren<Leader>().m_bTarget = false;
                break;
            case AbilityType.Summon:
                // 発動タイミングが死亡時ではない場合何もしない
                if (activationTiming != ActivationTiming.End) return;

                SummonAction(cardInfo.m_CardData.cardName);
                break;
            case AbilityType.Buff:
                // 強化の処理を書く
                break;
        }
    }

    /// <summary>
    /// 能力の行動処理
    ///【発動タイミング】
    /// 任意、常時、生成時
    /// </summary>
    public void Action()
    {
        // カードデータを取得
        CardInfo cardInfo = GetComponent<CardInfo>();
        // 能力タイプを取得
        AbilityType abilityType = cardInfo.m_CardData.cardAbilityType;

        // 能力に応じた処理を実行
        switch (abilityType)
        {
            case AbilityType.None:
                // 何もしない
                break;
            case AbilityType.Rush:
                // 場に出た瞬間から攻撃可能にする
                cardInfo.m_CardData.AttackFlag = true;
                break;
            case AbilityType.Guard:
                // 守護を持つカード以外のSetTargetのターゲット設定を無効にする
                for(int i = 0; i < transform.parent.childCount; i++)
                {
                    // 子オブジェクトを取得
                    Transform childTransform = transform.parent.GetChild(i);
                    // 能力タイプを取得
                    AbilityType childAbilityType = childTransform.GetComponent<CardInfo>().m_CardData.cardAbilityType;

                    // 自分自身や守護を持つカードは対象外
                    if (childTransform == transform || childAbilityType == AbilityType.Guard) continue;

                    // SetBattleTargetコンポーネントを取得してターゲット設定を無効にする
                    SetBattleTarget setBattleTarget = childTransform.GetComponentInChildren<SetBattleTarget>();
                    if (setBattleTarget != null)
                    {
                        setBattleTarget.m_bTarget = false;
                    }
                }
                // 自身のリーダーのターゲット設定を無効にする
                transform.parent.parent.GetComponentInChildren<Leader>().m_bTarget = false;
                break;
            case AbilityType.Summon:
                SummonAction(cardInfo.m_CardData.cardName);
                break;
            case AbilityType.Buff:
                // 強化の処理を書く
                break;
        }
    }

    /// <summary>
    /// 召喚の処理
    /// </summary>
    /// <param name="Name">召喚を行う親の名前</param>
    public void SummonAction(string Name)
    {
        // 召喚可能数の検査
        FieldCard fieldCard = transform.parent.GetComponent<FieldCard>();
        int CanSummonCount = fieldCard.maxFieldCardSize - fieldCard.transform.childCount;
        
        // 召喚可能数が0以下なら処理を抜ける
        if (CanSummonCount <= 0) return;

        // アセットの中のデータベースを取得
        CardDatabase cardDataBase = AssetDatabase.LoadAssetAtPath<CardDatabase>("Assets/Data/CardDatabase.asset");

        // モンスターの名前で処理を分岐
        switch (Name)
        {
            case "狼「リーダー」": // 狼2体の召喚処理
                // 生成回数の制限
                int summonNum = Mathf.Clamp(CanSummonCount, 0, 2);

                for(int i = 0; i < summonNum;i++)
                {
                    // データベースをインスタンス化
                    CardDatabase cardDatabaseInstance = ScriptableObject.Instantiate(cardDataBase);
                    // 狼のフィールドプレハブを取得
                    GameObject WolfPrefab = CardDatabase.GetCardData("狼", cardDatabaseInstance).cardFieldPrefab;
                    // 狼を生成
                    GameObject Wolf = GameObject.Instantiate(WolfPrefab);
                    // 生成した狼をフィールドの子オブジェクトに設定
                    Wolf.transform.SetParent(fieldCard.transform);
                    // スケールを設定
                    Wolf.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    // カードデータの設定
                    Wolf.GetComponent<CardInfo>().m_CardData = CardDatabase.GetCardData("狼", cardDatabaseInstance);
                }
                break;
        }
    }
}
