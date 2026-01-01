using UnityEngine;
using UnityEngine.SceneManagement;

public class ExacuteSave : ButtonScript
{
    public override void ExecuteCustomLogic()
    {
        Debug.Log("Save");
        SceneManager.LoadScene("SaveScene");
    }
}
