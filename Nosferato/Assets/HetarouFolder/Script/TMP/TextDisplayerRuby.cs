using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using System.Xml;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class TextDisplayerRuby : MonoBehaviour//PublicStaticStatusを更新することもする //SaveSceneはPublicStaticStatusを参照してセーブする
{
    private TextAsset csvFile; // CSVファイル
    private List<string[]> csvData = new List<string[]>(); // CSVファイルの中身を入れるリスト

    private TMP_Text messageText;            //文章のText
    public TMP_Text nameText;               //名前のText

    private string lastMessageText;

    [SerializeField]
    private TMP_Text backLogText;            //バックログ用のText
    public float charDelay = 0.05f;         // 文字送りの速さ
    public float autoDelay = 2f;

    public UnityEngine.UI.Image backgroudImage;

    public UnityEngine.UI.Image characterImage;
    public GameObject characterSprite;
    bool isCharacterSprite = false;

    public int messageIndex;

    public int rowNumber = 0;

    string threadNumber;
    string lastThreadNumber;

    public bool isTyping = false;
    private bool isAddWord = true;
    private bool isWaitRuby = false;

    public AudioSource audioSourceBGM;
    public AudioSource audioSourceSE;

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

        threadNumber = csvData[rowNumber][0];
        lastThreadNumber = threadNumber;

        StartCoroutine(DisplayChar(csvData[rowNumber][2]));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && isTyping == false )
        {
            AnyDisplay();
        }
    }

    public void AnyDisplay()
    {
        rowNumber++;

        isTyping = true;

        Debug.Log("afdasfd");
        threadNumber = csvData[rowNumber][0];

        if (threadNumber != lastThreadNumber)
        {
            messageText.text = "";
            nameText.text = "";
            lastThreadNumber = threadNumber;
        }

        if (csvData[rowNumber][1] != null)
        {
            nameText.text = csvData[rowNumber][1];
        }

        StartCoroutine(DisplayChar(csvData[rowNumber][2]));

        if (csvData[rowNumber][3].Length != 0)
        {
            DisplayCharacter();
        }

        if (csvData[rowNumber][4].Length != 0)
        {
            DisplayBackgroud();
        }

        if (csvData[rowNumber][5].Length != 0)
        {
            AudioClip clipSE = Resources.Load<AudioClip>("SE/" + csvData[rowNumber][5]);
            PlaySE(clipSE);
        }

        if (csvData[rowNumber][6].Length != 0)
        {
            AudioClip clipBGM = Resources.Load<AudioClip>("BGM/" + csvData[rowNumber][6]);
            PlayBGM(clipBGM);
        }
        else if (csvData[rowNumber][6] == "stop")
        {
            StopBGM();
        }
    }

    public IEnumerator DisplayChar(string message)
    {
        isTyping = true; // ← 先頭で true にする

        backLogText.text += message + "\n";

        foreach (char c in message)
        {
            isAddWord = true;

            if (csvData[rowNumber][7].Length != 0)
            {
                if (c == '_')
                {
                    if (!isWaitRuby)
                    {
                        isAddWord = false;
                        lastMessageText = messageText.text;
                    }
                    else
                    {
                        TmpRuby(csvData[rowNumber][7]);
                    }
                    isWaitRuby = !isWaitRuby;
                }
                else if (c == '|') TmpIcon();
            }

            if (isAddWord) messageText.text += c;

            yield return new WaitForSeconds(charDelay);
        }

        Debug.Log(messageText.text);
        messageText.text += "\n";

        PublicStaticStatus.ReferencedRowToSave = rowNumber;
        PublicStaticStatus.DisplayedText = messageText.text;

        // 文字送り終了時
        isTyping = false;
        Debug.Log("TypingFinished");
    }


    public void TmpIcon()
    {
        isAddWord = false;
        messageText.text += "<sprite name=\"dictionary Y_0\">";
    }

    public void TmpRuby(string addRubyText)
    {
        isAddWord = false;
        messageText.SetTextAndExpandRuby(lastMessageText + addRubyText, fixedLineHeight: true, autoMarginTop: false);
    }

    public void DisplayCharacter()
    {
        Sprite sprite = Resources.Load<Sprite>("Character/" + csvData[rowNumber][3]);

        if (sprite != null)
        {
            if (isCharacterSprite == false)
            {
                isCharacterSprite = true;
                characterSprite.SetActive(isCharacterSprite);
            }
            characterImage.sprite = sprite;
        }
        else if (csvData[rowNumber][3] == "なし")
        {
            isCharacterSprite = false;
            characterSprite.SetActive(false);
        }
    }

    public void DisplayBackgroud()
    {
        Sprite sprite = Resources.Load<Sprite>("Background/" + csvData[rowNumber][4]);

        if (sprite != null)
        {
            backgroudImage.sprite = sprite;
        }
        else
        {
            Debug.LogError("指定した名前のスプライトが見つかりません");
        }
    }

    public void PlaySE(AudioClip SE)
    {
        audioSourceSE.clip = SE;
        audioSourceSE.Play();
    }
    
    public void PlayBGM(AudioClip BGM, bool loop = true)
    {
        audioSourceBGM.clip = BGM;
        audioSourceBGM.loop = loop;
        audioSourceBGM.Play();
    }
    public void StopBGM()
    {
        audioSourceBGM.Stop();
    }
}
