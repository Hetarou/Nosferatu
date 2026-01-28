using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMProRuby_Test : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private string message_test;
    [SerializeField] private float speed = 0.05f;
    [SerializeField] private RichTagDiscriminator jumpTableMaker;

    Dictionary<int, int> jumpTable = new Dictionary<int, int>();

    private bool isTyping;
    private bool reqestSkkiped;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isTyping)
            {
                StartTypewriter(message_test);
                isTyping = true;
            }
            else
            {
                reqestSkkiped = true;
            }
        }
    }
    public void StartTypewriter(string message)
    {
        jumpTable = jumpTableMaker.MakeJumpTable(message_test);
        textMeshPro.SetTextAndExpandRuby(message, fixedLineHeight: true, autoMarginTop: false);
        textMeshPro.maxVisibleCharacters = 0;
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textMeshPro.ForceMeshUpdate();
        int totalCharacters = textMeshPro.textInfo.characterCount;

        for (int i = 0; i <= totalCharacters; i++)
        {
            if (jumpTable.TryGetValue(i, out int endPos))
            {
                i = endPos;
            }
            else if (reqestSkkiped)
            {
                i = totalCharacters;
            }
            textMeshPro.maxVisibleCharacters = i;
            yield return new WaitForSeconds(speed);
        }

        isTyping = false;
        reqestSkkiped = false;
    }
}