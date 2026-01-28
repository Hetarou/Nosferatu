using UnityEngine;

public class BGMPlayer_Title : MonoBehaviour
{
    [SerializeField]
    AudioClip audioClip;
    void Start()
    {
        if(!PublicStaticStatus.IsClearedNow)
        {
            SimpleAudioManager_BGM.instance.PlayBGM(audioClip);
        }
        else if(PublicStaticStatus.IsClearedNow)
        {
            PublicStaticStatus.IsClearedNow =false;
            return;
        }
    }
}
