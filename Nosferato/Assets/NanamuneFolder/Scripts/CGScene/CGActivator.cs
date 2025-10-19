using System.Collections.Generic;
using UnityEngine;

public class CGActivator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> CGs = new List<GameObject>();
    private int CGsToDisplay;
    private void Awake()
    {
        CGsToDisplay = CalculateCGsToDisplay();
        for (int i = 0; i < CGsToDisplay; i++)
        {
            CGs[i].SetActive(true);
        }
    }
    private int CalculateCGsToDisplay()
    {
        return (6);
    }

}
