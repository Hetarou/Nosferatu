using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExacuteAuto : ButtonScript
{
    public static bool isAuto = false;

    public static bool hasSkiped = false;

    public List<Image> autoImages = new List<Image>();

    public TextDisplayerRuby displayer;

    public List<Sprite> normalAutoSprite = new List<Sprite>();
    public List<Sprite> specificAutoSprite = new List<Sprite>();

    private Coroutine autoCoroutine;

    [SerializeField] private GameMode GameMode;

    // (ExcuteButton, AutoLoopなどのメソッドはそのまま)

    // このメソッドを追加
    void OnEnable()
    {
        // GameObjectがアクティブになった時、isAuto の状態に基づいて
        // UI（スプライト）とコルーチンの状態を復元します。

        if (isAuto)
        {
            /*// isAutoがtrue（オートモードであるべき）な場合

            // 1. 画像をオートモード用に設定
            for (int i = 0; i < autoImages.Count; i++)
            {
                if (i < specificAutoSprite.Count) // 念のため
                    autoImages[i].sprite = specificAutoSprite[i];
            }

            // 2. コルーチンを再開する
            // (OnDisableで停止しているので、再度StartCoroutineする)
            if (autoCoroutine != null)
            {
                // 念のため、古い（停止済みの）コルーチン参照でStopを呼ぶ
                StopCoroutine(autoCoroutine);
            }
            GameMode.ModeManager("Auto");
            autoCoroutine = StartCoroutine(AutoLoop());*/

            isAuto = false;
        }
        else
        {
            // isAutoがfalse（通常モードであるべき）な場合

            // 1. 画像を通常モード用に設定
            for (int i = 0; i < autoImages.Count; i++)
            {
                if (i < normalAutoSprite.Count) // 念のため
                    autoImages[i].sprite = normalAutoSprite[i];
            }

            // 2. コルーチンが動いていないことを確認 (基本不要だが安全のため)
            if (autoCoroutine != null)
            {
                StopCoroutine(autoCoroutine);
                autoCoroutine = null;
            }
        }
    }
    public override void ExecuteCustomLogic()
    {
        isAuto = !isAuto;

        if(GameMode.modeAuto)
        {
            GameMode.ModeManager("Auto");
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
                // 停止
                if (autoCoroutine != null)
                {
                    StopCoroutine();
                }
            }
        }
        else
        {
            Debug.Log("Autoできません！");
        }
        
    }

    private IEnumerator AutoLoop()
    {
        while (isAuto)
        {
            // displayer が文字送り中なら待つ
            yield return new WaitUntil(() => !displayer.isTyping);

            // 文字送りが終わった後に autoDelay を待つ
            if (!hasSkiped)
            {
                yield return new WaitForSeconds(displayer.autoDelay);
            }

            yield return new WaitUntil(() => GameMode.modeAuto);

            // 自動で次を表示
            displayer.rowNumber++;
            displayer.AnyDisplay();
            hasSkiped = false;
        }
        autoCoroutine = null; // 終了したらリセット
    }

    public void StopCoroutine()
    {
        for (int i = 0; i < autoImages.Count; i++)
        {
            autoImages[i].sprite = normalAutoSprite[i];
        }

        isAuto = false;
        GameMode.ModeManager("Reading");

        if (autoCoroutine != null)
        {
            StopCoroutine(autoCoroutine);
            autoCoroutine = null;
        }
    }

}