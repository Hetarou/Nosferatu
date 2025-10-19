using UnityEngine;
using UnityEngine.UI;

public class VolumeButtons_Config : MonoBehaviour
{
    public void ChangeVolume(int num)
    {

        Debug.Log(num);
        foreach (Transform t in transform)
        {
            t.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.15f);
        }
        for (int i = 0; i < num+1; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
        }
    }
}
