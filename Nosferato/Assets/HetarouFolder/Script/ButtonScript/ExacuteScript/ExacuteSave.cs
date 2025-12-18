using UnityEngine;
using UnityEngine.SceneManagement;

public class ExacuteSave : ButtonScript_Test
{
    public override void ExcuteButton()
    {
        Debug.Log("Save");
        SceneManager.LoadScene("SaveScene");
    }
}
