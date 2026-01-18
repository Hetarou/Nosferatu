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
    private float lineHeight;

    bool isAjust;
    bool a = true;
    void OnEnable()
    {
        isAjust = false;

        float height1 = tmp.GetPreferredValues("A").y;      // 1行の高さ
        float height2 = tmp.GetPreferredValues("A\nA").y;   // 2行の高さ

        // この差分が、真の「1行分の高さ（行間込み）」です
        lineHeight = height2 - height1;


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
            visibleCount = Mathf.CeilToInt(viewportHeight / lineSpacing) ;
            if(a)
            {
                Debug.Log(visibleCount);
                a = !a;
            }
            for (int i = 0; i < visibleCount; i++)
            {
                RectTransform line = Instantiate(linePrefab, parent);
                line.sizeDelta = new Vector2(parent.rect.width, 2);
                linePool.Add(line);
            }

            // スクロールイベント登録
            scrollRect.onValueChanged.AddListener(_ => UpdateLines());
            
            UpdateLines();

            Canvas.ForceUpdateCanvases(); // 「今すぐ高さを計算し直せ！」という強制命令
            scrollRect.verticalNormalizedPosition = 0f; // 「よし、その高さの0地点（一番下）へ行け！」
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

        // 現在のスクロール位置
        float contentTopY = parent.anchoredPosition.y;
        int topIndex = Mathf.FloorToInt(contentTopY / lineHeight);

        for (int i = 0; i < visibleCount; i++)
        {
            RectTransform line = linePool[i];
            int lineIndex = topIndex + i;

            if (lineIndex >= totalLineCount || lineIndex < 0)
            {
                line.gameObject.SetActive(false);
            }
            else
            {
                line.gameObject.SetActive(true);

                var lineInfo = textInfo.lineInfo[lineIndex];

                // 1. TMPローカル空間での「行の下端」の座標を取得
                // (Xは中心=0としておきます。Yは行の下端)
                Vector3 localPosInTmp = new Vector3(0, lineInfo.lineExtents.min.y, 0);

                // 2. それを「ワールド座標」に変換
                // (tmpオブジェクトが画面上のどこにあっても、絶対的な世界の位置に変換されます)
                Vector3 worldPos = tmp.transform.TransformPoint(localPosInTmp);

                // 3. そのワールド座標を「Content (parent) のローカル座標」に逆変換
                // (これで line が Content の中のどこに配置されるべきかが正確に出ます)
                Vector2 localPosInContent = parent.InverseTransformPoint(worldPos);

                // 4. 座標を適用
                // X座標は 0 (Contentの中央) にするか、元々のズレを維持するか選べますが
                // とりあえず Y座標 はこれで完璧に合います。
                line.anchoredPosition = new Vector2(0, localPosInContent.y + heightAjust+2.5f);
            }
        }
    }
}
