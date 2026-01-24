using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackTitleDirector : MonoBehaviour
{
    [SerializeField]
    GameObject backTitle;

    [SerializeField] private GameObject Menu;

    [SerializeField] private TextDisplayerRuby textDisplayerRuby;

    private CanvasGroup canvasGroup;

    public static BackTitleDirector Instance;

    [SerializeField] GameMode gameMode;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        canvasGroup = Menu.GetComponent<CanvasGroup>();
    }
    // Update is called once per frame
    void Update()
    {
        int i;
        if (Input.GetMouseButtonDown(1))
        {
            BackTitle();
        }
    }

    public void BackTitle()
    {
        textDisplayerRuby.RequestHide();
        //タイトルに戻りますかのオブジェクトのみを表示し、あとは非表示にする
        backTitle.SetActive(true);
        InitializeMenu();
        gameMode.ModeManager("BackLog");
    }

    public void InitializeMenu()
    {
        canvasGroup.alpha = 0.0f;
    }

    public void ActivateMenu()
    {
        canvasGroup.alpha = 1.0f;
    }
}
