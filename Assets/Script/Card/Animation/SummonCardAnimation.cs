using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SummonCardAnimation : MonoBehaviour
{
    // 経過時間
    private float m_ElapsedTime;
    // 最大時間
    private float m_MaxTime = 1.5f;

    // 生成するフィールドオブジェクトに必要な情報
    public struct FieldObjectInfo
    {
        // 生成するプレハブ
        public GameObject prefab;
        // 親オブジェクト情報
        public Transform parentTransform;
        // カードデータ
        public CardData cardData;
    }
    // フィールドオブジェクト情報
    private FieldObjectInfo m_FieldObjectInfo;


    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="fieldObjectInfo">フィールドオブジェクト情報</param>
    /// <param name="In_Sprite">表示するテクスチャ</param>
    public void InitSetting(FieldObjectInfo fieldObjectInfo, Sprite In_Sprite)
    {
        // フィールドオブジェクト情報を設定
        m_FieldObjectInfo = fieldObjectInfo;

        // 経過時間を初期化
        m_ElapsedTime = 0.0f;
        // テクスチャを設定
        GetComponent<Image>().sprite = In_Sprite;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        // 0秒から1秒までの間
        if (m_ElapsedTime < 0.5f)
        {
            // 開始時のスケールを1.0f、最大時のスケールを8.0fとして拡大を行う
            float scale = Mathf.Lerp(1.0f, 8.0f, m_ElapsedTime / 0.3f);
            transform.localScale = new Vector3(scale, scale, 1.0f);

            // 横回転を加える
            float rotateY = Mathf.Lerp(0.0f, 360.0f, m_ElapsedTime / 0.3f);
            transform.rotation = Quaternion.Euler(transform.rotation.x, rotateY, transform.rotation.z);
        }
        // 1秒から2秒までの間
        else if (m_ElapsedTime >= 0.5f && m_ElapsedTime < 1.5f)
        {
            // 最大時のスケールを8.0f、最小時のスケールを3.0fとして縮小を行う
            float scale = Mathf.Lerp(8.0f, 2.3f, (m_ElapsedTime - 1.0f) / 0.4f);
            transform.localScale = new Vector3(scale, scale, 1.0f);

            // 横回転を加える
            float rotateY = Mathf.Lerp(360.0f, 720.0f, (m_ElapsedTime - 1.0f) / 0.3f);
            transform.rotation = Quaternion.Euler(transform.rotation.x, rotateY, transform.rotation.z);

            // 親オブジェクトの位置に移動
            transform.position = Vector3.Lerp(transform.position, m_FieldObjectInfo.parentTransform.position, (m_ElapsedTime - 1.0f) / 0.5f);

            // 1.5秒経過時にテクスチャをフィールドオブジェクトのものに変更
            if (m_ElapsedTime >= 1.0f)
            {
                // テクスチャを変更
                GetComponent<Image>().sprite = m_FieldObjectInfo.cardData.cardFieldPrefab.GetComponent<Image>().sprite;
            }
        }

        // 経過時間を更新
        m_ElapsedTime += Time.deltaTime;

        // 最大時間を超えたらオブジェクトを破棄
        if (m_ElapsedTime >= m_MaxTime)
        {
            // フィールドオブジェクトを生成
            GameObject CardObject = GameObject.Instantiate(m_FieldObjectInfo.prefab, m_FieldObjectInfo.parentTransform);
            // スケールを設定
            CardObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            // カードデータを設定
            CardObject.GetComponent<CardInfo>().m_CardData = m_FieldObjectInfo.cardData;
            // 生成時発動の能力を発動
            if (m_FieldObjectInfo.cardData.cardActivationTiming == CardAbility.ActivationTiming.Start)
            {
                CardObject.GetComponent<CardAbility>().Action();
            }

            // アニメーションオブジェクトを破棄
            Destroy(gameObject);
        }
    }
}
