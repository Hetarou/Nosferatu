using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class TextDisplayerRuby : MonoBehaviour// PublicStaticStatusを更新することもする //SaveSceneはPublicStaticStatusを参照してセーブする
{
    private TextAsset csvFile; // CSVファイル
    private List<string[]> csvData = new List<string[]>(); // CSVファイルの中身を入れるリスト

    private TMP_Text messageText;            // 文章のText
    [SerializeField]
    private TMP_Text nameText;               // 名前のText

    private string lastMessageText;

    [SerializeField]
    private TMP_Text backLogText;            // バックログ用のText
    public float charDelay = 0.05f;          // 文字送りの速さ
    public float autoDelay = 2f;

    [SerializeField]
    private Image backgroudImage;
    [SerializeField]
    private Image characterImage;
    public GameObject characterSprite;
    bool isCharacterSprite = false;

    public int messageIndex;

    public int rowNumber;

    string threadNumber;
    string lastThreadNumber;

    public bool isTyping = false;
    private bool isAddWord = true;
    private bool isWaitRuby = false;

    public AudioSource audioSourceBGM;
    public AudioSource audioSourceSE;

    [SerializeField] private Animator waitAnim;
    [SerializeField] private GameObject waitObj;

    [SerializeField] private CanvasGroup canvasGroup;

    private bool isHidingRequested = false;

    private bool isSkipRequested = false;


    private static bool hasExcuted_BGM = false;

    //[SerializeField] private int skiped

    [Header("本編を進めるためのキー")]
    [SerializeField] private List<KeyCode> targetKeys = new List<KeyCode>();

    [Header("インスタンスを取得")]
    [SerializeField] GameMode gameMode;
    [SerializeField] FadeManager fadeManager;

    //[SerializeField] ButtonScript buttonScript;
    void Start()
    {
        //それぞれの必要なComponentを取得
        messageText = GetComponent<TMP_Text>();
        waitAnim = waitObj.GetComponent<Animator>();

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
        
        //backgroudImage = GetComponent<Image>();

        //StartCoroutine(DisplayChar(csvData[rowNumber][2]));　//文章自体のデバックが終わるまでコメントアウトしておきます
        AnyDisplay_Start();
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //Debug.Log($"\ncurrentRowNumber:{PublicStaticStatus.RowToSave}\nCGsCount:{DisplayCGsCalculater.CalculateCGsToDisplay(PublicStaticStatus.RowToSave)}");
            //canvasGroup.DOFade(1f, 2.0f);
        }

        foreach (var key in targetKeys)
        {
            if (Input.GetKeyDown(key) && !PublicStaticStatus.IsEnter)
            {
                // --- 追加・修正箇所 ---
                if (isTyping)
                {
                    // タイピング中にキーが押されたらスキップ
                    SkipTypewriter();
                }
                else if(gameMode.modeRead)
                {
                    // タイピング中でなければ次の行へ
                    waitObj.SetActive(false);
                    waitAnim.SetBool("isWaitAnim", false);
                    rowNumber++;
                    AnyDisplay();
                }
                // -----------------------
                break;
            }
        }

    }

    // スキップ用メソッド
    public void SkipTypewriter()
    {
        isSkipRequested = true;
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
        if (csvData[rowNumber][1] != null && (csvData[rowNumber][1] != csvData[rowNumber - 1][1] || csvData[rowNumber][0] != csvData[rowNumber - 1][0]))
        {
            nameText.text += csvData[rowNumber][1] + "\n";
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
            SimpleAudioManager_BGM.instance.PlayBGM(clipBGM);
        }
        else if (csvData[rowNumber][6] == "stop")
        {
            StopBGM();
        }
    }

    public void AnyDisplay_Start()//新しいスレッドのときと、スレッドの続きを読むときで分ける
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

        //BackLogに直近50行を表示させる
        for (int i = 50; i > 0; i--)
        {
            int currentIndex = rowNumber - i;

            if (currentIndex < 0 || csvData[currentIndex][2] == "OP" || csvData[currentIndex][2] == "ED")
            {
                continue;
            }

            backLogText.text += csvData[currentIndex][2];
            backLogText.text += "\n";
        }



        //名前
        if (csvData[rowNumber][1] != null && nameText.text != nameText.text + "\n")
        {
            nameText.text = csvData[rowNumber][1] + "\n";
        }
        else
        {
           
            //さかのぼって取得
            for ( int i=rowNumber; i>0; i--)
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
            //さかのぼって取得
            for (int i = rowNumber; i > 0; i--)
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
            ;
            //さかのぼって取得
            for (int i = rowNumber; i > 0; i--)
            {
                if (csvData[i][4].Length != 0)
                {  
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
            AudioClip clipBGM = Resources.Load<AudioClip>("BGM/" + csvData[rowNumber][6]);
            PlayBGM(clipBGM);
        }
        else if (csvData[rowNumber][6] == "stop")
        {
            StopBGM(); 

        }
        else
        {
            //さかのぼって取得
            for (int i = rowNumber; i > 0; i--)
            {
                
                if (csvData[i][6].Length != 0 && !hasExcuted_BGM)
                {
                    AudioClip clipBGM = Resources.Load<AudioClip>("BGM/" + csvData[i][6]);
                    PlayBGM(clipBGM);
                    hasExcuted_BGM = true;
                    break;
                }
            }
        }
        
        StartCoroutine(DisplayChar(csvData[rowNumber][2]));
    }

    public IEnumerator DisplayChar(string message)
    {
        isTyping = true;
        isHidingRequested = false;
        isSkipRequested = false; // 開始時にリセット

        // 1. バックログ処理 (変更なし)
        foreach (char c in message)
        {
            isAddWord = true;
            if (csvData[rowNumber][7].Length != 0)
            {
                if (c == '_') WaitTmpRuby();
                else if (c == '|') TmpIcon();
            }
            if (isAddWord) backLogText.text += c;
        }
        backLogText.text += "\n";
        isWaitRuby = false;

        // 2. 表示処理
        bool isInsideRuby = false;
        int hiddenCharCount = 0;
        char[] chars = message.ToCharArray();

        for (int i = 0; i < chars.Length; i++)
        {
            char c = chars[i];
            isAddWord = true;

            // ED 分岐
            if (message == "ED")
            {
                PublicStaticStatus.RowToSave = rowNumber + 1;
                BiggestRowSaver.EnableIsCleared();
                yield return canvasGroup.DOFade(1f, 2.0f).WaitForCompletion();
                SceneManager.LoadScene("EDScene");
                yield break; // 修正: breakだと後の処理が走るためyield break
            }
            else if(message == "タイトル画面へ")
            {
                BiggestRowSaver.EnableIsCleared();
                yield return canvasGroup.DOFade(1f, 2.0f).WaitForCompletion();
                SceneManager.LoadScene("TitleScene");
                yield break;
            }

            // カスタムタグ判定
            if (csvData[rowNumber][7].Length != 0)
            {
                if (c == '_')
                {
                    isAddWord = false;
                    if (!isInsideRuby)
                    {
                        isInsideRuby = true;
                        lastMessageText = messageText.text;
                        hiddenCharCount = 0;
                    }
                    else
                    {
                        isInsideRuby = false;
                        TmpRuby(csvData[rowNumber][7]);

                        // スキップフラグが立っていない時だけ待機
                        if (hiddenCharCount > 0 && !isSkipRequested)
                        {
                            yield return new WaitForSeconds(charDelay * hiddenCharCount);
                        }
                    }
                    continue;
                }
                else if (c == '|')
                {
                    TmpIcon();
                }
            }

            // ルビ内をカウント
            if (isInsideRuby)
            {
                isAddWord = false;
                hiddenCharCount++;
            }

            // 通常文字の表示
            if (isAddWord)
            {
                messageText.text += c;

                // スキップフラグが立っていない時だけ待機
                if (!isSkipRequested)
                {
                    yield return new WaitForSeconds(charDelay);
                }
            }
        }

        // 3. 終了処理
        messageText.text += "\n";

        if (isHidingRequested)
        {
            isTyping = false;
            gameObject.SetActive(false);
            isHidingRequested = false;
            yield break;
        }

        // 文字送り終了時
        waitObj.SetActive(true);
        waitAnim.SetBool("isWaitAnim", true);
        isTyping = false;
        isSkipRequested = false; // 終了時にフラグを戻す
        BiggestRowSaver.SaveIfBiggestRow();
    }

    public void RequestHide()
    {
        isHidingRequested = true;

        // もしタイプライター中でなければ、すぐに非表示にする
        if (!isTyping)
        {
            Debug.Log("ImmediateHide");
            isHidingRequested = false;
            gameObject.SetActive(false);
        }
        // (タイプライター中の場合は、DisplayChar の終了処理に任せる)
    }

    public void WaitTmpRuby()
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
    }
    //辞書
    public void TmpIcon()
    {
        isAddWord = false;
        messageText.text += "<sprite name=\"MagnifyingGlass_0\">";
    }

    //るび
    public void TmpRuby(string addRubyText)
    {
        isAddWord = false;
        messageText.SetTextAndExpandRuby(lastMessageText + addRubyText, fixedLineHeight: true, autoMarginTop: false);
        isWaitRuby = !isWaitRuby;
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
        else if (csvData[myRowNumber][3] == "なし")
        {

            isCharacterSprite = false;
            characterSprite.SetActive(false);
        }
    }

    public void DisplayBackgroud(int myRowNumber)
    {
        
        if (csvData[myRowNumber][4] == "カット")
        {
            ChangeBlack();
        }
        else
        {
            if(IsBlack())
            {
                ChangeWhite();
            }
            Sprite sprite = Resources.Load<Sprite>("Background/" + csvData[myRowNumber][4]);
            if (sprite != null)
            {
                backgroudImage.sprite = sprite;
            }
            else
            {
                Debug.LogError("指定した"+ csvData[myRowNumber][4] + "のスプライトが見つかりません");
            }
        }
    }

    private void ChangeBlack()
    {
        backgroudImage.color = Color.black;
    }

    private void ChangeWhite()
    {
        backgroudImage.color = Color.white;
    }

    private bool IsBlack()
    {
        return backgroudImage.color == Color.black;
    }

    public void PlaySE(AudioClip SE)
    {
        SimpleAudioManager_SE.instance.PlaySE(SE);
    }
    
    public void PlayBGM(AudioClip BGM, bool loop = true)
    {
        SimpleAudioManager_BGM.instance.PlayBGM(BGM);
    }
    public void StopBGM()
    {
        audioSourceBGM.Stop();
    }
}
