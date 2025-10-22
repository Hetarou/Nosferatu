using UnityEngine;

public class LoadButton : MonoBehaviour
{
    [SerializeField]
    private int slotNum;

    public void OnClick()
    {
        Debug.Log("LoadButton Clicked");
        ExcuteLoad(slotNum);
    }

    public void ExcuteLoad(int slot)
    {
        string key = $"ScenarioData{slot}";
        if (PlayerPrefs.HasKey(key))
        {
            //jsonデータにしたやつをここで元に戻す
            string json = PlayerPrefs.GetString(key);//Slot番号からJSON持ってくる
            ScenarioDataToSave myScenarioDataToSave = JsonUtility.FromJson<ScenarioDataToSave>(json);//クラスをUserDataToSaveに戻す
            Debug.Log("セーブ" + slot + "をロードしました。" + "Ref" + myScenarioDataToSave.ReferencedRow);
        }
        else
        {
            Debug.Log("PlayerUserDataが存在しません");
        }
    }
}
