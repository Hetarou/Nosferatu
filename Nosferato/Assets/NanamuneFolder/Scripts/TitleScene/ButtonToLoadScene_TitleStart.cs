using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonToLoadScene_TitleStart : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log("Start New Game.Go ScenarioScene");
        PublicStaticStatus.RowToSave = 2;
        SceneManager.LoadScene("ScenarioScene");
    }
}
