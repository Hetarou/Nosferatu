using UnityEngine;
using UnityEngine.SceneManagement;

public class ExacuteConfig : ButtonScript
{
    public override void ExecuteCustomLogic()
    {
        SceneManager.LoadScene("Config");
    }
}
