using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PannelExcuter : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private TMP_Text text;
    void Start()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("マウスが入った！");
        text.text = "aaafasdf";
    }
}
