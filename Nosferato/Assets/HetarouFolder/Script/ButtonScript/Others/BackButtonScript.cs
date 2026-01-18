using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackButtonScript : ButtonScript
{
    public GameObject exacutedObject;

    [SerializeField] private GameMode gameMode;

    public override void ExecuteCustomLogic()
    {
        PublicStaticStatus.IsEnter = false;
        exacutedObject.SetActive(false);
        gameMode.ModeManager(gameMode.lastModeName);
    }
}
