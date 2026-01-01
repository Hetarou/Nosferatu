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
        //rowNumber = PublicStaticStatus.RowToSave;
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
        if (Input.GetKeyDown(KeyCode.F)) // Fキーを押したらテスト
        {
            Debug.Log("Test FadeOut");
            canvasGroup.DOFade(1f, 2.0f);
        }

        foreach (var key in targetKeys)
        {
            if (Input.GetKeyDown(key) && !isTyping && gameMode.modeRead && !PublicStaticStatus.IsEnter)
            {
                waitObj.SetActive(false);
                waitAnim.SetBool("isWaitAnim", false);
                rowNumber++;
                AnyDisplay();
                break; // 1つ見つかれば十分なのでループを抜ける
            }
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
        if (csvData[rowNumber][1] != null && (csvData[rowNumber][1] != csvData[rowNumber - 1][1] || csvData[rowNumber][0] != csvData[rowNumber - 1][0]))
        {
            nameText.text += csvData[rowNumber][1] + "\n";
            Debug.Log(nameText.text);
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

        //BackLogに直近20行を表示させる
        for (int i = 20; i > 0; i--)
        {
            if(rowNumber < i - 1) continue;

            if (csvData[rowNumber - (i - 1)][2] == "OP" || csvData[rowNumber - (i - 1)][2] == "ED" || rowNumber < i-1)
            {
                Debug.Log("この部分はスキップします");
                continue;
            }

            backLogText.text += csvData[rowNumber - (i - 1)][2];
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
            Debug.Log("起動bgmif");
            AudioClip clipBGM = Resources.Load<AudioClip>("BGM/" + csvData[rowNumber][6]);
            PlayBGM(clipBGM);
        }
        else if (csvData[rowNumber][6] == "stop")
        {
            Debug.Log("起動bgmstop");
            StopBGM(); 

        }
        else
        {
            Debug.Log("起動bgmelse");
            //さかのぼって取得
            for (int i = rowNumber; i > 0; i--)
            {
                
                if (csvData[i][6].Length != 0)
                {
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
        isTyping = true;
        isHidingRequested = false; // 初期化


        // ---------------------------------------------------------
        // 1. バックログ処理 (元のコードのまま)
        // ---------------------------------------------------------
        foreach (char c in message)
        {
            isAddWord = true;

            if (message == "OP") { Debug.Log(message); break; }

            if (csvData[rowNumber][7].Length != 0)
            {
                if (c == '_') WaitTmpRuby(); // 既存メソッドを利用
                else if (c == '|') TmpIcon();
            }

            if (isAddWord) backLogText.text += c;
        }
        backLogText.text += "\n";

        // バックログ生成で isWaitRuby フラグが変動している可能性があるためリセット
        // (※WaitTmpRubyがクラス変数のisWaitRubyを使っている場合への安全策)
        isWaitRuby = false;


        // ---------------------------------------------------------
        // 2. 表示処理 (ここをご要望のロジックに差し替え)
        // ---------------------------------------------------------

        bool isInsideRuby = false;   // ルビ内かどうかのフラグ
        int hiddenCharCount = 0;     // ルビの中に何文字隠れているかカウントする変数

        char[] chars = message.ToCharArray();

        // 文字列を一文字ずつ走査
        for (int i = 0; i < chars.Length; i++)
        {
            char c = chars[i];
            isAddWord = true;

            // --- OP / ED 分岐 (元のコードから移植) ---
            // OP部分の修正例
            if (message == "OP")
            {
                Debug.Log("OP開始：フェードアウトします");
                // blocksRaycastsをtrueにして、フェード中の誤クリックを防ぐ
                if (canvasGroup != null) canvasGroup.blocksRaycasts = true;

                // 2秒かけて真っ暗にし、完了を待ってからロード
                yield return canvasGroup.DOFade(1f, 2.0f).WaitForCompletion();

                SceneManager.LoadScene("OPScene");
                yield break;
            }
            else if (message == "ED")
            {
                Debug.Log(message);
                PublicStaticStatus.RowToSave = rowNumber + 1;
                PublicStaticStatus.IsCleared = true;
                yield return canvasGroup.DOFade(1f, 2.0f).WaitForCompletion();
                SceneManager.LoadScene("EDScene");
                break;
            }
            // ---------------------------------------

            // カスタムタグ判定
            if (csvData[rowNumber][7].Length != 0)
            {
                if (c == '_')
                {
                    isAddWord = false;

                    if (!isInsideRuby)
                    {
                        // ルビ開始
                        isInsideRuby = true;
                        // TmpRubyで使うために現在のテキストを保存
                        lastMessageText = messageText.text;
                        hiddenCharCount = 0; // カウントリセット
                    }
                    else
                    {
                        // ルビ終了
                        isInsideRuby = false;

                        // ★ここで「漢字」が塊でドンと出る
                        TmpRuby(csvData[rowNumber][7]);

                        // ★ここがリズム調整
                        // 隠していた文字数分だけ待機することで、リズムを合わせる
                        if (hiddenCharCount > 0)
                        {
                            yield return new WaitForSeconds(charDelay * hiddenCharCount);
                        }
                    }
                    continue; // 次のループへ
                }
                else if (c == '|')
                {
                    TmpIcon();
                }
            }

            // ルビの中身（漢字など）を隠す処理
            if (isInsideRuby)
            {
                isAddWord = false;
                hiddenCharCount++; // 隠している文字数を数える
            }

            // 通常文字の追加
            if (isAddWord)
            {
                messageText.text += c;
                yield return new WaitForSeconds(charDelay);
            }
        }


        // ---------------------------------------------------------
        // 3. 終了処理 (元のコードのまま)
        // ---------------------------------------------------------
        messageText.text += "\n";

        if (isHidingRequested)
        {
            isTyping = false;
            gameObject.SetActive(false); // 非表示実行
            isHidingRequested = false;
            yield break; // コルーチン終了
        }

        // 文字送り終了時
        waitObj.SetActive(true);
        waitAnim.SetBool("isWaitAnim", true);
        isTyping = false;
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
        audioSourceSE.PlayOneShot(SE);
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
