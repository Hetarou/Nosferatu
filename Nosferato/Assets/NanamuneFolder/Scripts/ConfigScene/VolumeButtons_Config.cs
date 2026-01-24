using UnityEngine;
using UnityEngine.UI;

public class VolumeButtons_Config : MonoBehaviour
{
    [SerializeField]
    private string KindOfVolume;
    [SerializeField]
    private SaveLoadSystem_Config mySaveLoadSystem_Config;
    public void GetClick(int num)
    {
        UpdateValue(num);
        mySaveLoadSystem_Config.SaveStatus(KindOfVolume,num);
        UpdateView(num);
    }
    public void UpdateView(int num)
    {
        foreach (Transform t in transform)
        {
            t.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.1f);
        }
        for (int i = 0; i < num+1; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
        }
    }
    private void UpdateValue(int num)
    {
        if (KindOfVolume == "Volume") { PublicStaticStatus.Volume = num; }
        if (KindOfVolume == "BGMVolume") { PublicStaticStatus.BGMVolume = num; }
        if (KindOfVolume == "SEVolume") { PublicStaticStatus.SEVolume = num; }
        RefrectBGMVolume();
        RefrectSEVolume();
    }
    private void RefrectBGMVolume()
    {
        SimpleAudioManager_BGM.instance.SetVolume(PublicStaticStatus.Volume, PublicStaticStatus.BGMVolume);
    }

    private void RefrectSEVolume()
    {
        SimpleAudioManager_SE.instance.SetVolume(PublicStaticStatus.Volume, PublicStaticStatus.SEVolume);
    }
}
