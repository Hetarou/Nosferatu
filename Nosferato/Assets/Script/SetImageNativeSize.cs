using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SetImageNativeSize : MonoBehaviour
{
    void Start()
    {
        var img = GetComponent<Image>();
        img.SetNativeSize(); // スプライトのピクセルサイズをそのまま使う
    }
}
