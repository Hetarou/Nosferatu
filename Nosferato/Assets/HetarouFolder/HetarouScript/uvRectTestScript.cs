using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class uvRectTestScript : MonoBehaviour
{
    public RawImage Line;
    void Start()
    {
        Line.uvRect = new Rect(1, 1, 1, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
