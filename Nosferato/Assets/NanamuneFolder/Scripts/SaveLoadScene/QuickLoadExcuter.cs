using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickLoadExcuter : MonoBehaviour
{
    public static void ExcuteQuickLoad()
    {
        PublicStaticStatus.RowToSave = ExcuteLoad().ReferencedRow;
        SceneManager.LoadScene("ScenarioScene");
    }
    private static ScenarioDataToSave ExcuteLoad()
    {
        string key = $"ScenarioData{0}";
        if (PlayerPrefs.HasKey(key))
        {
            //jsonデータにしたやつをここで元に戻す
            string json = PlayerPrefs.GetString(key);//Slot番号からJSON持ってくる
            ScenarioDataToSave myScenarioDataToSave = JsonUtility.FromJson<ScenarioDataToSave>(json);//クラスをUserDataToSaveに戻す
            Debug.Log("LoadExcuted.Key:" + $"ScenarioData{0}" + "\n" + "Ref:" + myScenarioDataToSave.ReferencedRow + "\n" + "JSON:" + json);
            return (myScenarioDataToSave);
        }
        else
        {
            Debug.Log("PlayerUserDataが存在しません");
            return null;
        }
    }
}
