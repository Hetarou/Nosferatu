using UnityEngine;
using UnityEngine.SceneManagement;

public class ExacuteConfig : ButtonScript
{
    public override void ExecuteCustomLogic()
    {
        PublicStaticStatus.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("ConfigScene");
    }
}
