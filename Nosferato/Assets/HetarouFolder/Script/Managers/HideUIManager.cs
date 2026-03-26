using UnityEngine;

public class HideUIManager : MonoBehaviour
{
    [SerializeField] private GameMode gameMode;
    [SerializeField] private CanvasGroup menuGroup;
    [SerializeField] private CanvasGroup messageWindowGroup;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!gameMode.CanHideUI)
            {
                OffHideUI();
            }
            else
            {
                OnHideUI();
            }
        }
    }

    private void OffHideUI()
    {
        if(gameMode.ModeName == "BackLog") return;

        gameMode.ModeManager(gameMode.LastModeName);

        menuGroup.alpha = 1.0f;
        menuGroup.blocksRaycasts = true;
        messageWindowGroup.alpha = 1.0f;
        messageWindowGroup.blocksRaycasts = true;

        Debug.Log("”ñ”­‰Î");
    }

    private void OnHideUI()
    {
        gameMode.ModeManager("HideUI");

        menuGroup.alpha = 0f;
        menuGroup.blocksRaycasts = false;
        messageWindowGroup.alpha = 0f;
        messageWindowGroup.blocksRaycasts =false;

        Debug.Log("”­‰Î");
    }
}
