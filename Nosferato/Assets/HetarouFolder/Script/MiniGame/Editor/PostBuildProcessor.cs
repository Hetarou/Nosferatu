using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class PostBuildProcessor
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        // 1. コピー元：プロジェクトルートにある ExternalFiles/W7 を指定
        // System.Environment.CurrentDirectory はプロジェクトの親フォルダ（Assetsがある場所）を指します
        string sourcePath = Path.Combine(System.Environment.CurrentDirectory, "ExternalFiles", "W7");

        // 2. コピー先：ビルドされた .exe の隣にある _Data/StreamingAssets/W7
        string buildRoot = Path.GetDirectoryName(pathToBuiltProject);
        string dataFolderName = Path.GetFileNameWithoutExtension(pathToBuiltProject) + "_Data";
        string targetPath = Path.Combine(buildRoot, dataFolderName, "StreamingAssets", "W7");

        Debug.Log($"[PostBuild] コピー開始...");
        Debug.Log($"Source: {sourcePath}");
        Debug.Log($"Target: {targetPath}");

        // 3. フォルダごとコピーを実行
        if (Directory.Exists(sourcePath))
        {
            // StreamingAssets フォルダがまだなければ作成
            string streamingAssetsFolder = Path.GetDirectoryName(targetPath);
            if (!Directory.Exists(streamingAssetsFolder))
            {
                Directory.CreateDirectory(streamingAssetsFolder);
            }

            // コピー実行（既存のファイルは上書き）
            CopyDirectory(sourcePath, targetPath);
            Debug.Log("<color=green>ミニゲームのコピーが正常に完了しました！</color>");
        }
        else
        {
            Debug.LogError($"<color=red>コピー失敗：</color>元データが見つかりません。パスを確認してください: {sourcePath}");
        }
    }

    // ディレクトリを再帰的にコピーする補助関数
    private static void CopyDirectory(string sourceDir, string destDir)
    {
        Directory.CreateDirectory(destDir);

        // ファイルをコピー
        foreach (var file in Directory.GetFiles(sourceDir))
        {
            string destFile = Path.Combine(destDir, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }

        // サブフォルダをコピー
        foreach (var directory in Directory.GetDirectories(sourceDir))
        {
            string destSubDir = Path.Combine(destDir, Path.GetFileName(directory));
            CopyDirectory(directory, destSubDir);
        }
    }
}