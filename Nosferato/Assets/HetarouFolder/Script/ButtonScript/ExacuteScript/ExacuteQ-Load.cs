using UnityEngine;

public class ExacuteQ_Load : ButtonScript
{
    public override void ExecuteCustomLogic()
    {
        PublicStaticStatus.IsEnter = false;
        QuickLoadExcuter.ExcuteQuickLoad();
    }
}
