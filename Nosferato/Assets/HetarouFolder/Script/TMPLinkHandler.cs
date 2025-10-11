using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TMPLinkHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI tmpText;
    public GameObject popupPrefab; // Image + TMPText のプレハブ

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(tmpText, eventData.position, eventData.pressEventCamera);
        if (linkIndex == -1) return;

        tmpText.ForceMeshUpdate();
        TMP_LinkInfo linkInfo = tmpText.textInfo.linkInfo[linkIndex];

        // リンクの末尾の文字座標を基準にする
        int charIndex = linkInfo.linkTextfirstCharacterIndex + linkInfo.linkTextLength - 1;
        Vector3 charBRLocal = tmpText.textInfo.characterInfo[charIndex].bottomRight;
        Vector3 worldPos = tmpText.transform.TransformPoint(charBRLocal);

        Camera cam = tmpText.canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : tmpText.canvas.worldCamera;
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(cam, worldPos);

        RectTransform canvasRect = tmpText.canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, cam, out Vector2 localPos);

        // === ポップアップ生成 ===
        GameObject popupObj = Instantiate(popupPrefab, tmpText.canvas.transform, false);
        RectTransform popupRect = popupObj.GetComponent<RectTransform>();
        popupRect.pivot = new Vector2(0f, 0f); // 左上を基準
        popupRect.anchoredPosition = localPos + new Vector2(0f, 10f);

        // === テキスト設定 ===
        TextMeshProUGUI popupText = popupObj.GetComponentInChildren<TextMeshProUGUI>();
        popupText.text = linkInfo.GetLinkID();

        // === 自動で消す場合 ===
        Destroy(popupObj, 2f);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(tmpText, Input.mousePosition, eventData.pressEventCamera);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = tmpText.textInfo.linkInfo[linkIndex];
            Debug.Log("マウスオーバー中のリンク: " + linkInfo.GetLinkText());//Debug.Log("クリックされたリンク: " + linkInfo.GetLinkID());
            tmpText.color = Color.red; // とりあえず全体を赤に
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tmpText.color = Color.white;
    }
}
