using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class TMPLineDrawer : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public RectTransform linePrefab;
    public RectTransform parent;
    public ScrollRect scrollRect;

    private List<RectTransform> linePool = new List<RectTransform>();
    private int visibleCount;
    private float lineSpacing;

    bool isAjust;

    void OnEnable()
    {
        isAjust = false;
    }

    private void LateUpdate()
    {
        if (!isAjust)
        {
            // TMPの行情報を更新
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
            tmp.ForceMeshUpdate();
            var textInfo = tmp.textInfo;

            
            

            int totalLineCount = textInfo.lineCount;

            // 行の高さを計算（1行目の高さを基準にする）
            lineSpacing = textInfo.lineInfo[0].lineHeight;

            // Viewportに必要な本数だけ確保（＋バッファ）
            float viewportHeight = scrollRect.viewport.rect.height;
            visibleCount = Mathf.CeilToInt(viewportHeight / lineSpacing) + 2;

            for (int i = 0; i < visibleCount; i++)
            {
                RectTransform line = Instantiate(linePrefab, parent);
                line.sizeDelta = new Vector2(parent.rect.width, 2);
                linePool.Add(line);
            }

            // スクロールイベント登録
            scrollRect.onValueChanged.AddListener(_ => UpdateLines());
            UpdateLines();

            isAjust = true;
        } 
    }
    void OnDisable()
    {
        // 無効化されたら罫線を全部破壊
        foreach (var line in linePool)
        {
            if (line != null)
                Destroy(line.gameObject);
        }
        linePool.Clear();
    }

    void UpdateLines()
    {
        tmp.ForceMeshUpdate();
        var textInfo = tmp.textInfo;
        int totalLineCount = textInfo.lineCount;

        float heightAjust = parent.rect.height / 2;
        Debug.Log(heightAjust);

        // 現在のスクロール位置
        float contentTopY = parent.anchoredPosition.y;
        int topIndex = Mathf.FloorToInt(contentTopY / lineSpacing);

        for (int i = 0; i < linePool.Count; i++)
        {
            int lineIndex = topIndex + i;
            if (lineIndex >= totalLineCount || lineIndex < 0)
            {
                linePool[i].gameObject.SetActive(false);
            }
            else
            {
                linePool[i].gameObject.SetActive(true);

                var lineInfo = textInfo.lineInfo[lineIndex];
                float yPos = lineInfo.lineExtents.min.y + heightAjust;

                linePool[i].anchoredPosition = new Vector2(0, yPos);
            }
        }
    }
}
