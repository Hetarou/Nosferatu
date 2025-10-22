using UnityEngine;

public class SaveButton : MonoBehaviour
{
    [SerializeField]
    private int slotNum;
    
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K)){ExcuteSave(1);}
        //if (Input.GetKeyDown(KeyCode.L)){OnLoad(1);}
        //if (Input.GetKeyDown(KeyCode.M)){ExcuteSave(2);}
        //if (Input.GetKeyDown(KeyCode.N)){OnLoad(2);}
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
            ReferencedRow = PublicStaticStatus.ReferencedRowToSave
        };


        string json = JsonUtility.ToJson(myScenarioDataToSave, true);
        string key = $"ScenarioData{slot}";
        Debug.Log("セーブ" + slot + "にセーブしました" + "Ref" + myScenarioDataToSave.ReferencedRow);
        Debug.Log(json);// jsonデータにできたかどうか確認

        //jsonデータにしたやつらをここに格納
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();

        //サムネイルを変更する
        //へたちゃんから奪う
    }
}
