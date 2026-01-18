using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject executedTextIcon;
    [SerializeField] private string executedText;

    private TextMeshProUGUI executedTextDisplayer;
    private CanvasGroup executedCanvasGroup;

    private static bool executed = false;

    private void Start()
    {
        executedTextDisplayer = executedTextIcon.GetComponentInChildren<TextMeshProUGUI>();
        executedCanvasGroup = executedTextIcon.GetComponent<CanvasGroup>();

        if (executed)
        {
            StartCoroutine(DisplayCoroutine());
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            StartCoroutine(DisplayCoroutine()); 
        }
    }

    private IEnumerator DisplayCoroutine()
    {
        executedTextIcon.SetActive(true);
        if (executedTextDisplayer != null)
        {
            executedTextDisplayer.text = executedText;
        }
        else
        {
            Debug.Log("Ç†ÇËÇ‹ÇπÇÒÅI");
        }

        yield return new WaitForSeconds(1f);

        yield return executedCanvasGroup.DOFade(0f, 2.0f).WaitForCompletion();
        executedTextIcon.SetActive(false);
        executedCanvasGroup.alpha = 1f;
        executed = false;
    }
}
