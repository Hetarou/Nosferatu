using UnityEngine;
using UnityEngine.Video;

public class ConfigRefrecter_Video : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        float volume = PublicStaticStatus.Volume;
        videoPlayer.SetDirectAudioVolume(0, volume/10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
