using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private string thisFunction;

    private bool isMouseOver = false;

    public GameObject exacutedObject;



    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isMouseOver == true)
        {
            Debug.Log(thisFunction);
            exacutedObject.SetActive(false);
            isMouseOver = false;
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
