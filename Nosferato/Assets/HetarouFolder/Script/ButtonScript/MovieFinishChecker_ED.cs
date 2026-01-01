using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using static Unity.VisualScripting.Member;

public class MovieFinishChecker_ED : MonoBehaviour
{
    [SerializeField]
    VideoPlayer videoPlayer;

    [SerializeField] private Image fadeImage;
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    [SerializeField] private Image skipGaugeImage; // 作成したゲージ用Imageをアサイン
    [SerializeField] private CanvasGroup skipGaugeGroup; // ゲージ全体の透明度管理用

    private float _pressTimer = 0f; // 長押し時間を測るタイマー
    private const float SkipHoldTime = 1.0f; // 必要な長押し時間（1秒）
    private bool _isSkipping = false; // スキップが開始されたかどうかのフラグ

    void Start()
    {
        
        if (skipGaugeImage != null) skipGaugeImage.fillAmount = 0f;
        if (skipGaugeGroup != null) skipGaugeGroup.alpha = 0f;
        StartCoroutine(PlayMovieCoroutine());
    }

    private IEnumerator PlayMovieCoroutine()
    {
        //黒から白にFadeIn
        if (fadeImage != null)fadeImage.color = Color.black;
        yield return fadeImage.DOColor(Color.white, 1.0f).WaitForCompletion(); //

        fadeCanvasGroup.alpha = 0f;

        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null; // 1フレーム待ってから再度チェック
        }

        videoPlayer.Play();

        while ((videoPlayer.isPlaying || videoPlayer.time < videoPlayer.length - 0.1) && !_isSkipping)
        {
            if (Input.GetMouseButton(0))
            {
                _pressTimer += Time.deltaTime;

                if (skipGaugeImage != null)
                {
                    skipGaugeGroup.alpha = 1f;
                    skipGaugeImage.fillAmount = _pressTimer / SkipHoldTime;
                }

                if (_pressTimer >= SkipHoldTime)
                {
                    _isSkipping = true;
                }
            }
            else
            {
                _pressTimer = 0;
                if (skipGaugeImage != null) skipGaugeImage.fillAmount = 0f;
                if (skipGaugeGroup != null) skipGaugeGroup.alpha = 0f; // 離したら消す
            }

            yield return null; // 1フレーム待ってから再度チェック
        }

        if (skipGaugeGroup != null) skipGaugeGroup.alpha = 0f;

        videoPlayer.Stop();

        //黒にFadeOut
        if (fadeImage != null) fadeImage.color = Color.black;
        yield return fadeCanvasGroup.DOFade(1f, 1.0f).WaitForCompletion();

        SceneManager.LoadScene("ScenarioScene");
    }
}
