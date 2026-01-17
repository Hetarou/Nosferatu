using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButtonScript_Test : ButtonScript
{
    [SerializeField]
    GameObject backTitle;
    [SerializeField]
    List<GameObject> objList = new List<GameObject>();
    public override void ExecuteCustomLogic()
    {
        backTitle.SetActive(true);

        int i;
        for (i = 0; i < objList.Count; i++)
        {
            objList[i].SetActive(false);
        }
    }
}
