using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonToLoadScene_Sample : MonoBehaviour
{
    [SerializeField]
    private string SceneToLoad;
    public void OnClick()
    {
        Debug.Log(SceneToLoad+"‚ÉƒV[ƒ“‘JˆÚ");
        SceneManager.LoadScene(SceneToLoad);
    }
}
