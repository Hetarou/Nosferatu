using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SentenceReader : MonoBehaviour
{
    private TextAsset csvFile; // CSVファイル
    private List<string[]> csvData = new List<string[]>(); // CSVファイルの中身を入れるリスト

    void Start()
    {
        csvFile = Resources.Load("Sample") as TextAsset; // ResourcesにあるCSVファイルを格納
        StringReader reader = new StringReader(csvFile.text); // TextAssetをStringReaderに変換

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine(); // 1行ずつ読み込む
            csvData.Add(line.Split(',')); // csvDataリストに追加する
        }

        for (int i = 0; i < csvData.Count; i++) // csvDataリストの条件を満たす値の数（全て）
        {
            // データの表示
            if (csvData[i][0].Length != 0) Debug.Log(csvData[i][0] +  csvData[i][1] + csvData[i][2]);
            else Debug.Log("nullです");
        }
    }

    void Update()
    {
        
    }
}
