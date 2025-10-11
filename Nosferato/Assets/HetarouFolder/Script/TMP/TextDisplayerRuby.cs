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

        //データをロードする
        rowNumber = PublicStaticStatus.RowToSave;
        Debug.Log(rowNumber);

        csvFile = Resources.Load("MainScenario") as TextAsset;        // ResourcesにあるCSVファイルを格納
        StringReader reader = new StringReader(csvFile.text);         // TextAssetをStringReaderに変換

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine(); // 1行ずつ読み込む
            csvData.Add(line.Split(',')); // csvDataリストに追加する
        }

        threadNumber = csvData[rowNumber][0];
        lastThreadNumber = threadNumber;

        //StartCoroutine(DisplayChar(csvData[rowNumber][2]));
        AnyDisplay_Start();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && isTyping == false )
        {
            rowNumber++;
            AnyDisplay();
        }
    }

    public void AnyDisplay()//新しいスレッドのときと、スレッドの続きを読むときで分ける
    {
        isTyping = true;

        threadNumber = csvData[rowNumber][0];

        //新しいスレッドならテキスト更新//セーブ
        if (threadNumber != lastThreadNumber)
        {
            messageText.text = "";
            nameText.text = "";
            lastThreadNumber = threadNumber;
            
            //セーブする
            PublicStaticStatus.RowToSave = rowNumber;
        }

        //名前
        if (csvData[rowNumber][1] != null)
        {
            nameText.text = csvData[rowNumber][1];
        }

        StartCoroutine(DisplayChar(csvData[rowNumber][2]));

        //立ち絵
        if (csvData[rowNumber][3].Length != 0)
        {
            DisplayCharacter(rowNumber);
        }

        //背景
        if (csvData[rowNumber][4].Length != 0)
        {
            DisplayBackgroud(rowNumber);
        }

        //SE
        if (csvData[rowNumber][5].Length != 0)
        {
            AudioClip clipSE = Resources.Load<AudioClip>("SE/" + csvData[rowNumber][5]);
            PlaySE(clipSE);
        }

        //BGM
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

    public void AnyDisplay_Start()//新しいスレッドのときと、スレッドの続きを読むときで分ける
    {
        Debug.Log(rowNumber);
        isTyping = true;

        threadNumber = csvData[rowNumber][0];

        //新しいスレッドならテキスト更新//セーブ
        if (threadNumber != lastThreadNumber)
        {
            messageText.text = "";
            nameText.text = "";
            lastThreadNumber = threadNumber;

            //セーブする
            PublicStaticStatus.RowToSave = rowNumber;
        }

        //名前
        if (csvData[rowNumber][1] != null)
        {
            Debug.Log("起動した01");
            nameText.text = csvData[rowNumber][1];
        }
        else
        {
            //さかのぼって取得
            for( int i=rowNumber; i<0; i--)
            {
                if (csvData[i][1].Length != 0)
                {
                    nameText.text = csvData[i][1];
                    break;
                }
            }
        }

        

        //立ち絵
        if (csvData[rowNumber][3].Length != 0)
        {
            DisplayCharacter(rowNumber);
        }
        else
        {
            Debug.Log("起動した02");

            //さかのぼって取得
            for (int i = rowNumber; i < 0; i--)
            {
                if (csvData[i][3].Length != 0)
                {
                    
                    DisplayCharacter(i);
                    break;
                }
            }
        }

        //背景
        if (csvData[rowNumber][4].Length != 0)
        {
            DisplayBackgroud(rowNumber);
        }
        else
        {
            //さかのぼって取得
            for (int i = rowNumber; i < 0; i--)
            {
                if (csvData[i][4].Length != 0)
                {
                    Debug.Log("起動した03");
                    DisplayBackgroud(i);
                    break;
                }
            }
        }

        //SE
        if (csvData[rowNumber][5].Length != 0)
        {
            AudioClip clipSE = Resources.Load<AudioClip>("SE/" + csvData[rowNumber][5]);
            PlaySE(clipSE);
        }
        //elseはいらないか

        //BGM
        if (csvData[rowNumber][6].Length != 0)
        {
            Debug.Log("起動した01");
            AudioClip clipBGM = Resources.Load<AudioClip>("BGM/" + csvData[rowNumber][6]);
            PlayBGM(clipBGM);
        }
        else if (csvData[rowNumber][6] == "stop")
        {
            StopBGM(); 
            Debug.Log("起動した02");

        }
        else
        {
            //さかのぼって取得
            for (int i = rowNumber; i < 0; i--)
            {
                if (csvData[i][6].Length != 0)
                {
                    Debug.Log("起動した03");
                    
                    AudioClip clipBGM = Resources.Load<AudioClip>("BGM/" + csvData[i][6]);
                    PlayBGM(clipBGM);
                    break;
                }
            }
        }
        
        StartCoroutine(DisplayChar(csvData[rowNumber][2]));
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

        //PublicStaticStatus.ReferencedRowToSave = rowNumber;
        //PublicStaticStatus.DisplayedText = messageText.text;

        // 文字送り終了時
        isTyping = false;
        Debug.Log("TypingFinished");
    }


    //辞書
    public void TmpIcon()
    {
        isAddWord = false;
        messageText.text += "<sprite name=\"dictionary Y_0\">";
    }

    //るび
    public void TmpRuby(string addRubyText)
    {
        isAddWord = false;
        messageText.SetTextAndExpandRuby(lastMessageText + addRubyText, fixedLineHeight: true, autoMarginTop: false);
    }

    public void DisplayCharacter(int myRowNumber)
    {
        Sprite sprite = Resources.Load<Sprite>("Character/" + csvData[myRowNumber][3]);

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

    public void DisplayBackgroud(int myRowNumber)
    {
        Sprite sprite = Resources.Load<Sprite>("Background/" + csvData[myRowNumber][4]);

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
