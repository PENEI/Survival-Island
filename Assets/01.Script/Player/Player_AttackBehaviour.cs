using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AttackBehaviour : StateMachineBehaviour
{
    [HideInInspector]
    public Player_Animation_Conroller ctr;
    public AudioSource audio;
    public AudioClip[] clip;

    // 상태 시작 시
    override public void OnStateEnter(Animator animator, 
        AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance._Animation.state = E_Player_State.Attack;
        // 무기와의 충돌이 가능하도록 활성화
        ctr.weapon.SetActive(true);
        // 플레이어가 무기 장착 시 사운드 변경
        audio.clip = Player.Instance._Animation.ani.GetBool("Weapon") ? 
            clip[1] : clip[0];
        // 오디오 출력
        audio.Play();
    }

    // 상태 끝날 시
    override public void OnStateExit(Animator animator,
        AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 무기 콜라이더 비활성화
        ctr.weapon.SetActive(false);
    }
}
