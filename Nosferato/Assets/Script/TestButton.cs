using UnityEngine;
using UnityEngine.SceneManagement;

public class TestButton : MonoBehaviour
{
    [SerializeField]
    string SceneToLoad;
    [SerializeField]
    bool IsCommand=false;
    public void OnClick()
    {
        if (!IsCommand)
        {
            Debug.Log(SceneToLoad+"に遷移します");
            SceneManager.LoadScene(SceneToLoad);
        }
        else if (IsCommand)
        {
            ExcuteCommand();
        }
    }

    void ExcuteCommand()
    {
        Debug.Log("ゲーム終了");
#if UNITY_EDITOR
        // エディタで実行中なら再生モードを終了
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // ビルド版ならアプリケーションを終了
            Application.Quit();
#endif
    }
}
