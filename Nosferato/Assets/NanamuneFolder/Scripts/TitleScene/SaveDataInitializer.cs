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

        //ScreenModeの反映
        ApplyScreenMode();
    }

    private void ApplyScreenMode()
    {
        if (PublicStaticStatus.ScreenMode == 0)
        {
            SetWindowed();
        }
        else if (PublicStaticStatus.ScreenMode == 1)
        {
            SetFullscreen();
        }
        else if (PublicStaticStatus.ScreenMode == 2)
        {
            SetBorderless();
        }
    }
    public void SetWindowed()
    {
        Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
    }

    public void SetFullscreen()
    {
        Screen.SetResolution(
            Screen.currentResolution.width,
            Screen.currentResolution.height,
            FullScreenMode.ExclusiveFullScreen
        );
    }

    public void SetBorderless()
    {
        Screen.SetResolution(
            Screen.currentResolution.width,
            Screen.currentResolution.height,
            FullScreenMode.FullScreenWindow
        );
    }
}
