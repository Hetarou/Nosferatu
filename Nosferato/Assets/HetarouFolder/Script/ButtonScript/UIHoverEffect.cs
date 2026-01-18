using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // インターフェースを使うために必要

// 「入る」と「出る」の両方のルール（インターフェース）を守ると宣言する
public class UIHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image myImage;

    [SerializeField] private Sprite mouseOversprite;
    [SerializeField] private Sprite mouseOutsprite;
    private void Start()
    {
        myImage = GetComponent<Image>();
    }
    // マウスが入った時の処理
    public void OnPointerEnter(PointerEventData eventData)
    {
        myImage.sprite = mouseOversprite;
    }

    // マウスが出た時の処理
    public void OnPointerExit(PointerEventData eventData)
    {
        myImage.sprite = mouseOutsprite;
    }
}