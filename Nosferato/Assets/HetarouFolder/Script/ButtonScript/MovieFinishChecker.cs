using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MovieFinishChecker : MonoBehaviour
{
    [SerializeField]
    VideoPlayer videoPlayer;
    void Start()
    {
        videoPlayer.loopPointReached += LoopPointReached;
        videoPlayer.Play();
    }
    public void LoopPointReached(VideoPlayer vp)
    {
        SceneManager.LoadScene("ScenarioScene");
    }
}
