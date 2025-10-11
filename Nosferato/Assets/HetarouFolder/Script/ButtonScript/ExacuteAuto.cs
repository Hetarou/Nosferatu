using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExacuteAuto : ButtonScript_Test
{
    public static bool isAuto = false;

    public List<Image> autoImages = new List<Image>();

    public TextDisplayerRuby displayer;

    public List<Sprite> normalAutoSprite = new List<Sprite>();
    public List<Sprite> specificAutoSprite = new List<Sprite>();

    private Coroutine autoCoroutine;

    public override void ExcuteButton()
    {
        isAuto = !isAuto;

        if (isAuto)
        {
            for (int i = 0; i < autoImages.Count; i++)
                autoImages[i].sprite = specificAutoSprite[i];

            // すでに Auto コルーチンが走っていなければ開始
            if (autoCoroutine == null)
                autoCoroutine = StartCoroutine(AutoLoop());
        }
        else
        {
            for (int i = 0; i < autoImages.Count; i++)
                autoImages[i].sprite = normalAutoSprite[i];

            // 停止
            if (autoCoroutine != null)
            {
                StopCoroutine(autoCoroutine);
                autoCoroutine = null;
            }
        }
    }

    private IEnumerator AutoLoop()
    {
        while (isAuto)
        {
            // displayer が文字送り中なら待つ
            yield return new WaitUntil(() => !displayer.isTyping);

            // 文字送りが終わった後に autoDelay を待つ
            yield return new WaitForSeconds(displayer.autoDelay);

            // 自動で次を表示
            displayer.AnyDisplay();
        }
        autoCoroutine = null; // 終了したらリセット
    }
}
