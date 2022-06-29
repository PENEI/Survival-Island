using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CtrDamage_Behaviour : StateMachineBehaviour
{
    public float pushPower;
    [HideInInspector]
    public Rigidbody rigid;
    [HideInInspector]
    public Vector3 dir;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rigid.AddForce(dir * pushPower,ForceMode.Acceleration);
    }
}
