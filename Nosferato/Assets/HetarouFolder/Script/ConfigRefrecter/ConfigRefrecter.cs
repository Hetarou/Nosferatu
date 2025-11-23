using UnityEngine;

public class ConfigRefrecter : MonoBehaviour
{
    [SerializeField] private SaveLoadSystem_Config saveLoadSystem_Config;

    void Start()
    {
        saveLoadSystem_Config.LoadStatus();
        GameModeSetter();
        FontSetter();
        ReadingSpeedSetter();
        VolumeSetter();
        BGMVolumeSetter();
        SEVolumeSetter();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GameModeSetter();
            FontSetter();
            ReadingSpeedSetter();
            VolumeSetter();
            BGMVolumeSetter();
            SEVolumeSetter();
        }
    }

    private void GameModeSetter()
    {
        switch (PublicStaticStatus.ScreenMode)
        {
            case 0:
                Debug.Log("0:ウィンドウ");
                break;
            case 1:
                Debug.Log("1:フルスクリーン");
                break;
            case 2:
                Debug.Log("2:疑似フルスクリーン");
                break;
        }
    }

    private void FontSetter()
    {
        switch (PublicStaticStatus.Font)
        {
            case 0:
                Debug.Log("0:ラノベポップ");
                break;
            case 1:
                Debug.Log("1:角ゴシック");
                break;
            case 2:
                Debug.Log("2:明朝体");
                break;
        }
    }

    private void ReadingSpeedSetter()
    {
        switch (PublicStaticStatus.ReadingSpeed)
        {
            case 0:
                Debug.Log("0:普通");
                break;
            case 1:
                Debug.Log("1:速い");
                break;
            case 2:
                Debug.Log("2:瞬時");
                break;
        }
    }

    private void VolumeSetter()
    {
        Debug.Log($"Volume:{PublicStaticStatus.Volume}");
    }

    private void BGMVolumeSetter()
    {
        Debug.Log($"BGMVolume:{PublicStaticStatus.BGMVolume}");
    }

    private void SEVolumeSetter()
    {
        Debug.Log($"SEVolume:{PublicStaticStatus.SEVolume}");
    }
}
