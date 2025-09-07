using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VerticalVirtualScrollFixedContent : MonoBehaviour
{
    [Header("ScrollView設定")]
    public ScrollRect scrollRect;
    public RectTransform content;
    public GameObject itemPrefab;

    [Header("データ設定")]
    public int totalItemCount = 200;   // 総データ数
    public float itemHeight = 100f;    // アイテム高さ
    public float spacing = 5f;         // アイテム間隔

    private List<GameObject> pool = new List<GameObject>();
    private int visibleCount;
    private int topIndex = 0;

    public float contentHeigt;

    bool isAjust;

    void OnEnable()
    {
        isAjust = false;
        // 表示に必要なアイテム数を計算
        float viewportHeight = scrollRect.viewport.rect.height;
        visibleCount = Mathf.CeilToInt(viewportHeight / (itemHeight + spacing)) + 2;

        
        Debug.Log(contentHeigt);
        // プール生成（表示中の分だけ）
        for (int i = 0; i < visibleCount; i++)
        {
            GameObject obj = Instantiate(itemPrefab, content);
            pool.Add(obj);
        }

        // スクロールイベント登録
        scrollRect.onValueChanged.AddListener(OnScroll);

        // 初期表示
        UpdateItems();
    }

    private void LateUpdate()
    {
        if (!isAjust)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
            contentHeigt = content.rect.height / 2;
            UpdateItems();
            isAjust = true;
        }
    }
    void OnScroll(Vector2 scrollPos)
    {
        UpdateItems();
    }

    void UpdateItems()
    {
        // Content上端からの位置に応じて先頭アイテムを決定
        //contentHeigt = content.rect.height / 2;
        float contentTopY = content.anchoredPosition.y;
        topIndex = Mathf.FloorToInt(contentTopY / (itemHeight + spacing));

        for (int i = 0; i < pool.Count; i++)
        {
            int dataIndex = topIndex + i;

            if (dataIndex >= totalItemCount || dataIndex < 0)
            {
                pool[i].SetActive(false);
            }
            else
            {
                pool[i].SetActive(true);

                // Raycast制御（必要に応じて）
                Image img = pool[i].GetComponent<Image>();
                img.raycastTarget = true;

                // 等間隔で縦位置設定
                float yPos = -dataIndex * (itemHeight + spacing) + contentHeigt;
                
                pool[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yPos);
            }
        }
    }
}