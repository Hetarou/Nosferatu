using UnityEngine;

public class TitlesetSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject earlySet;
    [SerializeField]
    private GameObject allClearSet;

    private void Awake()
    {
        if (!PublicStaticStatus.IsCleared)
        {
            earlySet.SetActive(true);
            allClearSet.SetActive(false);
        }
        if (PublicStaticStatus.IsCleared)
        {
            earlySet.SetActive(false);
            allClearSet.SetActive(true);
        }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.C))
        {
            Debug.Log("c");
            PublicStaticStatus.IsCleared = true;
        }
    }
}
