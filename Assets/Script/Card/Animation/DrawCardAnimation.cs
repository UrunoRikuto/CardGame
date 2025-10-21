using UnityEngine;
using UnityEngine.UI;

public class DrawCardAnimation : MonoBehaviour
{
    // 目的地
    private Vector3 m_TargetPoint;
    // 経過時間
    private float m_ElapsedTime;
    // 最大時間
    private float m_MaxTime = 3.0f;


    // 変化するテクスチャ
    private Sprite m_ChangeSprite;
    // テクスチャ変更フラグ
    bool m_TextureChangeFlag = false;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="In_Parent">親オブジェクト</param>
    /// <param name="In_Sprite">変化するテクスチャ</param>
    public void InitSetting(Transform In_Parent, Sprite In_Sprite)
    {
        bool StartSetting = false;
        bool EndSetting = false;

        // 親オブジェクトの子オブジェクトを取得
        for (int i = 0; i < In_Parent.childCount; i++)
        {
            // Deckタグがついているオブジェクトを現在位置に設定
            if (In_Parent.GetChild(i).CompareTag("Deck"))
            {
                transform.position = In_Parent.GetChild(i).position;
                StartSetting = true;
            }
            // HandCardタグがついているオブジェクトを目的地に設定
            if (In_Parent.GetChild(i).CompareTag("HandCard"))
            {
                m_TargetPoint = In_Parent.GetChild(i).position;
                EndSetting = true;
            }
            // 2つの設定が完了したらループを抜ける
            if (StartSetting && EndSetting) break;
        }

        // 変化するテクスチャを設定
        m_ChangeSprite = In_Sprite;

        // 経過時間を初期化
        m_ElapsedTime = 0.0f;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        /*
         * 移動
         */
        transform.position = Vector3.Lerp(transform.position, m_TargetPoint, m_ElapsedTime / m_MaxTime);

        /*
         * 回転
         */
        float rotateY = 0.0f;
        float rotateZ = 0.0f;
        float time = 0.0f;
        if (!m_TextureChangeFlag)
        {
            time = m_ElapsedTime * 2.5f;
            rotateY = Mathf.Lerp(0.0f, 90.0f, time * 2);
            rotateZ = Mathf.Lerp(0.0f, 90.0f, time);
        }
        else
        {
            time = m_ElapsedTime * 3;
            rotateY = Mathf.Lerp(90.0f, 0.0f, time * 2);
            rotateZ = 0.0f;
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, rotateY, rotateZ);
        if (rotateY >= 90.0f && !m_TextureChangeFlag)
        {
            rotateY = 90.0f;
            // テクスチャを変更
            m_TextureChangeFlag = true;
            // 変更するテクスチャを設定
            GetComponent<Image>().sprite = m_ChangeSprite;
            // 回転のZ軸をリセット
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0.0f);

            // サイズをリセット
            transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(80.0f, 100.0f);
        }


        // 目的地に到達したらオブジェクトを破棄
        if (Vector3.Distance(transform.position, m_TargetPoint) < 0.1f)
        {
            Destroy(gameObject);
        }

        // 経過時間を加算
        m_ElapsedTime += Time.deltaTime;
    }
}
