using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems; // EventSystemsをusing

// IPointerClickHandler を追加
public abstract class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private string thisFunction;
    [SerializeField] private float scaleRate = 1.1f;

    [Header("SEを流すためのフィールド")]
    [SerializeField] private AudioSource clikSESource;
    [SerializeField] private AudioClip clikSEClip;

    // マウスカーソルとオブジェクトが重なっているかを調べる
    public void OnPointerEnter(PointerEventData eventData)
    {
        PublicStaticStatus.IsEnter = true;
        transform.localScale *= scaleRate;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PublicStaticStatus.IsEnter = false;
        transform.localScale = new(1.0f, 1.0f, 1.0f);
    }

    // オブジェクトがクリックされた時に呼ばれる
    public void OnPointerClick(PointerEventData eventData)
    {
        // eventData.button を使えば、左クリックか右クリックかも判定できます
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log(thisFunction + " がクリックされました！");
            transform.localScale = new(1.0f, 1.0f, 1.0f);

            StartCoroutine(PerformPostClickAction()); // 抽象メソッドを呼ぶ
        }
    }

    public abstract void ExecuteCustomLogic();

    protected virtual IEnumerator PerformPostClickAction()
    {
        if (clikSESource != null && clikSEClip != null)
        {
            clikSESource.PlayOneShot(clikSEClip);

            // 2. SEが鳴り終わるまで待機
            // PlayOneShot直後はisPlayingが即座に反映されない場合があるため少し待つか、
            // クリップの長さを直接待つのが確実です
            yield return new WaitForSeconds(clikSEClip.length-0.6f);


        }

        ExecuteCustomLogic();

        yield return null;
    }
}
