using UnityEngine;

public class GameMode : MonoBehaviour
{
    public bool modeSkip     { get; private set; }
    public bool modeAuto     { get; private set; }
    public bool modeBackLog  { get; private set; }

    public string modeName { get; private set; }
    public string lastModeName { get; private set; }
    // Update is called once per frame
    void Start()
    {
        ModeManager("Reading");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(modeName);
        }

    }

    public void ModeManager(string gameMode)
    {
        if (modeName != null)
            lastModeName = modeName;
        modeName = gameMode;

        switch (modeName)
        {
            case "Reading":
                modeSkip = true;
                modeAuto = true;
                modeBackLog = true;
                break;

            case "Skip":
                modeSkip = true;
                modeAuto = false;
                modeBackLog = true;
                break;

            case "Auto":
                modeSkip = false;
                modeAuto = true;
                modeBackLog = true;
                break;

            case "BackLog":
                modeSkip = false;
                modeAuto = false;
                modeBackLog = true;
                break;
        }
    }
}
