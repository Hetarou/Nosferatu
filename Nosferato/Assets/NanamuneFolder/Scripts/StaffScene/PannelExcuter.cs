using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PannelExcuter : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    //NameTextとText
    [SerializeField]
    private string NameToShow;
    [SerializeField]
    private string TextToShow;
    [SerializeField]
    private float charDelay;
    [SerializeField]
    private TMP_Text NameComponent;
    [SerializeField]
    private TMP_Text TextComponent;
    [SerializeField]
    private PannelExcuterSystem pannelExcuterSystem;

    //スクロール
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float pixelsPerSecond = 50f; // 1秒間に進むピクセル数
    [SerializeField] private float currentContentHeight;
    bool a = false;

    //重なってる間Image
    [SerializeField]
    private Sprite SNSSprite;
    [SerializeField]
    private Sprite SNSTransparent;
    [SerializeField]
    private Sprite nameSpriteA;
    [SerializeField]
    private Sprite nameSpriteB;

    [SerializeField]
    private Image NameImage;
    [SerializeField]
    private Image SNSImage;

    [SerializeField]
    AudioClip audioClip0;

    private Coroutine coroutine;

    private void Start()
    {
        switch (PublicStaticStatus.ReadingSpeed)
        {
            case 0:
                charDelay = 0.12f;
                break;
            case 1:
                charDelay = 0.07f;
                break;
            case 2:
                charDelay = 0.035f;
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SimpleAudioManager_SE.instance.PlaySE(audioClip0);
        Debug.Log("マウスが入った！");
        NameImage.sprite= nameSpriteB;
        pannelExcuterSystem.GetOnPointerEnter(this);//他のPanelExcuter達のisTypingをfalseにする
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        NameImage.sprite = nameSpriteA;
    }
    public void CancelTyping()
    {
        SNSImage.sprite = SNSTransparent;
        StopCoroutine(coroutine);
    }
    public void StartExcution()
    {
        SNSImage.sprite = SNSSprite;
        coroutine = StartCoroutine(DisplayChar(TextToShow));
    }

    void Update()
    {
        //画面スクロールする
        if (scrollRect == null || scrollRect.content == null) return;
        // 現在のピクセル座標を取得
        Vector2 pos = scrollRect.content.anchoredPosition;
        // Contentの高さからスクロール可能な最大値（下端）を計算
        // ScrollRectの高さ(Viewport)を引いた分が、動かせる最大範囲
        float contentHeight = scrollRect.content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;
        float maxScrollY = Mathf.Max(0, contentHeight - viewportHeight);
        if (contentHeight >= currentContentHeight)
        {
            if (pos.y >= maxScrollY) return;

            // Y座標を加算して上に動かす（＝画面上は下にスクロールする）
            pos.y += pixelsPerSecond * Time.deltaTime;

            // 範囲を制限（0～最大値）
            pos.y = Mathf.Clamp(pos.y, 0, maxScrollY);

            // 座標を適用
            scrollRect.content.anchoredPosition = pos;
        }
        else
        {
            pos.y = 0f;
        }
        // 座標を適用
        scrollRect.content.anchoredPosition = pos;
        currentContentHeight = scrollRect.content.rect.height;
    }
    private IEnumerator DisplayChar(string message)
    {
        NameComponent.text = NameToShow;
        TextComponent.text = "";
        /*
        //ルビを振る仕組み。ここでは不採用//例えば38スレ目
        foreach (char c in message)
        {
            if (csvData[rowNumber][7].Length != 0)
            {
                if (c == '_') WaitTmpRuby();//
                else if (c == '|') TmpIcon();
            }
            yield return new WaitForSeconds(charDelay);
        }*/
        foreach (char c in message)
        {
            TextComponent.text += c;
            yield return new WaitForSeconds(charDelay);
        }

        Debug.Log(TextComponent.text);

        //コルーチン終了の旧仕様
        /*if (isHidingRequested)
        {
            Debug.Log("HideAfterTyping");
            isTyping = false;
            //skipRequested = false;
            gameObject.SetActive(false); // ★ここで非表示実行
            isHidingRequested = false;
            yield break; // ★コルーチンを完全に終了
        }*/

        // 文字送り終了時
        //謎
        //waitObj.SetActive(true);
        //謎
        //waitAnim.SetBool("isWaitAnim", true);
        Debug.Log("TypingFinished");
    }
}
