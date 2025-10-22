using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript_BackLog : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    string thisFunction;

    private bool isMouseOver = false;

    public GameObject exacutedObject;



    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isMouseOver == true)
        {
            Debug.Log(thisFunction);
            //BackLogをさかのぼって取得//スキップが止まる場所を区切りと呼ぶなら、前々回の区切りまで（区切り直後でも、一区切り分表示）
            exacutedObject.SetActive(true);
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
