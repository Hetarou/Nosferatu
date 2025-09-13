using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackTitleDirector : MonoBehaviour
{
    [SerializeField]
    GameObject backTitle;
    [SerializeField]
    List<GameObject> objList = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        int i;
        if (Input.GetMouseButtonDown(1))
        {
            //タイトルに戻りますかのオブジェクトのみを表示し、あとは非表示にする
            backTitle.SetActive(true);
            for (i = 0; i < objList.Count; i++)
            {
                objList[i].SetActive(false);
            }
        }
    }
}
