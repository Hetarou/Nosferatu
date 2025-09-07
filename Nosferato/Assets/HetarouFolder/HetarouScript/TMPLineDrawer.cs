using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TMPLineDrawer : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public RectTransform linePrefab;
    public RectTransform parent;

    bool isAjust;

    float H;

    void Awake()
    {
        isAjust = false;
    }

    private void LateUpdate()
    {
        if(isAjust == false)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
            tmp.ForceMeshUpdate();
            H = parent.rect.height;
            Debug.Log(H);
            GenerateLines();
            isAjust = true;
        }
    }

    

    void GenerateLines()
    {
        // TMPの行情報を最新にする
        

        var textInfo = tmp.textInfo;
        int lineCount = textInfo.lineCount;

        for (int i = 0; i < lineCount; i++)
        {
            var lineInfo = textInfo.lineInfo[i];

            // 行の下端のy座標（ローカル座標系）
            float yPos = lineInfo.lineExtents.min.y + H/2;

            // 線を生成
            RectTransform line = Instantiate(linePrefab, parent);
            line.anchoredPosition = new Vector2(0, yPos);
            line.sizeDelta = new Vector2(parent.rect.width, 2); // 横幅いっぱい、太さ2px
        }
    }
}
