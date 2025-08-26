using UnityEngine;

public class AnimationDirector : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void Execut()
    {
        anim.SetBool("isMenuAnim", true);
    }
}
