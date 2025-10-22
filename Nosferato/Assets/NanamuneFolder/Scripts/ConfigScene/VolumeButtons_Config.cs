using UnityEngine;
using UnityEngine.UI;

public class VolumeButtons_Config : MonoBehaviour
{
    [SerializeField]
    string ClassOfVolume;
    [SerializeField]
    SaveLoadSystem_Config mySaveLoadSystem_Config;
    public void ChangeVolume_fromButton(int num)
    {
        mySaveLoadSystem_Config.SaveStatus(ClassOfVolume,num);
        ChangeVolume(num);
    }

    public void ChangeVolume_fromInitializer(int num)
    {
        ChangeVolume(num);
    }
    private void ChangeVolume(int num)
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
}
