using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DisplayExecutedText : MonoBehaviour
{
    public static DisplayExecutedText Instance { get; private set; }

    private TextMeshProUGUI executedTextDisplayer;
    private CanvasGroup executedCanvasGroup;
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
        StartCoroutine(DisplayText(Text));
    }

    public IEnumerator DisplayText(string Text)
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

        yield return new WaitForSeconds(1f);

        yield return executedCanvasGroup.DOFade(0f, 2.0f).WaitForCompletion();
    }
}
