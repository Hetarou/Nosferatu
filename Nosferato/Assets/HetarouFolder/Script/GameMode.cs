using UnityEngine;

public class GameMode : MonoBehaviour
{
    public bool CanRead { get; private set; }
    public bool CanSkip     { get; private set; }
    public bool CanAuto     { get; private set; }
    public bool CanBackLog { get; private set; }
    public bool CanHideUI { get; private set; }

    public string ModeName { get; private set; }
    public string LastModeName { get; private set; }
    // Update is called once per frame
    void Start()
    {
        ModeManager("Reading");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(ModeName);
        }

    }

    public void ModeManager(string gameMode)
    {
        if (ModeName != null)
        {
            LastModeName = ModeName;
        }
            
        ModeName = gameMode;

        switch (ModeName)
        {
            case "Reading":
                CanRead = true;
                CanSkip = true;
                CanAuto = true;
                CanBackLog = true;
                CanHideUI = true;
                break;

            case "Skip":
                CanRead = true;
                CanSkip = true;
                CanAuto = false;
                CanBackLog = true;
                CanHideUI = true;
                break;

            case "Auto":
                CanRead = false;
                CanSkip = false;
                CanAuto = true;
                CanBackLog = true;
                CanHideUI = true;
                break;

            case "BackLog":
                CanRead = false;
                CanSkip = false;
                CanAuto = false;
                CanBackLog = false;
                CanHideUI = false;
                break;

            case "HideUI":
                CanRead = false;
                CanSkip = false;
                CanAuto = false;
                CanBackLog = false;
                CanHideUI = false;
                break;
        }
    }
}
