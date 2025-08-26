using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EXITButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Animator menuAnim;
    [SerializeField]
    string thisFunction;

    [SerializeField]
    GameObject Menu;

    private bool isMouseOver = false;
   
    //public GameObject exacutedObject;

    void Start()
    {
        menuAnim = Menu.GetComponent<Animator>();
        
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && isMouseOver == true)
        {
            Debug.Log(thisFunction);
            menuAnim.SetBool("isMenuAnim", true);
            
            //Invoke("isMenu", 0.6f);
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

    /*void isMenu()
    {
        if (Menu != null)
        {
            Menu.SetActive(false);
        }
    }*/
}



