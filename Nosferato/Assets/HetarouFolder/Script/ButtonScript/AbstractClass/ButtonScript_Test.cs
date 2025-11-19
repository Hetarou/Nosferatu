using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems; // EventSystemsをusing

// IPointerClickHandler を追加
public abstract class ButtonScript_Test : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private string thisFunction;
    [SerializeField] private float scaleRate = 1.1f;

    // マウスカーソルとオブジェクトが重なっているかを調べる
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(thisFunction + "とマウスが重なった！");
        transform.localScale *= scaleRate;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log(thisFunction + "からマウスが離れた！");
        transform.localScale = new(1.0f, 1.0f, 1.0f);
    }

    // オブジェクトがクリックされた時に呼ばれる
    public void OnPointerClick(PointerEventData eventData)
    {
        // eventData.button を使えば、左クリックか右クリックかも判定できます
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log(thisFunction + " がクリックされました！");
            ExcuteButton(); // 抽象メソッドを呼ぶ
        }
    }

    public abstract void ExcuteButton();
}
