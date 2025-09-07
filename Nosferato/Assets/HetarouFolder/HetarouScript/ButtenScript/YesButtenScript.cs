using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class YesButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    string thisFunction;
    [SerializeField]
    string SceneToLoad;
    private bool isMouseOver;


    void Start()
    {
    }
    void Update()
    {
        if (isMouseOver && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneToLoad);
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
}
