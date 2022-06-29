using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontMove_Behaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!World.Instance.islandManager.isQuake)
        {
            Player.Instance.Control.isAllowCharMove = false;
            Player.Instance.Control.isAllowInteraction = false;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!World.Instance.islandManager.isQuake)
        {
            Player.Instance.Control.isAllowCharMove = true;
            Player.Instance.Control.isAllowInteraction = true;
        }
    }
}
