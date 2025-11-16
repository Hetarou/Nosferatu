using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{
    [SerializeField]
    private int slotNum;

    private int rowNumber;
    private TextAsset csvFile; // CSVファイル
    private List<string[]> csvData = new List<string[]>();
    [SerializeField]
    private TMP_Text nameText;//名前
    [SerializeField]
    private TMP_Text scenarioText;//シナリオ
    [SerializeField]
    private Image backgroudImage;//
    bool isCharacterSprite = false;
    //[SerializeField]
    //private Image characterImage;
    public GameObject characterSprite;//

    private void Start()
    {
        //ここデバッグ
        PublicStaticStatus.RowToSave = 100;
        
        csvFile = Resources.Load("MainScenario") as TextAsset;        // ResourcesにあるCSVファイルを格納
        StringReader reader = new StringReader(csvFile.text);         // TextAssetをStringReaderに変換

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine(); // 1行ずつ読み込む
            csvData.Add(line.Split(',')); // csvDataリストに追加する
        }
        Debug.Log(csvData.Count);

        UpdateSaveImage(); 
    }
    public void OnClick()
    {
        Debug.Log("SaveButton Clicked");
        ExcuteSave(slotNum);
    }
    public void ExcuteSave(int slot)
    {
        ScenarioDataToSave myScenarioDataToSave = new ScenarioDataToSave()
        {
            ReferencedRow = PublicStaticStatus.RowToSave
        };
        string json = JsonUtility.ToJson(myScenarioDataToSave, true);
        string key = $"ScenarioData{slot}";
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();

        Debug.Log("key:" + $"ScenarioData{slot}" + "\n" + "Ref:" + myScenarioDataToSave.ReferencedRow + "\n" + "JSON:" + json);

        UpdateSaveImage();
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K)){ExcuteSave(1);}
        //if (Input.GetKeyDown(KeyCode.L)){OnLoad(1);}
        //if (Input.GetKeyDown(KeyCode.M)){ExcuteSave(2);}
        //if (Input.GetKeyDown(KeyCode.N)){OnLoad(2);}
        if (Input.GetKeyDown(KeyCode.F)) { PublicStaticStatus.RowToSave = 140; }
        if (Input.GetKeyDown(KeyCode.G)) { PublicStaticStatus.RowToSave = 160; }
    }

    private void UpdateSaveImage()
    {
        rowNumber=PublicStaticStatus.RowToSave;
        ChangeThumbnail();
    }

    private void ChangeThumbnail()//新しいスレッドのときと、スレッドの続きを読むときで分ける
    {
        scenarioText.text = csvData[rowNumber][2];
        //名前
        if (csvData[rowNumber][1] != null && nameText.text != nameText.text + "\n")
        {
            nameText.text = csvData[rowNumber][1] + "\n";
        }
        else
        {
            //さかのぼって取得
            for (int i = rowNumber; i > 0; i--)
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

    }
    public void DisplayCharacter(int myRowNumber)
    {
        Sprite sprite = Resources.Load<Sprite>("Character/" + csvData[myRowNumber][3]);
        if (sprite != null)
        {
            characterSprite.SetActive(true);
            characterSprite.GetComponent<Image>().sprite = sprite;
        }
        else if (csvData[myRowNumber][3] == "なし")
        {
            characterSprite.SetActive(false);
        }
        else { Debug.LogError("それでええんか"); }
    }

    public void DisplayBackgroud(int myRowNumber)
    {
        if (csvData[myRowNumber][4] == "カット")
        {
            backgroudImage.color = Color.black;
        }
        else
        {
            if (backgroudImage.color == Color.black)
            {
                backgroudImage.color = Color.white;
            }
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
    }


    /*
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
    if (csvData[rowNumber][1] != null && nameText.text != nameText.text + "\n")
    {
        Debug.Log("起動NameIf");
        nameText.text = csvData[rowNumber][1] + "\n";
    }
    else
    {
        Debug.Log("起動NameElse");
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
        Debug.Log("起動bIf");
        DisplayBackgroud(rowNumber);
    }
    else
    {
        Debug.Log("起動bElse");
        //さかのぼって取得
        for (int i = rowNumber; i > 0; i--)
        {
            if (csvData[i][4].Length != 0)
            {
                Debug.Log("aha");
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
            Debug.Log("a");
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
     */

}
