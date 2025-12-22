using UnityEngine;
using UnityEngine.SceneManagement;

public class ExacuteLoad : ButtonScript
{
    public override void ExcuteButton()
    {
        Debug.Log("Load");
        SceneManager.LoadScene("LoadScene");
    }
}