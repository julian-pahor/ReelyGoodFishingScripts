using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RodAnimationController : MonoBehaviour
{
    private Animator anim;
    public bool casted;
    [Range(0.0f, 1.0f)]
    public float rodBend;
    public CatchArrow catchArrow;
    // Start is called before the first frame update
    void Start()
    {
        catchArrow = FindObjectOfType<CatchArrow>();
        anim = GetComponent<Animator>();
        casted = false;

        GameManager.Instance.onGameLoop += Cast;

        GameManager.Instance.onLoopEnd += Suck;


    }

    // Update is called once per frame
    void Update()
    {
        //if (Mouse.current.leftButton.wasPressedThisFrame)
        //{
        //    if (casted == false)
        //    {
        //        Cast();
        //    }
        //    else
        //    {
        //        Suck();
        //    }    
        //}

        if (casted)
        {
            AnimatorStateInfo animatorinfo = anim.GetCurrentAnimatorStateInfo(0);
            if (animatorinfo.IsName("BendRod"))
            {
                anim.speed = 0;
                rodBend = catchArrow.reelPower / 18;
                anim.Play("BendRod", 0, rodBend);
            }
        }
    }
    public void Cast()
    {
        anim.SetBool("Casted", true);
        casted = true;
    }
    public void Suck()
    {
        anim.speed = 1;
        anim.SetBool("Casted", false);
        casted = false;
    }
}
