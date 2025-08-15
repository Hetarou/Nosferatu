using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SentenceReader : MonoBehaviour//このスクリプトがすることはテキスト表示
{
    private TextAsset csvFile; // CSVファイル
    private List<string[]> csvData = new List<string[]>(); // CSVファイルの中身を入れるリスト

    public TMP_Text messageText;            // 文章のText
    public TMP_Text nameText;               //名前のText
    public string messages;               　// 表示したい文章リスト
    public float charDelay = 0.05f;         // 文字送りの速さ

    public int messageIndex;

    int horizontalCounter = 0;

    string threadNumber;
    string nowThreadNumber;

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

        StartCoroutine(TypeMessage(csvData[0][1]));
        messages = csvData[0][1];
        threadNumber = csvData[0][0];
        nowThreadNumber = threadNumber;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            //ここからReadScenetence()
            horizontalCounter++;
            threadNumber = csvData[horizontalCounter][0];

            if(threadNumber != nowThreadNumber)
            {
                messages = "";
                nowThreadNumber = threadNumber;
            }

            StartCoroutine(TypeMessage(csvData[horizontalCounter][1]));
            messages = messages + "\n" + csvData[horizontalCounter][1];
            //ここまでReadScenetence()
        }
    }

    IEnumerator TypeMessage(string message)
    {
        isTyping = true;
        messageText.text = messages + "\n";

        foreach (char c in message)
        {
            messageText.text += c;
            yield return new WaitForSeconds(charDelay);
        }

        isTyping = false;
    }
    void SaveOrUpdatePublicStaticStatus()//この塊を呼び出す。//void ○○のところは、この塊が何をするかを名付ける    //PublicStaticStatusを更新してって欲しい。
    {
        Debug.Log("save");
        PublicStaticStatus.ReferencedRowToSave = 10;
    }

    public void ReadSentence()
    {
        //ここからReadScenetence()
        horizontalCounter++;
        threadNumber = csvData[horizontalCounter][0];

        if (threadNumber != nowThreadNumber)
        {
            messages = "";
            nowThreadNumber = threadNumber;
        }

        StartCoroutine(TypeMessage(csvData[horizontalCounter][1]));
        messages = messages + "\n" + csvData[horizontalCounter][1];
        //ここまでReadScenetence()
    }
}
