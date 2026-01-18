using UnityEngine;
using UnityEngine.EventSystems;

public class MiniGameButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private MiniGameLauncher miniGameLaunchar;

    [SerializeField]
    AudioClip audioClip0;
    [SerializeField]
    AudioClip audioClip1;
    public void OnClick()
    {
        SimpleAudioManager_SE.instance.PlaySE(audioClip0);
        miniGameLaunchar.LaunchMiniGame();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SimpleAudioManager_SE.instance.PlaySE(audioClip1);
    }
}
