using System.Collections.Generic;
using UnityEngine;

public class ExacuteTitle : ButtonScript
{
    [SerializeField] GameObject backTitle;   
    [SerializeField] List<GameObject> objList = new List<GameObject>();
    
    public override void ExecuteCustomLogic()
    {
        PublicStaticStatus.IsEnter = false;

        BackTitleDirector.Instance.InitializeMenu();

        backTitle.SetActive(true);

            for (int i = 0; i < objList.Count; i++)
            {
                objList[i].SetActive(false);
            }
    }
}
