using UnityEngine;
using UnityEngine.EventSystems;

public class VolumeButton_Config : MonoBehaviour
{
    [SerializeField]
    private int number;

    [SerializeField]
    AudioClip audioClip0;
    public void OnClick()
    {
        SimpleAudioManager_SE.instance.PlaySE(audioClip0);
        transform.parent.GetComponent<VolumeButtons_Config>().GetClick(number);
    }
}
