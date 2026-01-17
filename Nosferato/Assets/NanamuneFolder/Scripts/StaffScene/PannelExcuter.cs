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
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private float speed = 0.2f; // スクロール速度

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
        Vector2 pos = scrollRect.normalizedPosition;// 現在の位置を取得
        pos.y -= speed * Time.deltaTime;// Yを少しずつ減らす（0 = 下端, 1 = 上端）
        pos.y = Mathf.Clamp01(pos.y);// 範囲を制限
        scrollRect.normalizedPosition = pos;// 適用
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
