using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ButtonScript_Test : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    string thisFunction;


    private bool isMouseOver = false;

    //public GameObject exacutedObject;

    void Start()
    {
    }
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) && isMouseOver == true))
        {
            Debug.Log(thisFunction);
            ExcuteButton();
        }
    }

    //マウスカーソルとオブジェクトが重なっているかを調べる
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

    public abstract void ExcuteButton();//このやり方以外に、イベントを発行するやり方、SendMessageを使うやり方、もある
    /*{
        backTitle.SetActive(true);

        int i;
        for (i = 0; i < objList.Count; i++)
        {
            objList[i].SetActive(false);
        }
    }*/
}
