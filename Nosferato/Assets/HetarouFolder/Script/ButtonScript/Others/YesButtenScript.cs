using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YesButtonScript : ButtonScript
{
    [SerializeField] string SceneToLoad;

    [SerializeField] private Sprite mouseOutsprite;

    [SerializeField] private Image thisImage;

    public override void ExecuteCustomLogic()
    {
        thisImage.sprite = mouseOutsprite;
        SceneManager.LoadScene(SceneToLoad);
    }
}
