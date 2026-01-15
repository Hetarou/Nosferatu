using UnityEngine;

public class SimpleAudioManager_BGM : MonoBehaviour
{
    public static SimpleAudioManager_BGM instance;
    private AudioSource audioSource;

    void Awake()
    {
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

    public void PlayBGM(AudioClip clip)
    { 
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void SetVolume(float Volume)
    {
        audioSource.volume = Volume;
    }
}