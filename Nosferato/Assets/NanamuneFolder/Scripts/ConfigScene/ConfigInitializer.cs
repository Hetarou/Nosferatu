using NUnit.Framework.Internal;
using UnityEngine;

public class ConfigInitializer : MonoBehaviour
{
    [SerializeField]
    SaveLoadSystem_Config mySaveLoadSystem;
    [SerializeField]
    VolumeButtons_Config myVolumeButtons_Config0;
    [SerializeField]
    VolumeButtons_Config myVolumeButtons_Config1;
    [SerializeField]
    VolumeButtons_Config myVolumeButtons_Config2;
    private void Awake()
    {
        mySaveLoadSystem.LoadStatus();
        ReflectStatus();
    }
    private void ReflectStatus()
    {
        //Volume”½‰f
        myVolumeButtons_Config0.ChangeVolume_fromInitializer(PublicStaticStatus.Volume);
        myVolumeButtons_Config1.ChangeVolume_fromInitializer(PublicStaticStatus.BGMVolume);
        myVolumeButtons_Config2.ChangeVolume_fromInitializer(PublicStaticStatus.SEVolume);
    }
}
