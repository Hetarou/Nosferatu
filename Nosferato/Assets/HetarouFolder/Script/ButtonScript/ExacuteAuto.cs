using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class ExacuteAuto : ButtonScript_Test
{
    public static bool isAuto = false;

    public List<Image> autoImages = new List<Image>();

    public TextDisplayerRuby displayer;

    public List<Sprite> normalAutoSprite = new List<Sprite>();
    public List<Sprite> specificAutoSprite = new List<Sprite>();
    public override void ExcuteButton()
    {
        isAuto = !isAuto;
        if (isAuto)
        {
            for (int i = 0; i < autoImages.Count; i++)
            {
                autoImages[i].sprite = specificAutoSprite[i];
            }

            displayer.AnyDisplay();
        }
        else
        {
            for (int i = 0; i < autoImages.Count; i++)
            {
                autoImages[i].sprite = normalAutoSprite[i];
            }
        }
            Debug.Log("�Ă΂ꂽ");
    }

    
}
