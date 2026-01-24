using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoButtonScript : ButtonScript
{
    [SerializeField]
    GameObject backTitle;
    [SerializeField]
    List<GameObject> objList = new List<GameObject>();

    [SerializeField]
    private Sprite mouseOutsprite;

    [SerializeField]
    private Image thisImage;

    [SerializeField] GameMode gameMode;

    public override void ExecuteCustomLogic()
    {
        thisImage.sprite = mouseOutsprite;
        PublicStaticStatus.IsEnter = false;
        //BackTitleDirector.Instance.InitializeMenu();
        backTitle.SetActive(false);
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].SetActive(true);
        }
        BackTitleDirector.Instance.ActivateMenu();
        gameMode.ModeManager(gameMode.lastModeName);
    }
}
