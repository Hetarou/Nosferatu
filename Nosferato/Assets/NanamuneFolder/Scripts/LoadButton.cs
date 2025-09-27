using UnityEngine;

public class LoadButton : MonoBehaviour
{
    [SerializeField]
    private int slotNum;

    public void OnClick()
    {
        //SceneManager.LoadScene("SenarioScene");
        Debug.Log("SaveButton Clicked");
        ExcuteLoad(slotNum);
    }

    public void ExcuteLoad(int slot)
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
