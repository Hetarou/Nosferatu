using UnityEngine;

public class BiggestRowSaver : MonoBehaviour
{
    private static UserDataToSave myUserDataToSave;
    public static void SaveIfBiggestRow()
    {
        if(PublicStaticStatus.BiggestRow <= PublicStaticStatus.RowToSave)
        {
            PublicStaticStatus.BiggestRow = PublicStaticStatus.RowToSave;
            ExcuteSave();
        }
    }
    public static void EnableIsCleared()
    {
        if (!PublicStaticStatus.IsCleared)
        {
            PublicStaticStatus.IsCleared = true;
            ExcuteSave();
        }
    }
    private static void ExcuteSave()
    {
        string json;
        //ロード
        if (PlayerPrefs.HasKey("UserData"))
        {
            //jsonデータにしたやつをここで元に戻す
            json = PlayerPrefs.GetString("UserData");//Slot番号からJSON持ってくる
            myUserDataToSave = JsonUtility.FromJson<UserDataToSave>(json);//クラスをUserDataToSaveに戻す
            Debug.Log("LoadExcuted.Key:" + "UserData" + "\n" + "JSON:" + json);
        }
        else
        {
            Debug.LogWarning("PlayerUserDataが存在しません");
        }

        //書き換え
        myUserDataToSave.BiggestRow=PublicStaticStatus.BiggestRow;
        myUserDataToSave.IsCleared = PublicStaticStatus.IsCleared;

        //セーブ
        json = JsonUtility.ToJson(myUserDataToSave, true);
        string key = "UserData";
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
        Debug.Log("SaveExcuted.Key:" + "UserData" + "\n" + "JSON:" + json);

    }
}
