using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button_Config : MonoBehaviour, IPointerClickHandler//ScriptでSelectedColor解除のタイミングを決めたいため、Buttonではない
{
    [SerializeField]
    private Sprite normalSprite;
    [SerializeField]
    private Sprite selectedSprite;

    private Image image;
    private bool isSelected = false;
    private Buttons_Config buttons_Config;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = normalSprite;
        buttons_Config = transform.parent.GetComponent<Buttons_Config>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ExcuteSave();
        Select();
    }
    public void Select()
    {
        isSelected = true;
        image.sprite = selectedSprite;

        foreach (Button_Config component in buttons_Config.components)
        {
            if (component != this)
            {
                component.Deselect();
            }
        }
    }

    public void Deselect()
    {
        isSelected = false;
        image.sprite = normalSprite;
    }

    private void ExcuteSave()
    {
        UserDataToSave myUserDataToSave = new UserDataToSave()
        {
            Volume = 2
        };


        string json = JsonUtility.ToJson(myUserDataToSave, true);
        Debug.Log("Master" + myUserDataToSave.Volume);
        Debug.Log(json);
        PlayerPrefs.SetString("Default",json);
        PlayerPrefs.Save();
    }
}