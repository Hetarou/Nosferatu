using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YesButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    string thisFunction;

    [SerializeField]
    string SceneToLoad;

    private bool isMouseOver;

    [SerializeField]
    private Sprite mouseOversprite;
    [SerializeField]
    private Sprite mouseOutsprite;

    [SerializeField]
    private Image thisImage;

    


    void Start()
    {
    }
    void Update()
    {
        if (isMouseOver && Input.GetMouseButtonDown(0))
        {
            isMouseOver = false;
            thisImage.sprite = mouseOutsprite;
            SceneManager.LoadScene(SceneToLoad);
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
