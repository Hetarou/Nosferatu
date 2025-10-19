using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CGCloseUpper : MonoBehaviour
{
    [SerializeField]
    private Image PannelToCloseUp;
    [SerializeField]
    private Sprite myPannel;
    private bool isMyPannel=false;
    private void Start()
    {
    }
    public void OnClick()
    {
        isMyPannel = true;
        PannelToCloseUp.gameObject.SetActive(true);
        PannelToCloseUp.sprite = myPannel;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&&isMyPannel)
        {
            isMyPannel = false;
            PannelToCloseUp.gameObject.SetActive(false);
        }
    }
}