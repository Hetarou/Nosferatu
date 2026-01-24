using UnityEngine;

public class SimpleAudioManager_SE : MonoBehaviour
{
    public static SimpleAudioManager_SE instance;
    private AudioSource audioSource;

    void Awake()
    {
        // シングルトンの仕組み：1つだけを維持する
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 外から音を鳴らすための窓口
    public void PlaySE(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void SetVolume(float MainVolume, float SEVolume)
    {
        float Volume = MainVolume * SEVolume /120;

        audioSource.volume = Volume;
    }
}