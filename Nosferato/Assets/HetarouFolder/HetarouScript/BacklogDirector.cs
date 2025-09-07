using Unity.VisualScripting;
using UnityEngine;

public class BacklogDirector : MonoBehaviour
{
    [SerializeField] private GameObject topRuledLine;
    [SerializeField] private GameObject middleRuledLine;
    [SerializeField] private GameObject bottomRuledLine;

    [SerializeField] private GameObject scrollContent;

    private float RuledLineHeight;
    private float scrollContentHeight;

    float halfRuledLineHeight;


    private float difference  = 576;
    float halfScrollContentHeight;

    Vector3 topRuledLineRectAncPos;
    Vector3 middleRuledLineRectAncPos;
    Vector3 bottomRuledLineRectAncPos;
    Vector3 scrollContentRectAncPos;

    private float pastScrollContentRectAncPos;
    void Start()
    {

        //GameObject‚Ì‚‚³‚ğæ“¾
        RuledLineHeight = middleRuledLine.GetComponent<RectTransform>().sizeDelta.y;
        scrollContentHeight = scrollContent.GetComponent<RectTransform>().sizeDelta.y;

        //GameObject‚ÌˆÊ’u‚ğæ“¾
        topRuledLineRectAncPos = topRuledLine.GetComponent<RectTransform>().anchoredPosition;
        middleRuledLineRectAncPos = middleRuledLine.GetComponent<RectTransform>().anchoredPosition;
        bottomRuledLineRectAncPos = bottomRuledLine.GetComponent<RectTransform>().anchoredPosition;
        scrollContentRectAncPos = scrollContent.GetComponent<RectTransform>().anchoredPosition;

        //middleRuledLine‚Ì‰ŠúˆÊ’u‚ğİ’è
        halfRuledLineHeight = RuledLineHeight / 2;
        halfScrollContentHeight = scrollContentHeight / 2;
        middleRuledLineRectAncPos.y = halfScrollContentHeight - halfRuledLineHeight;

        //topRuledLine‚ÆbottomRuledLine‚Ì‰ŠúˆÊ’u‚ğİ’è
        topRuledLineRectAncPos.y = middleRuledLineRectAncPos.y + difference;
        bottomRuledLineRectAncPos.y = middleRuledLineRectAncPos.y - difference;

        pastScrollContentRectAncPos = scrollContentRectAncPos.y;

    }

    void Update()
    {
        scrollContentRectAncPos = scrollContent.GetComponent<RectTransform>().anchoredPosition;
        if (scrollContentRectAncPos.y - pastScrollContentRectAncPos > halfScrollContentHeight)
        {

        }
    }
}
