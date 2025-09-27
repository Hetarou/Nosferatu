using UnityEngine;

public class RubyTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var tmpText = GetComponent<TMPro.TMP_Text>();
        tmpText.SetTextAndExpandRuby("‚ ‚ ‚ ‚ ‚ ‚ ‚ ‚ \n<r=‚à‚¶>•¶Žš</r>\n‚ ‚ ‚ ‚ ‚ ‚ ‚ ‚ ‚ ", fixedLineHeight: true, autoMarginTop: true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
