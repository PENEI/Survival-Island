using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBehaviour : StateMachineBehaviour
{
    public MonsterAnimation monAni;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*//¼±°øÇü
        if (!monAni.info.type)
        {
            if (monAni.GetDistance(Player.Instance.transform.position, monAni.transform.position) > Mathf.Pow(monAni.info.searchDis, 2))
            {
                monAni.RandomMotion();
            }
            else
            {
                monAni.ani.SetInteger("State", 4);
            }
        }
        else
        {
            if (!monAni.info.isSerching2)
            {
                monAni.RandomMotion();
            }
            else
            {
                monAni.ani.SetInteger("State", 4);
            }
        }*/
    }

}
