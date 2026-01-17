using UnityEngine;
using UnityEngine.SceneManagement;

public class ExacuteConfig : ButtonScript
{
    public override void ExecuteCustomLogic()
    {
        PublicStaticStatus.PreviousScene = SceneManager.GetActiveScene().name;
        Debug.Log(PublicStaticStatus.PreviousScene);
        SceneManager.LoadScene("ConfigScene");
    }
}
