using UnityEngine;

public class BGMPlayer_Title : MonoBehaviour
{
    [SerializeField]
    AudioClip audioClip;
    void Start()
    {
        SimpleAudioManager_BGM.instance.PlayBGM(audioClip);
    }
}
