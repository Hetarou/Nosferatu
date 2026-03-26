using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextDisplayerRuby : MonoBehaviour
{
    #region 変数宣言 (インスペクターの紐付けが切れないよう、名前はそのままにしています)

    [Header("UI Components")]
    [SerializeField] private TMP_Text messageText;             // 文章のText
    [SerializeField] private TMP_Text nameText;                // 名前のText
    [SerializeField] private TMP_Text backLogText;             // バックログ用のText
    [SerializeField] private Image backgroudImage;
    [SerializeField] private Image characterImage;
    public GameObject characterSprite;
    [SerializeField] private GameObject waitObj;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Settings")]
    public float charDelay;          // 文字送りの速さ
    public float autoDelay = 2f;
    [SerializeField] private string CSVDataName;
    [SerializeField] private List<KeyCode> targetKeys = new List<KeyCode>(); // 本編を進めるためのキー
    [SerializeField] private AudioClip clickScenarioSE;

    [Header("System Instances")]
    [SerializeField] private GameMode gameMode;
    [SerializeField] private FadeManager fadeManager;
    [SerializeField] private FadeTitleController fadeTitle;
    [SerializeField] private RichTagDiscriminator richTagDiscriminator;

    public AudioSource audioSourceBGM;
    public AudioSource audioSourceSE;

    // 内部状態・データ
    private TextAsset csvFile;
    private List<string[]> csvData = new List<string[]>();
    private Dictionary<int, int> jumpTable = new Dictionary<int, int>();
    private Animator waitAnim;

    private float orizinFontSize = 30; // 元コードの変数名を維持
    private static float currentFontSize = -1;
    private static bool hasExcuted_BGM = false;

    public int rowNumber;
    public int messageIndex;
    private string threadNumber;
    private string lastThreadNumber;
    private string lastMessageText;
    private int currentVisibleCharacters;

    public bool isTyping = false;
    private bool isAddWord = true;
    private bool isWaitRuby = false;
    private bool isCharacterSprite = false;
    private bool isHidingRequested = false;
    private bool isSkipRequested = false;

    // CSVの列番号を定数化（何番目のデータか分かりやすくするため）
    private const int COL_THREAD = 0;
    private const int COL_NAME = 1;
    private const int COL_TEXT = 2;
    private const int COL_CHARA = 3;
    private const int COL_BG = 4;
    private const int COL_SE = 5;
    private const int COL_BGM = 6;
    private const int COL_PARAM = 7;

    #endregion


    #region Unity標準メソッド (Start / Update)

    void Start()
    {
        messageText = GetComponent<TMP_Text>();
        waitAnim = waitObj.GetComponent<Animator>();

        // データをロードする
        rowNumber = PublicStaticStatus.RowToSave;
        Debug.Log(rowNumber);

        csvFile = Resources.Load(CSVDataName) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            csvData.Add(line.Split(','));
        }

        threadNumber = csvData[rowNumber][COL_THREAD];
        lastThreadNumber = threadNumber;

        if (currentFontSize != -1)
        {
            messageText.fontSize = currentFontSize;
        }

        AnyDisplay_Start();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // デバッグ用（元コードのままコメントアウトを維持）
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
                else if (gameMode.CanRead)
                {
                    waitObj.SetActive(false);
                    waitAnim.SetBool("isWaitAnim", false);
                    rowNumber++;
                    AnyDisplay();
                }
                break;
            }
        }
    }

    #endregion


    #region シナリオ進行メインロジック

    // スキップ用メソッド
    public void SkipTypewriter()
    {
        isSkipRequested = true;
    }

    // 通常の行送り時の処理
    public void AnyDisplay()
    {
        isTyping = true;
        string[] currentRow = csvData[rowNumber];
        threadNumber = currentRow[COL_THREAD];

        UpdateThreadInfo();
        UpdateNameDisplay(currentRow);

        StartTypewriter(currentRow[COL_TEXT]);

        if (!string.IsNullOrEmpty(currentRow[COL_CHARA])) DisplayCharacter(rowNumber);
        if (!string.IsNullOrEmpty(currentRow[COL_BG])) DisplayBackgroud(rowNumber);
        if (!string.IsNullOrEmpty(currentRow[COL_SE])) PlaySEFromCSV(currentRow[COL_SE]);

        ProcessBGMCommand(currentRow);
    }

    // ロード直後・開始時の処理（さかのぼって状態を復元する）
    public void AnyDisplay_Start()
    {
        isTyping = true;
        currentVisibleCharacters = 0;
        messageText.maxVisibleCharacters = currentVisibleCharacters;

        string[] currentRow = csvData[rowNumber];
        threadNumber = currentRow[COL_THREAD];

        bool isSmall = false;
        if (csvData[rowNumber - 1][COL_TEXT] == "SMALL")
        {
            rowNumber--;
            ChangeFontSize(0.8f);
            isSmall = true;
        }

        UpdateThreadInfo();
        GenerateBacklog();

        RestoreNameDisplay();
        RestoreCharacterDisplay();
        RestoreBackgroundDisplay();
        RestoreSE();
        RestoreBGM();

        if (currentRow[COL_TEXT] == "" || isSmall) return;
        StartTypewriter(currentRow[COL_TEXT]);
    }

    #endregion


    #region 表示・状態更新の細分化メソッド

    private void UpdateThreadInfo()
    {
        if (threadNumber != lastThreadNumber)
        {
            messageText.text = "";
            nameText.text = "";
            lastThreadNumber = threadNumber;
            messageText.maxVisibleCharacters = 0;
            currentVisibleCharacters = 0;

            PublicStaticStatus.RowToSave = rowNumber;
        }
        else
        {
            messageText.maxVisibleCharacters = currentVisibleCharacters;
        }
    }

    private void UpdateNameDisplay(string[] currentRow)
    {
        // 1つ前の行から名前やスレッドが変わった場合のみ更新
        if (currentRow[COL_NAME] != null &&
           (rowNumber == 0 || currentRow[COL_NAME] != csvData[rowNumber - 1][COL_NAME] || currentRow[COL_THREAD] != csvData[rowNumber - 1][COL_THREAD]))
        {
            nameText.text += currentRow[COL_NAME] + "\n";
        }

        if (currentRow[COL_NAME] == "暮六") DisplayMode();
        else HideMode();
    }

    private void GenerateBacklog()
    {
        for (int i = 50; i > 0; i--)
        {
            int currentIndex = rowNumber - i;
            if (currentIndex < 0) continue;

            Debug.Log($"currentIndex:{currentIndex}");
            string param = csvData[currentIndex][COL_PARAM];
            string text = csvData[currentIndex][COL_TEXT];

            // デバッグログを出して、実際に何が読み込まれているか見る
            Debug.Log($"Index:{currentIndex} / Param:{param} / Text:{text}");
            if (param == "RESET")
            {
                Debug.Log("RESETを検出！BackLogを空にします");
                backLogText.text = "";
                continue;
            }

            if (IsSystemCommand(text)) continue;

            backLogText.text += richTagDiscriminator.ReplaceRuby(text) + "\n";

            
        }
    }

    // 前回のご相談箇所：バックログやテキスト表示から除外すべきコマンドを判定
    private bool IsSystemCommand(string text)
    {
        return text == "OP" || text == "ED" || text == "WAIT" || text == "FADEOUT" ||
               text == "FADEIN" || text == "DISPLAYTITLE" || text == "SMALL" ||
               text == "NORMAL" || text == "タイトル画面へ" || text == "";
    }

    #endregion


    #region 復元処理 (さかのぼり取得)

    private void RestoreNameDisplay()
    {
        if (!string.IsNullOrEmpty(csvData[rowNumber][COL_NAME]) && nameText.text != nameText.text + "\n")
        {
            nameText.text = csvData[rowNumber][COL_NAME] + "\n";
        }
        else
        {
            for (int i = rowNumber; i > 0; i--)
            {
                if (!string.IsNullOrEmpty(csvData[i][COL_NAME]))
                {
                    nameText.text = csvData[i][COL_NAME];
                    break;
                }
            }
        }
    }

    private void RestoreCharacterDisplay()
    {
        if (!string.IsNullOrEmpty(csvData[rowNumber][COL_CHARA]))
        {
            DisplayCharacter(rowNumber);
        }
        else
        {
            for (int i = rowNumber; i > 0; i--)
            {
                if (!string.IsNullOrEmpty(csvData[i][COL_CHARA]))
                {
                    DisplayCharacter(i);
                    break;
                }
            }
        }
    }

    private void RestoreBackgroundDisplay()
    {
        Debug.Log("RestoreBackgroundDisplay関数内に入りました");
        if (!string.IsNullOrEmpty(csvData[rowNumber][COL_BG]))
        {
            DisplayBackgroud(rowNumber);
        }
        else
        {
            for (int i = rowNumber; i > 0; i--)
            {
                if (!string.IsNullOrEmpty(csvData[i][COL_BG]))
                {
                    Debug.Log("段階1");
                    DisplayBackgroud(i);

                    break;
                }
            }
        }
    }

    private void RestoreSE()
    {
        // ※元のコードにあった「rowNumberを使い続けてループが無限に再生されるバグ」を修正し、
        // 過去の最新のSEを1回だけ取得するように直しています。
        /*for (int i = rowNumber; i > 0; i--)
        {
            if (csvData[i][COL_PARAM] == "STOP") break;
            if (!string.IsNullOrEmpty(csvData[i][COL_SE]))
            {
                PlaySEFromCSV(csvData[i][COL_SE]);
                break;
            }
        }*/

        PlaySEFromCSV(csvData[rowNumber][COL_SE]);
    }

    private void RestoreBGM()
    {
        for (int i = rowNumber; i > 0; i--)
        {
            // Trim() を使って、前後の余計な空白や改行を取り除く（超重要！）
            string bgmCommand = csvData[i][COL_BGM].Trim();

            if (!string.IsNullOrEmpty(bgmCommand))
            {
                // ★ csvData[rowNumber] ではなく、遡っている最中の bgmCommand (つまり csvData[i]) を判定する
                if (bgmCommand == "stop")
                {
                    Debug.Log("StopComandが実行されました！");
                    StopBGM();
                    break;
                }
                else if (bgmCommand == "VOLUME")
                {
                    Debug.Log("VOLUMEComandが実行されました！");

                    // ★ ここも rowNumber ではなく、過去の行 i のデータを渡す
                    string[] currentRow = csvData[i];
                    ProcessBGMCommand(currentRow);

                    // ※曲名を見つけるまでさらに過去に遡る必要があるため、ここでは break しない
                }
                else
                {
                    // ここには曲名（"BattleTheme"など）が入ってくる想定
                    Debug.Log($"EXCECUTEComand {bgmCommand} が実行されました！");
                    AudioClip clipBGM = Resources.Load<AudioClip>("BGM/" + bgmCommand);
                    PlayBGM(clipBGM);
                    break; // 曲を再生したら遡るのをやめる
                }
            }
        }
    }

    #endregion


    #region 音声ロジック (Audio)

    private void ProcessBGMCommand(string[] currentRow)
    {
        string bgmCommand = currentRow[COL_BGM];

        if (bgmCommand == "stop")
        {
            Debug.LogWarning($"Value: '{bgmCommand}', Length: {bgmCommand.Length}");
            StopBGM();
        }
        else if (bgmCommand == "VOLUME")
        {
            Debug.LogWarning($"Value: '{bgmCommand}', Length: {bgmCommand.Length}");
            Debug.LogWarning("HelloWorld");
            float volumeConst = float.Parse(currentRow[COL_PARAM]);
            SimpleAudioManager_BGM.instance.ChangeVolumeConst(volumeConst);
        }
        else if (bgmCommand == "FADEOUT")
        {
            Debug.LogWarning($"Value: '{bgmCommand}', Length: {bgmCommand.Length}");
            StartCoroutine(SimpleAudioManager_BGM.instance.FadeOutCoroutine());
        }
        else if (!string.IsNullOrEmpty(bgmCommand))
        {
            string bgmName = bgmCommand.Trim();
            AudioClip clipBGM = Resources.Load<AudioClip>("BGM/" + bgmName);

            if (clipBGM == null)
            {
                Debug.LogError($"【エラー】BGMが見つかりません！ パス: Assets/Resources/BGM/{bgmName}");
            }
            else
            {
                Debug.Log($"【成功】{bgmName} を読み込みました。再生します。");
                PlayBGM(clipBGM);
            }
        }
        else
        {
            Debug.Log("→ どこにも入りませんでした（文字数が0です）");
        }
    }

    private void PlaySEFromCSV(string seName)
    {
        AudioClip clipSE = Resources.Load<AudioClip>("SE/" + seName);
        PlaySE(clipSE);
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

    #endregion


    #region テキスト・演出コマンドロジック

    private void StartTypewriter(string message)
    {
        jumpTable = richTagDiscriminator.MakeJumpTable(messageText.text + message);
        foreach (var pair in jumpTable)
        {
            Debug.Log($"Key: {pair.Key}, Value: {pair.Value}");
        }

        messageText.SetTextAndExpandRuby(messageText.text + message, fixedLineHeight: true, autoMarginTop: false);

        switch (message)
        {
            case "DISPLAYTITLE":
                StartCoroutine(FadeTitle());
                break;

            case "WAIT":
                float waitTime = float.Parse(csvData[rowNumber][COL_PARAM]);
                StartCoroutine(WaitCoroutine(waitTime));
                break;

            case "ED":
                StartCoroutine(EndRoutine(rowNumber));
                break;

            case "タイトル画面へ":
                StartCoroutine(BackTitleRoutine());
                break;

            case "FADEIN":
                StartCoroutine(FadeInRoutine());
                break;

            case "FADEOUT":
                FadeOut();
                break;

            case "SMALL":
                ChangeFontSize(0.8f);
                break;

            case "NORMAL":
                ChangeFontSize(1.0f);
                break;

            default:
                // コマンドでない通常のテキストの場合のみバックログへ追加
                backLogText.text += richTagDiscriminator.ReplaceRuby(message);
                backLogText.text += "\n";
                StartCoroutine(DisplayChar());
                break;
        }
    }

    public IEnumerator DisplayChar()
    {
        isTyping = true;
        isHidingRequested = false;
        isSkipRequested = false;

        messageText.ForceMeshUpdate();
        int totalCharacters = messageText.textInfo.characterCount;
        float orizinCharDelay = charDelay; // 変数名維持

        Debug.Log(charDelay);

        if (csvData[rowNumber][COL_PARAM] == "SLOW")
        {
            charDelay *= 1.8f;
        }

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
        charDelay = orizinCharDelay;

        waitObj.SetActive(true);
        waitAnim.SetBool("isWaitAnim", true);

        isTyping = false;
        isSkipRequested = false;
        BiggestRowSaver.SaveIfBiggestRow();
    }

    private void ChangeFontSize(float sizeConst)
    {
        messageText.fontSize = orizinFontSize * sizeConst;
        currentFontSize = messageText.fontSize;
        rowNumber++;
        AnyDisplay();
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
            TmpRuby(csvData[rowNumber][COL_PARAM]);
        }
    }

    public void TmpIcon()
    {
        isAddWord = false;
        messageText.text += "<sprite name=\"MagnifyingGlass_0\">";
    }

    public void TmpRuby(string addRubyText)
    {
        isAddWord = false;
        messageText.SetTextAndExpandRuby(lastMessageText + addRubyText, fixedLineHeight: true, autoMarginTop: false);
        isWaitRuby = !isWaitRuby;
    }

    public void RequestHide()
    {
        isHidingRequested = true;
        if (!isTyping)
        {
            Debug.Log("ImmediateHide");
            isHidingRequested = false;
            gameObject.SetActive(false);
        }
    }

    #endregion


    #region ビジュアルロジック (立ち絵・背景・カラー)

    public void DisplayCharacter(int myRowNumber)
    {
        Sprite sprite = Resources.Load<Sprite>("Character/" + csvData[myRowNumber][COL_CHARA]);

        if (sprite != null)
        {
            if (isCharacterSprite == false)
            {
                isCharacterSprite = true;
                characterSprite.SetActive(isCharacterSprite);
            }
            characterImage.sprite = sprite;
        }
        else if (csvData[myRowNumber][COL_CHARA] == "なし")
        {
            isCharacterSprite = false;
            characterSprite.SetActive(false);
        }
    }

    private void HideMode()
    {
        characterImage.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    }

    private void DisplayMode()
    {
        characterImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public void DisplayBackgroud(int myRowNumber)
    {
        if (csvData[myRowNumber][COL_BG] == "DIM")
        {
            DimBackground();
        }
        else
        {
            if (csvData[myRowNumber][COL_BG] == "RETURN" || IsBlack())
            {
                ChangeWhite();
            }
            Sprite sprite = Resources.Load<Sprite>("Background/" + csvData[myRowNumber][COL_BG]);
            if (sprite != null)
            {
                backgroudImage.sprite = sprite;
            }
            else
            {
                Debug.LogError("指定した" + csvData[myRowNumber][COL_BG] + "のスプライトが見つかりません");
            }
        }
    }

    private void DimBackground()
    {
        backgroudImage.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
    }

    private void ChangeWhite()
    {
        backgroudImage.color = new Color(1f, 1f, 1f, 1.0f);
    }

    private bool IsBlack()
    {
        return backgroudImage.color == Color.black;
    }

    #endregion


    #region 演出コルーチン

    private IEnumerator FadeTitle()
    {
        yield return StartCoroutine(fadeTitle.CoroutineBranch(csvData[rowNumber][COL_PARAM]));
        rowNumber++;
        AnyDisplay();
    }

    private IEnumerator WaitCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        rowNumber++;
        AnyDisplay();
    }

    private IEnumerator EndRoutine(int rowNumber)
    {
        PublicStaticStatus.RowToSave = rowNumber + 1;
        BiggestRowSaver.EnableIsCleared();

        yield return canvasGroup.DOFade(1f, 2.0f).WaitForCompletion();

        SceneManager.LoadScene("EDScene");
    }

    private IEnumerator BackTitleRoutine()
    {
        BiggestRowSaver.EnableIsCleared();
        yield return canvasGroup.DOFade(1f, 2.0f).WaitForCompletion();
        SceneManager.LoadScene("TitleScene");
    }

    private IEnumerator FadeInRoutine()
    {
        if (csvData[rowNumber][COL_PARAM] == "START")
        {
            yield return StartCoroutine(fadeTitle.StartTitleFadeInCoroutine());
        }
        else
        {
            Debug.LogWarning("FadeOut");
            canvasGroup.alpha = 1f;
            Debug.LogWarning(csvData[rowNumber][COL_PARAM]);
            float fadeIn = float.Parse(csvData[rowNumber][COL_PARAM]);
            yield return canvasGroup.DOFade(fadeIn, 0.7f).WaitForCompletion();
            canvasGroup.blocksRaycasts = false;
        }

        rowNumber++;
        AnyDisplay();
    }

    private void FadeOut()
    {
        float FadeSpeed = 0.4f;

        switch (csvData[rowNumber][COL_PARAM])
        {
            case "SLOW": FadeSpeed = 0.7f; break;
            case "NORMAL": FadeSpeed = 0.4f; break;
            case "FAST": FadeSpeed = 0.1f; break;
        }

        StartCoroutine(FadeOutRoutine(FadeSpeed));
    }

    private IEnumerator FadeOutRoutine(float speed)
    {
        Debug.LogWarning("FadeOut");
        rowNumber++;
        canvasGroup.blocksRaycasts = true;
        yield return canvasGroup.DOFade(1f, speed).WaitForCompletion();
        AnyDisplay();
    }

    #endregion
}