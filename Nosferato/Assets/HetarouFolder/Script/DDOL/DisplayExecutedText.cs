using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DisplayExecutedText : MonoBehaviour
{
    public static DisplayExecutedText Instance { get; private set; }

    private TextMeshProUGUI executedTextDisplayer;
    private CanvasGroup executedCanvasGroup;
    private Tween fade_text;

    void Start()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        executedTextDisplayer = GetComponentInChildren<TextMeshProUGUI>();
        executedCanvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    public void StartDisplayCoroutine(string Text)
    {
        if (fade_text != null)
        {
            fade_text.Complete();
            fade_text = null;
        }

        StartCoroutine(DisplayText(Text));
    }

    private IEnumerator DisplayText(string Text)
    {
        

        executedCanvasGroup.alpha = 1f;
        if (executedTextDisplayer != null)
        {
            executedTextDisplayer.text = Text;
        }
        else
        {
            Debug.Log("Ç†ÇËÇ‹ÇπÇÒÅI");
        }

        yield return new WaitForSeconds(0.6f);

        fade_text = executedCanvasGroup.DOFade(0f, 2.0f);
        yield return fade_text.WaitForCompletion();

        fade_text?.Complete();
    }
}
