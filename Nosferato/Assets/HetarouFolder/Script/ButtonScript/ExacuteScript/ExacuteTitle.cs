using System.Collections.Generic;
using UnityEngine;

public class ExacuteTitle : ButtonScript
{
    [SerializeField] GameObject backTitle;   
    [SerializeField] List<GameObject> objList = new List<GameObject>();
    
    public override void ExecuteCustomLogic()
    {
        PublicStaticStatus.IsEnter = false;

        BackTitleDirector.Instance.BackTitle();
    }
}
