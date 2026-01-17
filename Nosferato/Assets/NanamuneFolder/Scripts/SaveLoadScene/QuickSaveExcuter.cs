using Unity.VisualScripting;
using UnityEngine;

public class QuickSaveExcuter
{
    public static void ExcuteQuickSave()
    {
        ScenarioDataToSave myScenarioDataToSave = new ScenarioDataToSave()
        {
            ReferencedRow = PublicStaticStatus.RowToSave,
            SavedDate = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        };
        string json = JsonUtility.ToJson(myScenarioDataToSave, true);
        string key = $"ScenarioData{0}";
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();

        Debug.Log("SaveExcuted.Key:" + $"ScenarioData{0}" + "\n" + "Ref:" + myScenarioDataToSave.ReferencedRow + "\n" + "JSON:" + json);
    }
}
