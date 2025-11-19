using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button_Config : MonoBehaviour, IPointerClickHandler//ScriptでSelectedColor解除のタイミングを決めたいため、Buttonではない
{
    public int Number;
    [SerializeField]
    private Sprite normalSprite;
    [SerializeField]
    private Sprite selectedSprite;
    private Image image;
    private Buttons_Config buttons_Config;

    private void Awake()
    {
        image = GetComponent<Image>();
        buttons_Config = transform.parent.GetComponent<Buttons_Config>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buttons_Config.GetClick(Number);
    }
    public void Deselect()
    {
        image.sprite = normalSprite;
    }
    public void Select()
    {
        image.sprite = selectedSprite;
    }

}