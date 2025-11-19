using NUnit.Framework.Internal;
using UnityEngine;

public class ConfigInitializer : MonoBehaviour
{
    [SerializeField]
    SaveLoadSystem_Config mySaveLoadSystem;
    [SerializeField]
    Buttons_Config myButtons_Config0;
    [SerializeField]
    Buttons_Config myButtons_Config1;
    [SerializeField]
    Buttons_Config myButtons_Config2;
    [SerializeField]
    VolumeButtons_Config myVolumeButtons_Config0;
    [SerializeField]
    VolumeButtons_Config myVolumeButtons_Config1;
    [SerializeField]
    VolumeButtons_Config myVolumeButtons_Config2;
    private void Awake()
    {
        mySaveLoadSystem.LoadStatus();
    }
    private void Start()
    {
        ReflectStatus();
    }
    private void ReflectStatus()
    {
        myButtons_Config0.UpdateView(PublicStaticStatus.ScreenMode);
        myButtons_Config1.UpdateView(PublicStaticStatus.Font);
        myButtons_Config2.UpdateView(PublicStaticStatus.ReadingSpeed);
        //Volume”½‰f
        myVolumeButtons_Config0.UpdateView(PublicStaticStatus.Volume);
        myVolumeButtons_Config1.UpdateView(PublicStaticStatus.BGMVolume);
        myVolumeButtons_Config2.UpdateView(PublicStaticStatus.SEVolume);
    }
}
