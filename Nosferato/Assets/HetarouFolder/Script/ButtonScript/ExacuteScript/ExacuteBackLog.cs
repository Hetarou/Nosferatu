using System.Runtime.InteropServices;
using UnityEngine;

public class ExacuteBackLog : ButtonScript
{
    [SerializeField] private GameObject backLog;
    [SerializeField] private GameMode GameMode;
    void Start()
    {

    }


    void Update()
    {

    }

    public override void ExecuteCustomLogic()
    {
        PublicStaticStatus.IsEnter = false;
        backLog.SetActive(true);
        GameMode.ModeManager("BackLog");
    }
}
