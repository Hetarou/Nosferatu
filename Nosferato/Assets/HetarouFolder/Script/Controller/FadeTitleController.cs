using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class FadeTitleController : MonoBehaviour
{
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject title_First;
    [SerializeField] private GameObject titleImage_SYU;
    [SerializeField] private Sprite startTitleSprite;
    [SerializeField] private Sprite endTitleSprite;

    private Image titleImage;
    private CanvasGroup titleCanvasGroup_first;
    private CanvasGroup titleCanvasGroup;

    private void Start()
    {
        titleImage = title.GetComponent<Image>();
        titleCanvasGroup_first = title_First.GetComponent<CanvasGroup>();
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
        yield return titleCanvasGroup.DOFade(1f, 1f).WaitForCompletion();

        yield return new WaitForSeconds(1f);

        yield return titleCanvasGroup.DOFade(0f, 1f).WaitForCompletion();
    }

    private IEnumerator EndTitleCoroutine()
    {
        titleImage.sprite = endTitleSprite;

        AudioClip clipBGM = Resources.Load<AudioClip>("BGM/âƒÇÃévÇ¢èo");
        SimpleAudioManager_BGM.instance.PlayBGM(clipBGM);

        yield return titleCanvasGroup.DOFade(1f, 0.7f).WaitForCompletion();

        yield return new WaitForSeconds(1f);

        titleImage_SYU.SetActive(true);

        yield return new WaitForSeconds(3f);


    }

}
