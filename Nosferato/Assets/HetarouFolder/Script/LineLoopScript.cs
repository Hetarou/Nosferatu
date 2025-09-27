using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LineLoopScript : MonoBehaviour
{
    [SerializeField] private RawImage background;      // 罫線用
    [SerializeField] private RectTransform content;    // ScrollView Content
    [SerializeField] private TextMeshProUGUI sampleText; // Content 内のログText

    private float lineHeight;

    void Start()
    {
        // TMP の行高を取得
        sampleText.ForceMeshUpdate();
        lineHeight = sampleText.preferredHeight;

        // 背景テクスチャをループ表示可能に
        if (background.texture != null)
            background.texture.wrapMode = TextureWrapMode.Repeat;
    }

    void LateUpdate()
    {
        if (lineHeight <= 0 || background.texture == null) return;

        float tileCount = content.rect.height / lineHeight;

        // ScrollRect のスクロール方向に合わせて符号を調整
        float offsetY = (-content.anchoredPosition.y / lineHeight) % 1f;

        // 罫線をループ表示
        background.uvRect = new Rect(0, offsetY, 1, tileCount);
    }
}
