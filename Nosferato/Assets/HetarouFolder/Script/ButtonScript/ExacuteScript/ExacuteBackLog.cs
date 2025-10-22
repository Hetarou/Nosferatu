using System.Runtime.InteropServices;
using UnityEngine;

public class ExacuteBackLog : ButtonScript_Test
{
    [SerializeField] private GameObject backLog;
    [SerializeField] private GameMode GameMode;
    void Start()
    {

    }


    void Update()
    {

    }

    public override void ExcuteButton()
    {
        backLog.SetActive(true);
        GameMode.ModeManager("BackLog");
    }
}
