using UnityEngine;
using UnityEngine.UI;

public class FixedVerticalScrollbar : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private float fixedSize = 0.2f; // つまみサイズ（0〜1）

    void Start()
    {
        // つまみのサイズを固定
        scrollbar.size = fixedSize;

        // Scrollbar → ScrollRect
        scrollbar.onValueChanged.AddListener(OnScrollbarChanged);
    }

    void Update()
    {
        // ScrollRect の進捗率を計算して Scrollbar に反映
        float progress = scrollRect.verticalNormalizedPosition;
        scrollbar.value = progress;
    }

    private void OnScrollbarChanged(float value)
    {
        // Scrollbar の進捗率を ScrollRect に反映
        float normalized = value;
        scrollRect.verticalNormalizedPosition = normalized;
    }
}