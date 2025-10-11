using UnityEngine;

public class VolumeButton_Config : MonoBehaviour
{
    [SerializeField]
    private int number;
    public void OnClick()
    {
        //Debug.Log(number);
        transform.parent.GetComponent<VolumeButtons_Config>().ChangeVolume(number);
    }
}
