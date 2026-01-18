using UnityEngine;
using UnityEngine.SceneManagement;

public class ExacuteLoad : ButtonScript
{
    public override void ExecuteCustomLogic()
    {
        PublicStaticStatus.IsEnter = false;
        PublicStaticStatus.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LoadScene");
    }
}