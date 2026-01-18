using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

public class ExacuteSkip : ButtonScript
{
    public TextDisplayerRuby displayer;
    public int[] targetRows = new int[] { 52, 302, 431, 513 };
    [SerializeField] private float acceleration;

    // スキップ中かどうかの状態
    private bool isSkipping = false;
    private Coroutine skipCoroutine;

    // 元の速度を保存する変数
    private float originalCharDelay;
    private float originalAutoDelay;

    [SerializeField] private GameMode GameMode;

    [SerializeField] private TextMeshProUGUI backLogText;

    [SerializeField] private ExacuteAuto ExacuteAuto;


    // [追加] Awakeで元の速度を保存
    void Awake()
    {
        // 起動時にオリジナルの速度を一度だけ保存する
        originalCharDelay = displayer.charDelay;
        originalAutoDelay = displayer.autoDelay;
    }

    // [修正] OnEnable のロジック
    void OnEnable()
    {
        // OnEnable では、コルーチンが止まっていることを前提に、
        // 状態（フラグと速度）をリセットする。

        if (isSkipping)
        {
            // スキップ中だった（フラグがtrue）なら、
            // 強制的に停止状態に戻す
            Debug.Log("Skip flag was true. Forcing stop on OnEnable.");

            // StopSkip() はコルーチン停止、速度復元、フラグリセットを
            // すべて行ってくれるので、これを呼ぶだけで良い。
            StopSkip();
        }
        else
        {
            // スキップ中でなかった場合
            // 念のため、速度がオリジナルに戻っていることを確認
            // (非アクティブ中に外部から速度が変更された可能性に対処)
            displayer.charDelay = originalCharDelay;
            displayer.autoDelay = originalAutoDelay;

            if (skipCoroutine != null)
            {
                // 本来ここには来ないはずだが、念のため
                StopCoroutine(skipCoroutine);
                skipCoroutine = null;
            }
        }
    }

    /// <summary>
    /// ボタンが押された時の処理
    /// </summary>
    public override void ExecuteCustomLogic()
    {
        //if (GameMode.modeSkip)
        //{
            isSkipping = !isSkipping;

            // [修正] GameMode.ModeManager の呼び出し位置
            // ExcuteButtonが呼ばれた時点で "Skip" に遷移開始
            GameMode.ModeManager("Skip");

            if (isSkipping)
            {
                StartSkip();
            }
            else
            {
                Debug.Log("Skip STOP requested by user.");
                StopSkip();
            }
        //}
        //else
        //{
            //Debug.Log("Skipできません！");
        //}
    }

    /// <summary>
    /// スキップを開始する処理
    /// </summary>
    private void StartSkip()
    {
        int nextTarget = JumpToClosestNextTarget();

        if (nextTarget != -1)
        {
            if (skipCoroutine != null)
            {
                StopCoroutine(skipCoroutine);
            }

            // [削除] 速度の保存処理 (Awakeで実施済)
            // originalCharDelay = displayer.charDelay;
            // originalAutoDelay = displayer.autoDelay;

            // スキップ用の速度に設定 (Awakeで保存した値を使う)
            displayer.charDelay = originalCharDelay / acceleration;
            displayer.autoDelay = originalAutoDelay / acceleration;

            skipCoroutine = StartCoroutine(SkipLoop(nextTarget));
            Debug.Log("Skip START -> Row " + nextTarget);
        }
        else
        {
            Debug.Log("ジャンプできる次のターゲットがありません。");
            // [修正] ターゲットがない場合も停止処理を呼ぶ
            StopSkip();
        }
    }

    /// <summary>
    /// [変更点] スキップを停止する処理（手動・自動兼用）
    /// </summary>
    private void StopSkip()
    {
        // [修正のポイント]
        // 止めるべきコルーチンが *存在する* かどうかで判断する。
        // (isSkipping フラグで判断すると、トグル直後で不整合が起きる)
        if (skipCoroutine != null)
        {
            // 1. コルーチンを停止
            StopCoroutine(skipCoroutine);
            skipCoroutine = null; // ハンドルをクリア

            // 2. 速度を元に戻す
            displayer.charDelay = originalCharDelay;
            displayer.autoDelay = originalAutoDelay;
            Debug.Log("Speed restored.");
        }

        // 3. 状態をリセット
        // (StopSkipが呼ばれたら、理由に関わらず「非スキップ状態」にする)
        ExacuteAuto.StopCoroutine();
        isSkipping = false;
    }


    /// <summary>
    /// ジャンプ先を探す「だけ」の関数
    /// </summary>
    public int JumpToClosestNextTarget()
    {
        var validTargets = targetRows.Where(target => target > displayer.rowNumber);
        return validTargets.Any() ? validTargets.Min() : -1;
    }

    /// <summary>
    /// ターゲットまで自動で進めるコルーチン
    /// </summary>
    private IEnumerator SkipLoop(int nextTarget)
    {
        // スキップ中は「演出」を無効化する設定が displayer にあれば呼ぶ
        // displayer.SetSkipMode(true); 

        backLogText.text = "";

        /*while (displayer.rowNumber < nextTarget)
        {
            if (!isSkipping) yield break;

            // 次の行へ進める（演出を待たずに内部インデックスだけ進める）
            displayer.rowNumber++;

            // ターゲットに到達するまでは、最小限の更新だけ行う
            if (displayer.rowNumber >= nextTarget)
            {
                displayer.AnyDisplay_Start(); // 最後に到達した行だけしっかり表示
            }
            else
            {
                // ここで「演出なしの内部更新」メソッドがあればベスト
                // displayer.UpdateDataOnly(); 
            }

            // 1フレームに1行だと速すぎる場合は、少しだけ待つ
            yield return null;
        }*/
        yield return new WaitUntil(() => !displayer.isTyping);

        displayer.rowNumber = nextTarget;

        displayer.AnyDisplay_Start();

        StopSkip();

        yield break;
    }
}