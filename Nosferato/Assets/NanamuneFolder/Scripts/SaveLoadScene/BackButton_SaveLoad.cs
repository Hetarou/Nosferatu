using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BackButton_SaveLoad : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    AudioClip audioClip0;
    [SerializeField]
    AudioClip audioClip1;
    public void OnClick()
    {
        SimpleAudioManager_SE.instance.PlaySE(audioClip0);
        Debug.Log(PublicStaticStatus.PreviousScene + "Ç…ÉVÅ[ÉìëJà⁄");
        SceneManager.LoadScene(PublicStaticStatus.PreviousScene);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SimpleAudioManager_SE.instance.PlaySE(audioClip1);
    }
}
