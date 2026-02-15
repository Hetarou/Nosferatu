using System.Collections;
using UnityEngine;

public class SimpleAudioManager_BGM : MonoBehaviour
{
    public static SimpleAudioManager_BGM instance;
    private AudioSource audioSource;
    private float Volume;
    private float VolumeConst = 1;
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

    private void Start()
    {
        SetVolume(PublicStaticStatus.Volume, PublicStaticStatus.BGMVolume);
    }

    public void PlayBGM(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void SetVolume(float MainVolume, float BGMVolume)
    {
        Volume = MainVolume * BGMVolume / 80;

        audioSource.volume = Volume * VolumeConst;
    }

    public IEnumerator FadeOutCoroutine()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            // 時間の経過に合わせてボリュームを減らす
            audioSource.volume -= startVolume * Time.deltaTime / 0.5f;
            yield return null;
        }

        StopBGM();
        audioSource.volume = startVolume; // 必要に応じてリセット
    }

    public void ChangeVolumeConst(float constNum)
    {
        VolumeConst = constNum;
        audioSource.volume = Volume * VolumeConst;
    }
}