using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonToLoadScene_TitleStart : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    AudioClip audioClip0;
    [SerializeField]
    AudioClip audioClip1;
    public void OnClick()
    {
        SimpleAudioManager_SE.instance.PlaySE(audioClip0);
        Debug.Log("Start New Game.Go ScenarioScene");
        PublicStaticStatus.RowToSave = 2;
        SceneManager.LoadScene("ScenarioScene");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SimpleAudioManager_SE.instance.PlaySE(audioClip1);
    }
}
