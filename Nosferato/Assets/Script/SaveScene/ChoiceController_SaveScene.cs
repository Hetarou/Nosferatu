using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChoiceController_SaveScene : MonoBehaviour
{
    [SerializeField]
    List<GameObject> ChoiceObject = new List<GameObject>();
    [SerializeField]
    GameObject BackButton;
    [SerializeField]
    GameObject ChoosingMenuObject;
    [SerializeField]
    GameObject AllMenuObject;
    [SerializeField]
    int ThisTypeNumber;

    GameObject ChoosingChoiceObject;

    public int CurrentLoadNumber;

    private int a = 1;
    private int b = 0;
    private int[,] ChoosingNumber = new int[3, 2]
    {
        {0, 1},
        {2, 3},
        {4, 5}
    };

    void Start()
    {
        //最初の選択肢に合わせる
        ChoosingChoiceObject = ChoiceObject[2];
        //カーソル合わせる
        ChoosingLoadBox(1, 0);
    }

    void Update()
    {
        //カーソルコントローラー
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
        {
            if (a == 1)
            {
                //NotNullChoice(a+1,b)、a+1,bの組み合わせが侵入可能か有効か判定、有効なら処理を続ける//直感的//説明しやすい//この時例外処理…だと説明しつらい//こだわりかも
                EndBox();
                a = 2;
                ChoosingLoadBox(a, b);
            }
            if (a == 0)
            {
                EndBox();
                a = 1;
                ChoosingLoadBox(a, b);
            }
        }

        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            if (a == 2)
            {
                EndBox();
                a = 1;
                ChoosingLoadBox(a, b);
            }

            else if (a == 1)
            {
                if (IsNotNullChoice(a - 1, b))
                {
                    EndBox();
                    a = 0;
                    ChoosingLoadBox(a, b);
                }
                else//×ボタンに行くため、固定で[0,1]地点に行く
                {
                    EndBox();
                    a = 0;
                    b = 1;
                    ChoosingLoadBox(a, b);
                }
            }
        }

        else if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && b == 0)
        {
            EndBox();
            b = 1;
            ChoosingLoadBox(a, b);
        }

        else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && b == 1)
        {
            if (IsNotNullChoice(a, b - 1))
            {
                EndBox();
                b = 0;
                ChoosingLoadBox(a, b);
            }
        }
        /*
        //カーソルコントローラー
        if((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && a == 0)
        {
            EndBox();
            a = 1;
            ChoosingLoadBox(a, b);
        }

        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            EndBox();
            if(a == 1)
            {
                a = 0;
                ChoosingLoadBox(a, b);
            }

            else if(a == 0)
            {
                ChoosingChoiceObject = BackButton;
                White();
            }
        }

        else if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && b == 0)
        {
            EndBox();
            b = 1;
            ChoosingLoadBox(a, b);
        }

        else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && b == 1)
        {
            EndBox();
            b = 0;
            ChoosingLoadBox(a, b);
        }
        */
        //マウス操作からカーソル操作に切り替える
        if (Cursor.visible == true && Input.anyKeyDown && !Input.GetMouseButtonDown(0))
        {
            Cursor.visible = false;
            EndBox();
            a = 1;
            b = 0;
            ChoosingLoadBox(a, b);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (ChoosingChoiceObject != BackButton)
            {
                //Debug.Log(ChoosingNumber[a, b]);
                ExcuteLoad(a, b);
            }

            else if (ChoosingChoiceObject == BackButton)
            {
                ChoosingMenuObject.SetActive(false);
                AllMenuObject.SetActive(true);
                GameObject.Find("StartMenuObject").GetComponent<StartMenuScript>().EndBox1(ThisTypeNumber);
                GameObject.Find("StartMenuObject").GetComponent<StartMenuScript>().StartBox(0);
            }
        }
    }

    private bool IsNotNullChoice(int numA, int numB)
    {
        if ((numA, numB) == (0, 0)) { return false; }
        else { return (true); }
    }

    public void ChoosingLoadBox(int VerticalNumber, int HorizontalNumber)
    {
        //Debug.Log("" + a + "," + b + "," + ChoosingNumber[a, b]);
        ChoosingChoiceObject = ChoiceObject[ChoosingNumber[VerticalNumber, HorizontalNumber]];
        Hilight();//選択中を分かりやすくする
    }

    public void EndBox()
    {
        ChoosingChoiceObject.GetComponent<Image>().color = new Color32(0, 0, 0, 125);//仮。ホントはこっち→//ChoosingChoiceObject.GetComponent<Image>().color = new Color32(255, 255, 255, 125);

    }

    int slot;
    public void ExcuteLoad(int VerticalNumber, int HorizontalNumber)
    {
        Debug.Log(ChoosingNumber[VerticalNumber, HorizontalNumber]);
        //ここでロードする
        if (ChoosingNumber[VerticalNumber, HorizontalNumber] == 3)
        {
            slot = 1;

        }
        if (ChoosingNumber[VerticalNumber, HorizontalNumber] == 4)
        {
            slot = 2;
        }
        string key = $"PlayerUserData{slot}";
        if (PlayerPrefs.HasKey(key))
        {
            //jsonデータにしたやつをここで元に戻す
            string json = PlayerPrefs.GetString(key);
            UserData data = JsonUtility.FromJson<UserData>(json);

            //ここでロード
            //Chikyu.transform.position = data.savedPosition;
            //Utyu.transform.position = data.savedPosition;
            //health = data.savedHealth;
            //SceneManager.LoadScene(data.savedStageName);
            Debug.Log(data.savedStageName);
            //Debug.Log(data.savedStageName);
            Debug.Log("セーブ" + slot + "をロードしました");
            Debug.Log("場所は" + data.savedStageName);
        }
        else
        {
            Debug.Log("PlayerUserDataが存在しません");
        }
        SceneManager.LoadScene("ActionScene06");
    }

    public void Hilight()
    {
        ChoosingChoiceObject.GetComponent<Image>().color = new Color32(255, 255, 255, 125);//仮//元の色に戻すという意味の処理
        //白を重ね掛けできるようにしたい。//MaskかShaderを使いそう
    }
}
