using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackTitleDirector : MonoBehaviour
{
    [SerializeField]
    GameObject backTitle;
    [SerializeField]
    List<GameObject> objList = new List<GameObject>();

    [SerializeField] private GameObject Menu_Group1;
    [SerializeField] private GameObject Menu_Group2;
    [SerializeField] private GameObject Menu_backGround;

    RectTransform rectTransform;

    private Vector2 initialAnchoredPosition;
    private float menuBackGroundHeight;

    [SerializeField] private TextDisplayerRuby textDisplayerRuby;

    private CanvasGroup canvasGroup_Group1;

    public static BackTitleDirector Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        canvasGroup_Group1 = Menu_Group1.GetComponentInChildren<CanvasGroup>();

        rectTransform = Menu_backGround.GetComponent<RectTransform>();

        initialAnchoredPosition = rectTransform.anchoredPosition;
        menuBackGroundHeight = rectTransform.rect.height;
    }
    // Update is called once per frame
    void Update()
    {
        int i;
        if (Input.GetMouseButtonDown(1))
        {
            textDisplayerRuby.RequestHide();
            //タイトルに戻りますかのオブジェクトのみを表示し、あとは非表示にする
            backTitle.SetActive(true);
            for (i = 0; i < objList.Count; i++)
            {
                objList[i].SetActive(false);
            }
        }
    }

    public void InitializeMenu()
    {
        Menu_Group1.SetActive(true);
        Menu_Group2.SetActive(false);

        canvasGroup_Group1.alpha = 1.0f;

        rectTransform.anchoredPosition = initialAnchoredPosition;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, menuBackGroundHeight);
    }
}
