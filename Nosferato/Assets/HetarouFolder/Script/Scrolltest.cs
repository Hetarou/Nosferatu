using UnityEngine;
using UnityEngine.UI;

public class Scrolltest : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float pixelsPerSecond = 50f; // 1秒間に進むピクセル数
    [SerializeField] private float currentContentHeight;
    bool a=false;

    private void Start()
    {
        //firstContentHeight = scrollRect.content.rect.height;
        Debug.Log(scrollRect.content);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))

        if (scrollRect == null || scrollRect.content == null) return;
        // 現在のピクセル座標を取得
        Vector2 pos = scrollRect.content.anchoredPosition;

        // Contentの高さからスクロール可能な最大値（下端）を計算
        // ScrollRectの高さ(Viewport)を引いた分が、動かせる最大範囲
        float contentHeight = scrollRect.content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;
        float maxScrollY = Mathf.Max(0, contentHeight - viewportHeight);

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(a);
        }


        if (contentHeight >= currentContentHeight)
        {
            if (pos.y >= maxScrollY) return;

            // Y座標を加算して上に動かす（＝画面上は下にスクロールする）
            pos.y += pixelsPerSecond * Time.deltaTime;

            // 範囲を制限（0 〜 最大値）
            pos.y = Mathf.Clamp(pos.y, 0, maxScrollY);

            // 座標を適用
            scrollRect.content.anchoredPosition = pos;

            a = true;
        }
        else
        {
            pos.y = 0f;
        }

        // 座標を適用
        scrollRect.content.anchoredPosition = pos;
        currentContentHeight = scrollRect.content.rect.height;
    }
}
