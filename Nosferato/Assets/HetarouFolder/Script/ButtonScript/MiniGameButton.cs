using UnityEngine;

public class MiniGameButton : MonoBehaviour
{
    [SerializeField] private MiniGameLauncher miniGameLaunchar;
    public void OnClick()
    {
        miniGameLaunchar.LaunchMiniGame();
    }
}
