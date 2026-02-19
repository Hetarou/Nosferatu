using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class FadeTitleController : MonoBehaviour
{
    [SerializeField] private CanvasGroup titleCanvasGroup_Background;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject title_First;
    [SerializeField] private GameObject titleImage_SYU;
    [SerializeField] private Sprite startTitleSprite;
    [SerializeField] private Sprite endTitleSprite;


    private Image titleImage;
    private CanvasGroup titleCanvasGroup_first;
    private CanvasGroup titleCanvasGroup;
    private CanvasGroup titleCanvasGroup_SYU;




    private void Start()
    {
        titleImage = title.GetComponent<Image>();
        titleCanvasGroup_first = title_First.GetComponent<CanvasGroup>();
        titleCanvasGroup_SYU = titleImage_SYU.GetComponent<CanvasGroup>();
        titleCanvasGroup = title.GetComponent<CanvasGroup>();
    }

    public IEnumerator CoroutineBranch(string command)
    {
        if (command == "FIRST")
        {
            yield return StartCoroutine(FirstTitleCoroutine());
        }
        else if (command == "START")
        {
            yield return StartCoroutine(StartTitleCoroutine());
        }
        else if (command == "END")
        {
            yield return StartCoroutine(EndTitleCoroutine());
        }
        else 
        {
            Debug.LogWarning("command");
        }
    }

    private IEnumerator FirstTitleCoroutine()
    {
        yield return titleCanvasGroup_first.DOFade(1f, 1f).WaitForCompletion();

        yield return new WaitForSeconds(1f);

        yield return titleCanvasGroup_first.DOFade(0f, 1f).WaitForCompletion();
    }

    private IEnumerator StartTitleCoroutine()
    {
        titleImage.sprite = startTitleSprite;

        yield return titleCanvasGroup_Background.DOFade(1f, 0.5f).WaitForCompletion();

        yield return titleCanvasGroup.DOFade(1f, 0.5f).WaitForCompletion();

        yield return new WaitForSeconds(2f);

        
    }

    public IEnumerator StartTitleFadeInCoroutine()
    {
        yield return titleCanvasGroup.DOFade(0f, 0.5f).WaitForCompletion();

        yield return titleCanvasGroup_Background.DOFade(0f, 0.5f).WaitForCompletion();
    }

    private IEnumerator EndTitleCoroutine()
    {
        titleImage.sprite = endTitleSprite;

        AudioClip clipBGM = Resources.Load<AudioClip>("BGM/âƒÇÃévÇ¢èo");
        SimpleAudioManager_BGM.instance.PlayBGM(clipBGM);

        yield return titleCanvasGroup_Background.DOFade(1f, 1f).WaitForCompletion();

        yield return new WaitForSeconds(0.5f);

        yield return titleCanvasGroup.DOFade(1f, 2f).WaitForCompletion();

        yield return titleCanvasGroup_SYU.DOFade(1f, 1f).WaitForCompletion();

        yield return new WaitForSeconds(2f);

        yield return titleCanvasGroup.DOFade(0f, 1.5f).WaitForCompletion();
    }

}
