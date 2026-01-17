using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using static Unity.VisualScripting.Member;

public class MovieFinishChecker_OP : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject videoMonitor;


    [SerializeField] private Image fadeImage;
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    
    [SerializeField] private Image skipGaugeImage; // 作成したゲージ用Imageをアサイン
    [SerializeField] private CanvasGroup skipGaugeGroup; // ゲージ全体の透明度管理用

    [SerializeField] private GameObject forteSoftLogo;

    [SerializeField] private float displayTime = 1.0f;

    private float _pressTimer = 0f; // 長押し時間を測るタイマー
    private const float SkipHoldTime = 1.0f; // 必要な長押し時間（1秒）
    private bool _isSkipping = false; // スキップが開始されたかどうかのフラグ

    void Start()
    {
        //初期化
        if (skipGaugeImage != null) skipGaugeImage.fillAmount = 0f;
        if (skipGaugeGroup != null) skipGaugeGroup.alpha = 0f;
        if (forteSoftLogo != null) forteSoftLogo.SetActive(false);
        if (videoMonitor != null) videoMonitor.SetActive(false);
        StartCoroutine(PlayMovieCoroutine());
    }

    private IEnumerator PlayMovieCoroutine()
    {
        
        videoPlayer.Prepare();
        Debug.Log("到達度:" + "1");
        while (!videoPlayer.isPrepared)
        {
            yield return null; // 1フレーム待ってから再度チェック
        }

        Debug.Log("到達度:" + "2");
        forteSoftLogo.SetActive(true);
        if (fadeImage != null) fadeImage.color = Color.black;
        yield return fadeCanvasGroup.DOFade(0f, 1.0f).WaitForCompletion();

        yield return new WaitForSeconds(displayTime);

        yield return fadeCanvasGroup.DOFade(1f, 1.0f).WaitForCompletion();
        yield return new WaitForSeconds(1f);

        fadeCanvasGroup.alpha = 0f;
        forteSoftLogo.SetActive(false);
        videoMonitor.SetActive(true);
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

        if (fadeImage != null) fadeImage.color = Color.black;
        yield return fadeCanvasGroup.DOFade(1f, 1.0f).WaitForCompletion();

        SceneManager.LoadScene("TitleScene");
    }
}
