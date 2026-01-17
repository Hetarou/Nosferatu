using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CGCloseUpper : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private Image PannelToCloseUp;
    [SerializeField]
    private Sprite myPannel;
    private bool isMyPannel=false;

    [SerializeField]
    AudioClip audioClip0;
    [SerializeField]
    AudioClip audioClip1;
    [SerializeField]
    AudioClip audioClip2;

    private void Start()
    {
    }
    public void OnClick()
    {
        SimpleAudioManager_SE.instance.PlaySE(audioClip0);
        isMyPannel = true;
        PannelToCloseUp.gameObject.SetActive(true);
        PannelToCloseUp.sprite = myPannel;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&&isMyPannel)
        {
            SimpleAudioManager_SE.instance.PlaySE(audioClip1);
            isMyPannel = false;
            PannelToCloseUp.gameObject.SetActive(false);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SimpleAudioManager_SE.instance.PlaySE(audioClip2);
    }
}