using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour, IPointerEnterHandler//参考TextDisplayerRuby
{
    [SerializeField]
    private int slotNum;
    private TextAsset csvFile;
    private List<string[]> csvData = new List<string[]>();
    [SerializeField]
    GameObject thumbnailObj;
    [SerializeField]
    private TMP_Text dateText;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text scenarioText;
    [SerializeField]
    private Image backgroudImage;
    [SerializeField]
    GameObject characterSprite;

    [SerializeField]
    AudioClip audioClip0;
    [SerializeField]
    AudioClip audioClip1;
    [SerializeField]
    AudioClip audioClip2;

    private void Start()
    {
        //MainScenario格納
        csvFile = Resources.Load("MainScenario") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            csvData.Add(line.Split(','));
        }
        Debug.Log(csvData.Count);

        if (ExcuteLoad(slotNum) != null)
        {
            ChangeThumbnail(ExcuteLoad(slotNum).ReferencedRow,ExcuteLoad(slotNum).SavedDate);
        }
        else
        {
            thumbnailObj.SetActive(false);
        }
    }
    public void OnClick()
    {
        Debug.Log("LoadButton Clicked");
        if (ExcuteLoad(slotNum) != null)
        {
            SimpleAudioManager_SE.instance.PlaySE(audioClip0);
            PublicStaticStatus.RowToSave= ExcuteLoad(slotNum).ReferencedRow;
            SceneManager.LoadScene("ScenarioScene");
        }
        else
        {
            SimpleAudioManager_SE.instance.PlaySE(audioClip2);
            Debug.Log("空だよ");
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SimpleAudioManager_SE.instance.PlaySE(audioClip1);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K)){ExcuteSave(1);}
        //if (Input.GetKeyDown(KeyCode.L)){OnLoad(1);}
        //if (Input.GetKeyDown(KeyCode.M)){ExcuteSave(2);}
        //if (Input.GetKeyDown(KeyCode.N)){OnLoad(2);}
        //if (Input.GetKeyDown(KeyCode.F)) { PublicStaticStatus.RowToSave = 140; }
        //if (Input.GetKeyDown(KeyCode.G)) { PublicStaticStatus.RowToSave = 160; }
    }
    private void ExcuteSave(int slot)
    {
        ScenarioDataToSave myScenarioDataToSave = new ScenarioDataToSave()
        {
            ReferencedRow = PublicStaticStatus.RowToSave,
            SavedDate = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        };
        string json = JsonUtility.ToJson(myScenarioDataToSave, true);
        string key = $"ScenarioData{slot}";
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();

        Debug.Log("SaveExcuted.Key:" + $"ScenarioData{slot}" + "\n" + "Ref:" + myScenarioDataToSave.ReferencedRow + "\n" + "JSON:" + json);
    }
    private ScenarioDataToSave ExcuteLoad(int slot)
    {
        string key = $"ScenarioData{slot}";
        if (PlayerPrefs.HasKey(key))
        {
            //jsonデータにしたやつをここで元に戻す
            string json = PlayerPrefs.GetString(key);//Slot番号からJSON持ってくる
            ScenarioDataToSave myScenarioDataToSave = JsonUtility.FromJson<ScenarioDataToSave>(json);//クラスをUserDataToSaveに戻す
            Debug.Log("LoadExcuted.Key:" + $"ScenarioData{slot}" + "\n" + "Ref:" + myScenarioDataToSave.ReferencedRow + "\n" + "JSON:" + json);
            return (myScenarioDataToSave);
        }
        else
        {
            Debug.Log("PlayerUserDataが存在しません");
            return null;
        }
    }


    private void ChangeThumbnail(int myRowNumber,string savedDate)
    {
        //日付テキスト
        dateText.text = savedDate;
        //本文テキスト
        scenarioText.text = csvData[myRowNumber][2];
        //名前テキスト
        if (csvData[myRowNumber][1] != null && nameText.text != nameText.text + "\n")
        {
            nameText.text = csvData[myRowNumber][1] + "\n";
        }
        else
        {
            //さかのぼって取得
            for (int i = myRowNumber; i > 0; i--)
            {
                if (csvData[i][1].Length != 0)
                {
                    nameText.text = csvData[i][1];
                    break;
                }
            }
        }
        //立ち絵
        if (csvData[myRowNumber][3].Length != 0)
        {
            DisplayCharacter(myRowNumber);
        }
        else
        {
            //さかのぼって取得
            for (int i = myRowNumber; i > 0; i--)
            {
                if (csvData[i][3].Length != 0)
                {
                    DisplayCharacter(i);
                    break;
                }
            }
        }
        //背景
        if (csvData[myRowNumber][4].Length != 0)
        {
            DisplayBackgroud(myRowNumber);
        }
        else
        {
            //さかのぼって取得
            for (int i = myRowNumber; i > 0; i--)
            {
                if (csvData[i][4].Length != 0)
                {
                    DisplayBackgroud(i);
                    break;
                }
            }
        }

    }
    private void DisplayCharacter(int myRowNumber)
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
        else { Debug.LogError("キャラ参照なし"); }
    }

    private void DisplayBackgroud(int myRowNumber)
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
}
