using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TextDisplayer : MonoBehaviour//PublicStaticStatusを更新することもする //SaveSceneはPublicStaticStatusを参照してセーブする
{
    private TextAsset csvFile; // CSVファイル
    private List<string[]> csvData = new List<string[]>(); // CSVファイルの中身を入れるリスト

    public TMP_Text messageText;            // 文章のText
    public TMP_Text nameText;               //名前のText
    public float charDelay = 0.05f;         // 文字送りの速さ

    public int messageIndex;

    int RowNumber = 0;

    string threadNumber;
    string LastThreadNumber;

    private bool isTyping = false;

    void Start()
    {
        csvFile = Resources.Load("Sample") as TextAsset;        // ResourcesにあるCSVファイルを格納
        StringReader reader = new StringReader(csvFile.text);   // TextAssetをStringReaderに変換

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine(); // 1行ずつ読み込む
            csvData.Add(line.Split(',')); // csvDataリストに追加する
        }

        threadNumber = csvData[0][0];
        LastThreadNumber = threadNumber;


        StartCoroutine(DisplayChar(csvData[0][1]));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            RowNumber++;
            DisplayText();
        }
    }

    IEnumerator DisplayChar(string message)
    {
        isTyping = true;

        foreach (char c in message)
        {
            messageText.text += c;
            yield return new WaitForSeconds(charDelay);
        }
        messageText.text += "\n";
        PublicStaticStatus.ReferencedRowToSave = RowNumber;
        PublicStaticStatus.DisplayedText= messageText.text;
        isTyping = false;
    }

    public void DisplayText()
    {
        Debug.Log("afdasfd");
        threadNumber = csvData[RowNumber][0];

        if (threadNumber != LastThreadNumber)
        {
            messageText.text = "";
            LastThreadNumber = threadNumber;
        }
        StartCoroutine(DisplayChar(csvData[RowNumber][1]));
    }
}
