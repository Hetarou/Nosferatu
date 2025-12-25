using UnityEditor;
using UnityEngine;

public class ExacuteExit : ButtonScript
{
    Animator menuAnim;
    [SerializeField]
    GameObject Menu;
    [SerializeField]
    bool isMenu;

    void Start()
    {
        menuAnim = Menu.GetComponent<Animator>();
    }
    public override void ExcuteButton()
    {
        menuAnim.SetBool("isMenuAnim", isMenu);
        menuAnim.SetBool("exIsMenuAnim", true);
    }
}
