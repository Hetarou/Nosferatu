using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PannelExcuter : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private string NameToShow;
    [SerializeField]
    private string TextToShow;
    [SerializeField]
    private float charDelay;

    [SerializeField]
    private PannelExcuterSystem pannelExcuterSystem;
    [SerializeField]
    private TMP_Text NameComponent;
    [SerializeField]
    private TMP_Text TextComponent;

    private Coroutine coroutine;
    void Start()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("マウスが入った！");
        //他のPanelExcuter達のisTypingをfalseにする
        pannelExcuterSystem.GetOnPointerEnter(this);

    }
    public void CancelTyping()
    {
        StopCoroutine(coroutine);
    }
    public void StartExcution()
    {
        coroutine = StartCoroutine(DisplayChar(TextToShow));
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
