using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject executedTextDirector;
    [SerializeField] private string executedText;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            DisplayExecutedText.Instance.StartDisplayCoroutine(executedText);
        }
    }
}
