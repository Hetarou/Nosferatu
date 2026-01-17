using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonToLoadScene_Sample : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private string SceneToLoad;
    [SerializeField]
    AudioClip audioClip0;
    [SerializeField]
    AudioClip audioClip1;
    public void OnClick()
    {
        SimpleAudioManager_SE.instance.PlaySE(audioClip0);
        Debug.Log(SceneToLoad+"Ç…ÉVÅ[ÉìëJà⁄");
        SceneManager.LoadScene(SceneToLoad);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SimpleAudioManager_SE.instance.PlaySE(audioClip1);
    }
}
