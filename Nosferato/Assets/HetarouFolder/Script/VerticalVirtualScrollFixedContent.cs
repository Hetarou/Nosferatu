using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ATMPLineDrawer : MonoBehaviour
{
    public TextMeshProUGUI tmp;         // 対象のTextMeshPro
    public RectTransform linePrefab;    // 罫線用のPrefab（横線Image）
    public RectTransform parent;        // 配置先（通常はtmpの親）

    void Start()
    {
        GenerateLines();
    }

    void GenerateLines()
    {
        // TMPの行情報を更新
        tmp.ForceMeshUpdate();
        var textInfo = tmp.textInfo;

        int lineCount = textInfo.lineCount;

        for (int i = 0; i < lineCount; i++)
        {
            var lineInfo = textInfo.lineInfo[i];

            // 行の下端のY座標（ローカル座標）
            float yPos = lineInfo.lineExtents.min.y;

            // 罫線を生成
            RectTransform line = Instantiate(linePrefab, parent);
            line.anchoredPosition = new Vector2(0, yPos);
            line.sizeDelta = new Vector2(parent.rect.width, 2); // 横幅いっぱい、太さ2px
        }
    }
}
