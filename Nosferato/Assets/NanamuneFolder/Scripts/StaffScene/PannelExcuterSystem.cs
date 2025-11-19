using UnityEngine;

public class PannelExcuterSystem : MonoBehaviour
{
    private PannelExcuter lastPlannelExcuter;
    public void GetOnPointerEnter(PannelExcuter pannelExcuter)
    {
        if(lastPlannelExcuter)
        {
            lastPlannelExcuter.CancelTyping();
        }
        pannelExcuter.StartExcution();
        lastPlannelExcuter = pannelExcuter;
    }
}
