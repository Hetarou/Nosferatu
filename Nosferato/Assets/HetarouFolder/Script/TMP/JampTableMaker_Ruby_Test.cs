using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RichTagDiscriminator : MonoBehaviour
{
    // ルビタグを探すための正規表現
    private static readonly Regex RubyRegex = new Regex(@"<r=(?<ruby>[^>]+)>(?<kanji>[^<]+)</r>");
    // 他の装飾タグ（<color>など）を無視して文字数を数えるための正規表現
    private static readonly Regex TagRemoveRegex = new Regex(@"<[^>]+>");

    public Dictionary<int, int> MakeJumpTable(string targetMessage)
    {
        var jumpTable = new Dictionary<int, int>();

        // 1. 文章の中から全ての <r=...>...</r> を見つけ出す
        MatchCollection matches = RubyRegex.Matches(targetMessage);

        foreach (Match m in matches)
        {
            string ruby = m.Groups["ruby"].Value;
            string kanji = m.Groups["kanji"].Value;

            // 2. このタグより前にある「生テキスト」を取得
            string textBefore = targetMessage.Substring(0, m.Index);

            // 3. その中から余計なタグを除去して「表示上の開始位置」を特定
            int startPos = TagRemoveRegex.Replace(textBefore, "").Length + kanji.Length;

            // 4. ジャンプ先の位置を計算（漢字の数 + ルビの数）
            int endPos = startPos + ruby.Length;

            // 5. テーブルに登録（開始位置にきたら、終了位置まで飛ぶ）
            jumpTable[startPos] = endPos;
        }

        return jumpTable;
    }

    public string ReplaceRuby(string message)
    {
        return RubyRegex.Replace(message, "${kanji}");
    }
}