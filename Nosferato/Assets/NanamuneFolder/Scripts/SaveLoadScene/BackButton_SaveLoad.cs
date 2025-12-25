using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton_SaveLoad : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log(PublicStaticStatus.PreviousScene + "‚ÉƒV[ƒ“‘JˆÚ");
        SceneManager.LoadScene(PublicStaticStatus.PreviousScene);
    }
}
