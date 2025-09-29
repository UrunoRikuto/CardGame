using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    /// <summary>
    /// シーン切り替え処理
    /// </summary>
    /// <param name="SceneName">切り替えるシーンの名前</param>
    public void OnButton(string SceneName)
    {
        // シーン切り替え処理
        SceneManager.LoadScene(SceneName);
    }
}
