// この 'using' がスクリプトの先頭に必要です
using System.Diagnostics;
using System.IO; // Path.GetDirectoryName を使うために追加
using UnityEngine;

public class MiniGameLauncher : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            LaunchMiniGame();
        }
    }

    public void LaunchMiniGame()
    {
        // ビルドしたミニゲームの .exe ファイルへのフルパスを指定します
        string pathToExe = @"C:\Users\isida\Nosferatu\Nosferato\W7\Ynmg2";

        // Process.Start(pathToExe); 
        // ↑ のように直接実行すると、先ほどのエラーが出る可能性が高いです。

        // 確実な方法: ProcessStartInfo を使う
        ProcessStartInfo startInfo = new ProcessStartInfo();

        // 実行するファイル
        startInfo.FileName = pathToExe;

        //
        // 【最重要】
        // .exe があるフォルダを作業フォルダ(WorkingDirectory)に指定します。
        // これにより、.exe は自分の隣にある _Data フォルダを正しく見つけられます。
        //
        startInfo.WorkingDirectory = Path.GetDirectoryName(pathToExe);

        try
        {
            Process.Start(startInfo);
            UnityEngine.Debug.Log("ミニゲームを起動します。");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("起動に失敗: " + e.Message);
        }
    }
}