using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ExcelReader : MonoBehaviour//このスクリプトのすることは「テキスト表示してね」とか「画像差し替えてね」とか、命令するスクリプト
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
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
        nowThreadNumber = threadNumber;*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            /*
            //何番目に画像名があったら、背景を差し替える
            myHaikeiReader.ReadHaikei();//またはChangeBackgroud();
            ChangeTathie();
            mySentenceReader.ReadSentence();*/
        }
    }

    /*void CangeBackground()
    {

    }*/
}
