using System.Diagnostics;
using System.IO;
using UnityEngine;

public class MiniGameLauncher : MonoBehaviour
{
    public void LaunchMiniGame()
    {
        string pathToExe = "";

        if (Application.isEditor)
        {
            // エディタ中：プロジェクト直下の ExternalFiles を見る
            // Path.Combineを使うと Windows のパス区切り(\)を自動で正しく処理してくれます
            pathToExe = Path.Combine(System.Environment.CurrentDirectory, "ExternalFiles", "W7", "Ynmg2.exe");
        }
        else
        {
            // ビルド後：StreamingAssets 内を見る
            pathToExe = Path.Combine(Application.streamingAssetsPath, "W7", "Ynmg2.exe");
        }

        if (!File.Exists(pathToExe))
        {
            UnityEngine.Debug.LogError("EXEが見つかりません。パスを確認してください:\n" + pathToExe);
            return;
        }

        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = pathToExe;
        startInfo.WorkingDirectory = Path.GetDirectoryName(pathToExe);

        try
        {
            Process.Start(startInfo);
            UnityEngine.Debug.Log("ミニゲームを起動しました！");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("起動失敗: " + e.Message);
        }
    }
}