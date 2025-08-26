using KoganeUnityLib;
using UnityEngine;

public class Example : MonoBehaviour
{
    public TMP_Typewriter m_typewriter;
    public float m_speed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // 1 文字ずつ表示する演出を再生（ルビ対応）
            m_typewriter.Play
            (
                text: "abcde\n<r=asd>aaaa</r>aaaa\naaaa<r=a>a</r>a",
                speed: m_speed,
                onComplete: () => Debug.Log("完了")
                // ルビがある行とない行で高さが変動しないようにするにはtrue
                //fixedLineHeight:true
            // 1行目にルビがある時、TextMeshProのMargin機能を使って位置調整
            //autoMarginTop: true
            );
        }
        //...
    }
}
