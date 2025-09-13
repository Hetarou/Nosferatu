using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollHandleButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    string thisFunction;

    private bool isMouseOver = false;

    [SerializeField] private Sprite mousePushedsprite;
    [SerializeField] private Sprite mouseOutsprite;
    [SerializeField] private Image thisImage;

    void Update()
    {
        if (Input.GetMouseButton(0) && isMouseOver == true)
        {
            Debug.Log(thisFunction);
            thisImage.sprite = mousePushedsprite;
        }
        else
        {
            thisImage.sprite = mouseOutsprite;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        Debug.Log(thisFunction + "とマウスが重なった！");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        Debug.Log(thisFunction + "からマウスが離れた！");
    }
}
