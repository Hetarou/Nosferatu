using UnityEngine;
using UnityEngine.Video;

public class ConfigRefrecter_Video : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        float MainVolume = PublicStaticStatus.Volume;
        float BGMVolume = PublicStaticStatus.BGMVolume;

        videoPlayer.SetDirectAudioVolume(0, MainVolume * BGMVolume * 1.2f / 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
