using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    string thisFunction;
    [SerializeField]
    GameObject backTitle;
    [SerializeField]
    List<GameObject> objList = new List<GameObject>();

    private bool isMouseOver = false;

    [SerializeField]
    private Sprite mouseOversprite;
    [SerializeField]
    private Sprite mouseOutsprite;

    [SerializeField]
    private Image thisImage;
    
    //public GameObject exacutedObject;

    void Start()
    {
    }
    void Update()
    {

        int i;
        if ((Input.GetMouseButtonDown(0) && isMouseOver == true))
        {
            Debug.Log(thisFunction);

            thisImage.sprite = mouseOutsprite;

            backTitle.SetActive(false);
            for (i = 0; i < objList.Count; i++)
            {
                objList[i].SetActive(true);
            }
        }
    }

    //マウスカーソルとオブジェクトが重なっているかを調べる
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;

        thisImage.sprite = mouseOversprite;

        Debug.Log(thisFunction + "とマウスが重なった！");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;

        thisImage.sprite = mouseOutsprite;

        Debug.Log(thisFunction + "からマウスが離れた！");
    }
}
