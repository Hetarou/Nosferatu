using System.Collections.Generic;
using UnityEngine;

public class Buttons_Config : MonoBehaviour
{
    [SerializeField]
    private string KindOfButton;
    [SerializeField]
    private SaveLoadSystem_Config mySaveLoadSystem_Config;
    [SerializeField]
    private List<Button_Config> components = new List<Button_Config>();
    public void GetClick(int num)
    {
        UpdateValue(num);
        mySaveLoadSystem_Config.SaveStatus("KindOfButton", num);
        UpdateView(num);
    }
    public void UpdateView(int num)
    {
        for(int i = 0;i < components.Count;i++)
        {
            if (i != num)
            {
                components[i].Deselect();
            }
            else if (i == num)
            {
                components[i].Select();
            }
        }
    }

    private void UpdateValue(int num)
    {
        if (KindOfButton == "ScreenMode") { PublicStaticStatus.ScreenMode = num; ApplyScreenMode(); }
        if (KindOfButton == "Font") { PublicStaticStatus.Font = num; }
        if (KindOfButton == "ReadingSpeed") { PublicStaticStatus.ReadingSpeed = num; }
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
        Screen.SetResolution(1280 , 720, FullScreenMode.Windowed);
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