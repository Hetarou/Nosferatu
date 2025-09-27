using UnityEngine;

public class SaveButton : MonoBehaviour
{
    [SerializeField]
    private int slotNum;
    
    private void Update()
    {
        
        /*if (Input.GetKeyDown(KeyCode.K))
        {
            ExcuteSave(1);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnLoad(1);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ExcuteSave(2);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            OnLoad(2);
        }*/
    }
    public void OnClick()
    {
        //SceneManager.LoadScene("SenarioScene");
        Debug.Log("SaveButton Clicked");
        ExcuteSave(slotNum);
    }
    public void ExcuteSave(int slot)
    {
        string st = Random.Range(1, 100) + "stage";
        Debug.Log(st);
        UserDataToSave data = new UserDataToSave()
        {
            savedStageName = st//StageName
        };


        string json = JsonUtility.ToJson(data, true);
        string key = $"PlayerUserData{slot}";
        Debug.Log(json);// jsonデータにできたかどうか確認
        Debug.Log("セーブ" + slot + "にセーブしました");

        //jsonデータにしたやつらをここに格納
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public void OnLoad(int slot)
    {
        string key = $"PlayerUserData{slot}";
        if (PlayerPrefs.HasKey(key))
        {
            //jsonデータにしたやつをここで元に戻す
            string json = PlayerPrefs.GetString(key);
            UserDataToSave data = JsonUtility.FromJson<UserDataToSave>(json);

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
    }
}
