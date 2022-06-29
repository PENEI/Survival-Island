using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator ani;
    public PlayerControl control;
    public GameObject weapon;
    public bool isGathering;        //ä����
    public bool isAttacking;        //������

    public bool b_attack;           //<-- ���� ���ɿ���

    void Start()
    {
        control = Player.Instance.Control;
        ani = GetComponent<Animator>();
        b_attack = true;
    }

    IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);
        ani.SetBool("IsSleep", false);
        control.SetAllowAll(true);
        /*World.Instance.uiButton.IsSleeping = false;*/
    }

    void Update()
    {
        PlayerMove();
        if (control.m_ActionObj != null)
        {
            /*//����
            if (World.Instance.uiButton.IsSleeping)
            {
                control.SetAllowAll(false);
                ani.SetBool("IsSleep", true);
                StartCoroutine(WaitTime(5f));
            }*/
        }
        if (!isAttacking)
        {

            if (Input.GetMouseButtonDown(0))
            {
                isAttacking = true;
                StartCoroutine(WaitAttack());
                Vector3 scale = weapon.transform.localScale;
                scale.x += 4;
                scale.y += 4;
                scale.z += 4;
                weapon.transform.localScale = scale;
                weapon.GetComponent<BoxCollider>().isTrigger = true;
            }
        }
    }
    IEnumerator WaitAttack()
    {

        yield return new WaitForSeconds(1f);
        //ani.SetBool("", fals);
        
        Vector3 scale = weapon.transform.localScale;
        scale.x -= 4;
        scale.y -= 4;
        scale.z -= 4;
        weapon.transform.localScale = scale;
        weapon.GetComponent<BoxCollider>().isTrigger = false ;

        isAttacking = false;
    }
    IEnumerator WaitDemege()
    {
        yield return new WaitForSeconds(1f);
        ani.SetBool("isDemege", false);
    }

    //�÷��̾� �̵� (�ȱ�, �ٱ�)
    public void PlayerMove()
    {
        if (control.isMove)
        {
            ani.SetBool("IsWalk", true);
            if(control.isAccel)
            {
                ani.SetBool("IsRun", true);
            }
            else if(!control.isAccel)
            {
                ani.SetBool("IsRun", false);
            }            
        }
        else if(!control.isMove)
        {
            ani.SetBool("IsWalk", false);
        }
    }
}
