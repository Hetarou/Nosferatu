using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Xml;

public class TextDisplayer_Test : MonoBehaviour//PublicStaticStatusを更新することもする //SaveSceneはPublicStaticStatusを参照してセーブする
{
    /*private TextAsset csvFile; // CSVファイル
    private List<string[]> csvData = new List<string[]>(); // CSVファイルの中身を入れるリスト

    public TMP_Text messageText;            //文章のText
    public TMP_Text nameText;               //名前のText
    [SerializeField]
    public TMP_Text backLogText;                //バックログ用のText
    public float charDelay = 0.05f;         // 文字送りの速さ

    public Image backgroudImage;

    public Image characterImage;
    public GameObject characterSprite;
    bool isCharacterSprite = false;

    public int messageIndex;

    int RowNumber = 0;

    string threadNumber;
    string LastThreadNumber;

    private bool isTyping = false;*/

    void Start()
    {
        /*csvFile = Resources.Load("MainScenario") as TextAsset;        // ResourcesにあるCSVファイルを格納
        StringReader reader = new StringReader(csvFile.text);   // TextAssetをStringReaderに変換

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine(); // 1行ずつ読み込む
            csvData.Add(line.Split(',')); // csvDataリストに追加する
        }

        threadNumber = csvData[0][0];
        LastThreadNumber = threadNumber;*/


       
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) /*&& isTyping == true*/)
        {
            //RowNumber++;
            //DisplayText();
            /*if (csvData[RowNumber][4] != null)
            {
                DisplayBackgroud();
            }
            if (csvData[RowNumber][3] != null)
            {
                DisplayCharacter();
            }*/
            StartCoroutine(DisplayChar("aaaaaaaa文字aaaaaa"));
        }
        if (Input.GetKeyDown(KeyCode.Z) /*&& isTyping == true*/)
        {
            //RowNumber++;
            //DisplayText();
            /*if (csvData[RowNumber][4] != null)
            {
                DisplayBackgroud();
            }
            if (csvData[RowNumber][3] != null)
            {
                DisplayCharacter();
            }*/
            DisplayText();
        }
    }

    IEnumerator DisplayChar(string message)
    {
        var tmpRuby = GetComponent<TextMeshProRuby>();
        //backLogText.text += message + "\n";

        foreach (char c in message)
        {
            //messageText.text += c;
            tmpRuby.Text += c;
            yield return new WaitForSeconds(0.05f);
            
        }
        rubi();

        //messageText.text += "\n";
        //PublicStaticStatus.ReferencedRowToSave = RowNumber;
        // PublicStaticStatus.DisplayedText = messageText.text;
        //isTyping = true;
    }

    public void DisplayText() 
    {
        var tmpRuby = GetComponent<TextMeshProRuby>();
        tmpRuby.Text = "";
    }

    public void rubi()
    {
        var tmpRuby = GetComponent<TextMeshProRuby>();
        tmpRuby.Text = "aaaaaaaa<r=もじ>文字</r>aaaaaa";
    }
    /*public void DisplayText()
    {
        isTyping = false;

        Debug.Log("afdasfd");
        threadNumber = csvData[RowNumber][0];

        if (threadNumber != LastThreadNumber)
        {
            messageText.text = "";
            nameText.text = "";
            LastThreadNumber = threadNumber;
        }

        if (csvData[RowNumber][1] != null)
        {
            nameText.text = csvData[RowNumber][1];
        }

        StartCoroutine(DisplayChar(csvData[RowNumber][2]));
    }
    public void DisplayCharacter()
    {
        Sprite sprite = Resources.Load<Sprite>("Character/" + csvData[RowNumber][3]);

        if (sprite != null)
        {
            if (isCharacterSprite == false)
            {
                isCharacterSprite = true;
                characterSprite.SetActive(isCharacterSprite);
            }
            characterImage.sprite = sprite;
        }
        else if (csvData[RowNumber][3] == "なし")
        {
            isCharacterSprite = false;
            characterSprite.SetActive(false);
        }
    }
    public void DisplayBackgroud()
    {
        Sprite sprite = Resources.Load<Sprite>("Background/" + csvData[RowNumber][4]);

        if (sprite != null)
        {
            backgroudImage.sprite = sprite;
        }
        else
        {
            Debug.LogError("指定した名前のスプライトが見つかりません");
        }
    }*/


}
