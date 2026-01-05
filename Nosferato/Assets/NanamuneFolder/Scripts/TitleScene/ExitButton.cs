using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void OnClick()
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
