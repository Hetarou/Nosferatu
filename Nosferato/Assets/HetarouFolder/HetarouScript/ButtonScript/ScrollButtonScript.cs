using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    string thisFunction;

    private bool isMouseOver = false;

    [SerializeField] private Sprite mousePushedsprite;
    [SerializeField] private Sprite mouseOutsprite;
    [SerializeField] private Image thisImage;

    [SerializeField] private float scrollSpeed;
   
    [SerializeField] private ScrollRect scrollRectBuckLog;

    void Update()
    {
        if (Input.GetMouseButton(0) && isMouseOver == true)
        {
            thisImage.sprite = mousePushedsprite;

            // 現在の位置を取得
            Vector2 pos = scrollRectBuckLog.normalizedPosition;

            // Yを少しずつ減らす（0 = 下端, 1 = 上端）
            pos.y -= scrollSpeed * Time.deltaTime;

            // 範囲を制限
            pos.y = Mathf.Clamp01(pos.y);

            // 適用
            scrollRectBuckLog.normalizedPosition = pos;

            Debug.Log(thisFunction);
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