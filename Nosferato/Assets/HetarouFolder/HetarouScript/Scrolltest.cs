using UnityEngine;
using UnityEngine.UI;

public class Scrolltest : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float speed = 0.2f; // スクロール速度

    void Update()
    {
        // 現在の位置を取得
        Vector2 pos = scrollRect.normalizedPosition;

        // Yを少しずつ減らす（0 = 下端, 1 = 上端）
        pos.y -= speed * Time.deltaTime;

        // 範囲を制限
        pos.y = Mathf.Clamp01(pos.y);

        // 適用
        scrollRect.normalizedPosition = pos;
    }
}
