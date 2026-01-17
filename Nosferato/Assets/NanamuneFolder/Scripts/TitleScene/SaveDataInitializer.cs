using UnityEngine;

public class SaveDataInitializer : MonoBehaviour
{
    void Awake()
    {
        if (PlayerPrefs.HasKey("UserData"))
        {
            //jsonデータにしたやつをここで元に戻す
            string json = PlayerPrefs.GetString("UserData");//Slot番号からJSON持ってくる
            UserDataToSave myUserDataToSave = JsonUtility.FromJson<UserDataToSave>(json);//クラスをUserDataToSaveに戻す
            PublicStaticStatus.ScreenMode = myUserDataToSave.ScreenMode;
            PublicStaticStatus.Font = myUserDataToSave.Font;
            PublicStaticStatus.ReadingSpeed = myUserDataToSave.ReadingSpeed;
            PublicStaticStatus.Volume = myUserDataToSave.Volume;
            PublicStaticStatus.BGMVolume = myUserDataToSave.BGMVolume;
            PublicStaticStatus.SEVolume = myUserDataToSave.SEVolume;
            PublicStaticStatus.BiggestRow = myUserDataToSave.BiggestRow;
            PublicStaticStatus.IsCleared = myUserDataToSave.IsCleared;
            Debug.Log("SaveExcuted.Key:" + "UserData" + "\n" + "JSON:" + json);
        }
        else
        {
            Debug.Log("PlayerUserDataが存在しません");
        }
    }
}
