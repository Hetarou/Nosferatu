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
            PublicStaticStatus.Volume = myUserDataToSave.Volume;
            PublicStaticStatus.BGMVolume = myUserDataToSave.BGMVolume;
            PublicStaticStatus.SEVolume = myUserDataToSave.SEVolume;
            Debug.Log("UserDataをロードしました。" + "Ref" + json);
        }
        else
        {
            Debug.Log("PlayerUserDataが存在しません");
        }
    }

    public void SaveStatus(string st,int num)
    {
        if (st == "Volume"){PublicStaticStatus.Volume=num;}
        if (st == "BGMVolume"){PublicStaticStatus.BGMVolume=num;}
        if (st == "SEVolume"){PublicStaticStatus.SEVolume=num;}

        UserDataToSave myUserDataToSave = new UserDataToSave()
        {
            Volume = PublicStaticStatus.Volume,
            BGMVolume = PublicStaticStatus.BGMVolume,
            SEVolume = PublicStaticStatus.SEVolume
        };


        string json = JsonUtility.ToJson(myUserDataToSave, true);
        string key = "UserData";
        Debug.Log("UserDataにセーブしました" + "Ref" + json);

        //jsonデータにしたやつらをここに格納
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }
}
