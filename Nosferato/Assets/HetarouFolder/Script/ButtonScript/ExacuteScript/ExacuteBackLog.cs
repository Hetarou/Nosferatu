using System.Runtime.InteropServices;
using UnityEngine;

public class ExacuteBackLog : ButtonScript
{
    [SerializeField] private GameObject backLog;
    [SerializeField] private GameMode GameMode;

    private bool isBackLog;
    void Start()
    {

    }


    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            if(!GameMode.CanBackLog) return;
            ExecuteCustomLogic();
        }
    }

    public override void ExecuteCustomLogic()
    {
        PublicStaticStatus.IsEnter = false;
        backLog.SetActive(true);
        GameMode.ModeManager("BackLog");
    }
}
