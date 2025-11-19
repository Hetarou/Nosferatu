using UnityEngine;

public class SaveLoadSystem_Config : MonoBehaviour
{
    public void LoadStatus()
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
            Debug.Log("SaveExcuted.Key:" + "UserData" + "\n" + "JSON:" + json);
        }
        else
        {
            Debug.Log("PlayerUserDataが存在しません");
        }
    }

    public void SaveStatus(string st,int num)
    {
        if (st == "ScreenMode") { PublicStaticStatus.ScreenMode = num; }
        if (st == "Font") { PublicStaticStatus.Font = num; }
        if (st == "ReadingSpeed") { PublicStaticStatus.ReadingSpeed = num; }
        if (st == "Volume"){PublicStaticStatus.Volume=num;}
        if (st == "BGMVolume"){PublicStaticStatus.BGMVolume=num;}
        if (st == "SEVolume"){PublicStaticStatus.SEVolume=num;}

        UserDataToSave myUserDataToSave = new UserDataToSave()
        {
            ScreenMode = PublicStaticStatus.ScreenMode,
            Font = PublicStaticStatus.Font,
            ReadingSpeed = PublicStaticStatus.ReadingSpeed,
            Volume = PublicStaticStatus.Volume,
            BGMVolume = PublicStaticStatus.BGMVolume,
            SEVolume = PublicStaticStatus.SEVolume
        };

        string json = JsonUtility.ToJson(myUserDataToSave, true);
        string key = "UserData";
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
        Debug.Log("SaveExcuted.Key:" + "UserData" + "\n" + "JSON:" + json);
    }
}
