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
        transform.parent.GetComponent<VolumeButtons_Config>().GetClick(number);
        SimpleAudioManager_SE.instance.PlaySE(audioClip0);
    }
}
