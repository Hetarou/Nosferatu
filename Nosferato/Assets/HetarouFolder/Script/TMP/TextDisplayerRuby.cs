using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using System.Xml;

public class TextDisplayerRuby : MonoBehaviour//PublicStaticStatusを更新することもする //SaveSceneはPublicStaticStatusを参照してセーブする
{
    private TextAsset csvFile; // CSVファイル
    private List<string[]> csvData = new List<string[]>(); // CSVファイルの中身を入れるリスト

    private TMP_Text messageText;            //文章のText
    public TMP_Text nameText;               //名前のText

    private string lastMassageText;

    [SerializeField]
    private TMP_Text backLogText;            //バックログ用のText
    public float charDelay = 0.05f;         // 文字送りの速さ
    public float autoDelay = 2f;

    public UnityEngine.UI.Image backgroudImage;

    public UnityEngine.UI.Image characterImage;
    public GameObject characterSprite;
    bool isCharacterSprite = false;

    public int messageIndex;

    public int RowNumber = 0;

    string threadNumber;
    string LastThreadNumber;

    private bool isTyping = false;
    private bool isAddWord = true;
    private bool isWaitRuby = false;

    void Start()
    {
        messageText = GetComponent<TMPro.TMP_Text>();

        csvFile = Resources.Load("MainScenario") as TextAsset;        // ResourcesにあるCSVファイルを格納
        StringReader reader = new StringReader(csvFile.text);         // TextAssetをStringReaderに変換

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine(); // 1行ずつ読み込む
            csvData.Add(line.Split(',')); // csvDataリストに追加する
        }

        threadNumber = csvData[RowNumber][0];
        LastThreadNumber = threadNumber;

        StartCoroutine(DisplayChar(csvData[RowNumber][2]));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && isTyping == true)
        {
            AnyDisplay();
        }
    }

    public void AnyDisplay()
    {
        RowNumber++;

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

        if (csvData[RowNumber][4] != null)
        {
            DisplayBackgroud();
        }

        if (csvData[RowNumber][3] != null)
        {
            DisplayCharacter();
        }
    }

    IEnumerator DisplayChar(string message)
    {

        

        backLogText.text += message + "\n";

        foreach (char c in message)
        {
            isAddWord = true;

            if (csvData[RowNumber][7].Length != 0)
            {
                if (c == '_')
                {   
                    if(!isWaitRuby)
                    {
                        isAddWord=false;
                        lastMassageText = messageText.text;
                    }
                    else
                    {
                        TmpRuby(csvData[RowNumber][7]);
                    }
                    isWaitRuby = !isWaitRuby;
                }
                else if (c == '|') TmpIcon();
            }

            if(isAddWord) messageText.text += c;  

            yield return new WaitForSeconds(charDelay);
        }

        /*if (csvData[RowNumber][7].Length != 0)
        {
            Debug.Log("呼ばれた");
            TmpRuby(csvData[RowNumber][7]);
        }*/
        Debug.Log(messageText.text);
        messageText.text += "\n";

        PublicStaticStatus.ReferencedRowToSave = RowNumber;
        PublicStaticStatus.DisplayedText = messageText.text;

        isTyping = true;
        
        if(ExacuteAuto.isAuto)
        {
            yield return new WaitForSeconds(autoDelay);
            AnyDisplay();
        }
    }

    public void TmpIcon()
    {
        isAddWord = false;
        messageText.text += "<sprite name=\"dictionary Y_0\">";
    }

    public void TmpRuby(string addRubyText)
    {
        isAddWord = false;
        messageText.SetTextAndExpandRuby(lastMassageText + addRubyText, fixedLineHeight: true, autoMarginTop: false);
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
    }
}
