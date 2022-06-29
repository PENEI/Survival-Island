using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AttackBehaviour : StateMachineBehaviour
{
    [HideInInspector]
    public Player_Animation_Conroller ctr;
    public AudioSource audio;
    public AudioClip[] clip;

    // ���� ���� ��
    override public void OnStateEnter(Animator animator, 
        AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance._Animation.state = E_Player_State.Attack;
        // ������� �浹�� �����ϵ��� Ȱ��ȭ
        ctr.weapon.SetActive(true);
        // �÷��̾ ���� ���� �� ���� ����
        audio.clip = Player.Instance._Animation.ani.GetBool("Weapon") ? 
            clip[1] : clip[0];
        // ����� ���
        audio.Play();
    }

    // ���� ���� ��
    override public void OnStateExit(Animator animator,
        AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���� �ݶ��̴� ��Ȱ��ȭ
        ctr.weapon.SetActive(false);
    }
}
