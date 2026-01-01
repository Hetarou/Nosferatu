using UnityEngine;
using UnityEngine.SceneManagement;

public class ExacuteLoad : ButtonScript
{
    public override void ExecuteCustomLogic()
    {
        Debug.Log("Load");
        SceneManager.LoadScene("LoadScene");
    }
}