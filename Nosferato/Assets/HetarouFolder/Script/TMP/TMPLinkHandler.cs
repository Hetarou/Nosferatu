using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TMPLinkHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI tmpText;
    public GameObject popupPrefab;
    private Camera cachedCamera;
    private RectTransform canvasRect;

    // 状態管理用
    private bool isHoveringObject = false;
    private int currentLinkIndex = -1;
    private GameObject currentPopup;

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        Canvas canvas = tmpText.canvas;
        if (canvas != null)
        {
            // オーバーレイの場合はnull、それ以外はWorldCameraを使う
            cachedCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            canvasRect = canvas.GetComponent<RectTransform>();
        }
    }

    void Update()
    {
        // マウスがテキストオブジェクトのRect外にいるなら処理しない
        if (!isHoveringObject) return;

        CheckLinkHover();
    }

    private void CheckLinkHover()
    {
        // マウス位置のリンクを取得
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(tmpText, Input.mousePosition, cachedCamera);

        // --- 状態が変わった時のみ処理を行う ---
        if (linkIndex != currentLinkIndex)
        {
            currentLinkIndex = linkIndex;

            // 古いポップアップがあれば消す
            if (currentPopup != null)
            {
                Destroy(currentPopup);
                currentPopup = null;
            }

            // 新しいリンク上にいるならポップアップを出す
            if (linkIndex != -1)
            {
                ShowPopup(linkIndex);
            }
        }
    }

    private void ShowPopup(int linkIndex)
    {
        TMP_LinkInfo linkInfo = tmpText.textInfo.linkInfo[linkIndex];

        // リンクの末尾の文字座標を基準にする
        int charIndex = linkInfo.linkTextfirstCharacterIndex + linkInfo.linkTextLength - 1;

        // 文字情報の更新が必要な場合があるためチェック
        if (charIndex >= tmpText.textInfo.characterInfo.Length) return;

        Vector3 charBRLocal = tmpText.textInfo.characterInfo[charIndex].bottomRight;
        Vector3 worldPos = tmpText.transform.TransformPoint(charBRLocal);
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(cachedCamera, worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, cachedCamera, out Vector2 localPos);

        // === ポップアップ生成 ===
        currentPopup = Instantiate(popupPrefab, tmpText.canvas.transform, false);
        RectTransform popupRect = currentPopup.GetComponent<RectTransform>();

        // 設定
        popupRect.pivot = new Vector2(0f, 0f); // 左上基準等はPrefab設定に合わせて調整してください
        popupRect.anchoredPosition = localPos + new Vector2(0f, 10f);

        // === テキスト設定 ===
        TextMeshProUGUI popupText = currentPopup.GetComponentInChildren<TextMeshProUGUI>();
        if (popupText != null)
        {
            popupText.text = linkInfo.GetLinkID();
        }
    }

    // マウスがテキストのRect内に入った
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHoveringObject = true;
    }

    // マウスがテキストのRectから出た
    public void OnPointerExit(PointerEventData eventData)
    {
        isHoveringObject = false;
        currentLinkIndex = -1;

        // 枠外に出たら即ポップアップを消す
        if (currentPopup != null)
        {
            Destroy(currentPopup);
            currentPopup = null;
        }

        tmpText.color = Color.white; // 元の色に戻す処理があれば
    }
}