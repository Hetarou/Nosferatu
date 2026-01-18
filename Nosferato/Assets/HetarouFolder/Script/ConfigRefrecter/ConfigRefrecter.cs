using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfigRefrecter : MonoBehaviour
{
    [SerializeField] private SaveLoadSystem_Config saveLoadSystem_Config;

    //SetGameModeで使う変数
    private FullScreenMode screenMode;

    //SetFontで使う変数
    [SerializeField] private TextMeshProUGUI targetTMP_MessageText;
    [SerializeField] private TextMeshProUGUI targetTMP_NameText;
    [SerializeField] private TextMeshProUGUI targetTMP_ExecuteText;
    [SerializeField] private List<TMP_FontAsset> newTMPFontAssets = new List<TMP_FontAsset>();

    //SetReadingSpeedで使う変数
    [SerializeField] private TextDisplayerRuby textDisplayerRuby;

    //SetVolumeで使う変数
    private float mainVolume;

    //SetBGMVolumeで使う変数
    //[SerializeField] private AudioSource audioSourceBGM;

    //SetSEVolumeで使う変数
    //[SerializeField] private AudioSource audioSourceSE;

    void Start()
    {
        saveLoadSystem_Config.LoadStatus();
        SetGameMode();
        SetFont();
        SetReadingSpeed();
        SetVolume();
        SetBGMVolume();
        SetSEVolume();
    }


    private void SetGameMode()
    {
        switch (PublicStaticStatus.ScreenMode)
        {
            case 0:
                screenMode = FullScreenMode.Windowed;
                break;
            case 1:
                screenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 2:
                screenMode = FullScreenMode.FullScreenWindow;
                break;
        }
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, screenMode);

        //UnityEditer状態では反映されないのでこのDebug.Logは残しておきます
        Debug.Log("ディスプレイモードを切り替えました: " + screenMode);
    }

    private void SetFont()
    {
        switch (PublicStaticStatus.Font)
        {
            case 0:
                targetTMP_MessageText.font = newTMPFontAssets[0];
                targetTMP_NameText.font = newTMPFontAssets[0];
                break;
            case 1:
                targetTMP_MessageText.font = newTMPFontAssets[1];
                targetTMP_NameText.font = newTMPFontAssets[1];
                break;
            case 2:
                targetTMP_MessageText.font = newTMPFontAssets[2];
                targetTMP_NameText.font = newTMPFontAssets[2];
                break;
        }
    }

    private void SetReadingSpeed()
    {
        switch (PublicStaticStatus.ReadingSpeed)
        {
            case 0:
                textDisplayerRuby.charDelay = 0.1f;
                break;
            case 1:
                textDisplayerRuby.charDelay = 0.05f;
                break;
            case 2:
                textDisplayerRuby.charDelay = 0.025f;
                break;
        }
    }

    private void SetVolume()
    {
        mainVolume = PublicStaticStatus.Volume;
        mainVolume /= 10;
    }

    private void SetBGMVolume()
    {
        SimpleAudioManager_BGM.instance.SetVolume(mainVolume, PublicStaticStatus.BGMVolume / 10);
    }

    private void SetSEVolume()
    {
        SimpleAudioManager_SE.instance.SetVolume(mainVolume, PublicStaticStatus.SEVolume  / 10);
    }
}
