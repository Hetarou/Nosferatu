using UnityEngine;

public class PublicStaticStatus : MonoBehaviour
{
    public static int RowToSave = 2;//セーブ対象
    public static string DisplayedText;//いらなければ使わないけどどう？

    public static string PreviousScene;//PreviousSceneRecorder参照

    public static int ScreenMode = 0;//0:ウィンドウ,1:フルスクリーン,2:疑似フルスクリーン
    public static int Font = 0;//0:ラノベポップ,1:角ゴシック,2:明朝体
    public static int ReadingSpeed = 0;//0:普通,1:速い,2:瞬時
    public static int Volume = 5;//0-10
    public static int BGMVolume = 5;//0-10
    public static int SEVolume = 5;//0-10
    public static bool IsCleared = false;
    public static int BiggestRow = 0;

    public static bool IsEnter = false; //Menuのアイコンと重なっているか
}