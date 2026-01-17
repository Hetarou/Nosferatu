using System.Collections;
using UnityEngine;
using DG.Tweening;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Animator fadeAnim;
    [SerializeField] private GameObject fadeAnimObj;

    [SerializeField] private CanvasGroup canvasGroup;

    public IEnumerator BlackFadeExecuter()
    {
        canvasGroup.DOFade(1, 0.5f);
        yield break;
    }
}
