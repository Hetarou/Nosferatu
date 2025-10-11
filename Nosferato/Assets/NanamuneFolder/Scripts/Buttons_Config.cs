using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Buttons_Config : MonoBehaviour
{
    public List<Button_Config> components = new List<Button_Config>();
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            components.Add(transform.GetChild(i).GetComponent<Button_Config>());
        }
    }

    private void Start()
    {
        Load();
        components[0].Select();
    }

    private void Load()
    {
    }
}