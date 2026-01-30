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

    [SerializeField] private AudioClip clickScenarioSE;

    [Header("本編を進めるためのキー")]
    [SerializeField] private List<KeyCode> targetKeys = new List<KeyCode>();

    [Header("インスタンスを取得")]
    [SerializeField] GameMode gameMode;
    [SerializeField] FadeManager fadeManager;
    [SerializeField] private RichTagDiscriminator richTagDiscriminator;

    private int currentVisibleCharacters;
    private Dictionary<int, int> jumpTable = new Dictionary<int, int>();

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
                SimpleAudioManager_SE.instance.PlaySE(clickScenarioSE);
                if (isTyping)
                {
                    SkipTypewriter();
                }
                else if(gameMode.modeRead)
                {
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
            messageText.maxVisibleCharacters = 0;
            currentVisibleCharacters = 0;

            //セーブする
            PublicStaticStatus.RowToSave = rowNumber;
        }
        else
        {
            messageText.maxVisibleCharacters = currentVisibleCharacters;
        }

        //名前
        if (csvData[rowNumber][1] != null && (csvData[rowNumber][1] != csvData[rowNumber - 1][1] || csvData[rowNumber][0] != csvData[rowNumber - 1][0]))
        {
            nameText.text += csvData[rowNumber][1] + "\n";
        }

        StartTypewriter(csvData[rowNumber][2]);

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

    private void StartTypewriter(string message)
    {
        jumpTable = richTagDiscriminator.MakeJumpTable(messageText.text + message);
        foreach (var pair in jumpTable)
        {
            Debug.Log($"Key: {pair.Key}, Value: {pair.Value}");
        }
        messageText.SetTextAndExpandRuby(messageText.text + message, fixedLineHeight: true, autoMarginTop: false);

        //BackLogの適応
        backLogText.text += richTagDiscriminator.ReplaceRuby(message);
        backLogText.text += "\n";

        if (message == "ED")
        {
            StartCoroutine(EndRoutine(rowNumber));
        }
        else if (message == "タイトル画面へ")
        {
            StartCoroutine(BackTitleRoutine());
        }
        else
        {
            StartCoroutine(DisplayChar());
        }
    }

    private IEnumerator EndRoutine(int rowNumber)
    {
        PublicStaticStatus.RowToSave = rowNumber + 1;
        BiggestRowSaver.EnableIsCleared();

        // 1. フェード開始し、終わるまで待機
        // DOFade().WaitForCompletion() を yield return するのがポイント
        yield return canvasGroup.DOFade(1f, 2.0f).WaitForCompletion();

        // 2. フェードが終わったらシーン遷移
        SceneManager.LoadScene("EDScene");
    }

    private IEnumerator BackTitleRoutine()
    {
        BiggestRowSaver.EnableIsCleared();
        yield return canvasGroup.DOFade(1f, 2.0f).WaitForCompletion();
        SceneManager.LoadScene("TitleScene");
        yield break;
    }

    public void AnyDisplay_Start()//新しいスレッドのときと、スレッドの続きを読むときで分ける
    {
        isTyping = true;

        currentVisibleCharacters = 0;
        messageText.maxVisibleCharacters = currentVisibleCharacters;

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

            backLogText.text += richTagDiscriminator.ReplaceRuby(csvData[currentIndex][2]);
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
        for (int i = rowNumber; i > 0; i--)
        {
            if (csvData[rowNumber][5].Length != 0)
            {
                AudioClip clipSE = Resources.Load<AudioClip>("SE/" + csvData[rowNumber][5]);
                PlaySE(clipSE);
            }
        }

        //さかのぼって取得
        for (int i = rowNumber; i > 0; i--)
        {

            if (csvData[i][6].Length != 0)
            {
                if (csvData[rowNumber][6] == "stop")
                {
                    StopBGM();
                }
                else
                {
                    AudioClip clipBGM = Resources.Load<AudioClip>("BGM/" + csvData[i][6]);
                    PlayBGM(clipBGM);
                }
                break;
            }
        }



        StartTypewriter(csvData[rowNumber][2]);
    }

    public IEnumerator DisplayChar()
    {
        // 開始時にリセット
        isTyping = true;
        isHidingRequested = false;
        isSkipRequested = false;

        /*// 2. 表示処理
        
        char[] chars = message.ToCharArray();


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
        BiggestRowSaver.SaveIfBiggestRow();*/

        messageText.ForceMeshUpdate();
        int totalCharacters = messageText.textInfo.characterCount;

        for (int i = currentVisibleCharacters; i <= totalCharacters; i++)
        {
            if (jumpTable.TryGetValue(i, out int endPos))
            {
                i = endPos;
            }
            else if (isSkipRequested)
            {
                i = totalCharacters;
            }
            messageText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(charDelay);
        }

        messageText.text += "\n";
        currentVisibleCharacters = messageText.maxVisibleCharacters;

        waitObj.SetActive(true);
        waitAnim.SetBool("isWaitAnim", true);

        isTyping = false;
        isSkipRequested = false;
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
        SimpleAudioManager_BGM.instance.StopBGM();
    }
}
