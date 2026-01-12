using UnityEngine;
using UnityEngine.SceneManagement;

public class ExacuteSave : ButtonScript
{
    public override void ExecuteCustomLogic()
    {
        PublicStaticStatus.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("SaveScene");
    }
}
