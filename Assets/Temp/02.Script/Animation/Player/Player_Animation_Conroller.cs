using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_Animation_Conroller : MonoBehaviour
{
    public E_Player_State state;
    public Animator ani;
    public float idle_Animation_StartTime;
    public float idle_Animation_delayTime;

    public float damageCriticalCut;

    public bool hiting;
    public GameObject weapon;
    public float hitDelay;

    private AudioSource _audioSource;
    public AudioClip[] attackSound;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        ani.GetBehaviour<Player_CtrDamage_Behaviour>().rigid = transform.GetComponentInParent<Rigidbody>();
        ani.GetBehaviour<Player_AttackBehaviour>().ctr = this;

        _audioSource = Player.Instance.GetComponent<AudioSource>();
        ani.GetBehaviour<Player_AttackBehaviour>().audio = _audioSource;
        Array.Resize<AudioClip>(ref ani.GetBehaviour<Player_AttackBehaviour>().clip, attackSound.Length);
        for (int i = 0; i < attackSound.Length; i++)
        {
            ani.GetBehaviour<Player_AttackBehaviour>().clip[i] = attackSound[i];
        }
    }

    public void IdleAnimation()
    {
        ani.SetInteger("Idle_State", UnityEngine.Random.Range(0, 3));
        ani.SetTrigger("IdleTrigger");
    }

    private void Update()
    {
        //공격
        if (Input.GetMouseButtonDown(0) )
        {
            //무기 애니메이션
            Player.Instance._Animation.ani.SetBool
                ("Weapon", UIManager.Instance.equipPanel.PlayerTool 
                == E_UseTool.Knife);
            
            CancelInvoke("IdleAnimation");
            ani.SetTrigger("AttackTrigger");
        }
        //대기
        else if (!Player.Instance.Control.isMove)
        {
            if (state != E_Player_State.Idle)
            {
                InvokeRepeating("IdleAnimation", 
                    idle_Animation_StartTime, idle_Animation_delayTime);
                ani.SetFloat("Horizontal", 0);
            }
        }
        //이동
        else if (Player.Instance.Control.isMove)
        {
            CancelInvoke("IdleAnimation");
            ani.SetFloat("Horizontal",
                Player.Instance.Control.isMove ? 
                Player.Instance.Control.GetSpeed() /
                Player.Instance.Control.MaxMoveSpeed : 0);

            state = E_Player_State.Move;
        }
    }

    //피격
    public IEnumerator PlayerHitAnimation(float damage)
    {
        state = E_Player_State.Hit;
        CancelInvoke("IdleAnimation");

        //피격 시 damage만큼의 체력 감소
        Player.Instance._Status.status.hp.statusValue -=
            damage - Player.Instance._Status.status.defensePower > 0 ?
            (damage - Player.Instance._Status.status.defensePower):0;
 
        hiting = true;      //채집 중 피격 체크
        
        #region Hit 애니메이션이 한번만 실행되게하기
        if (!ani.GetBool("HitDelay"))
        {
            ani.SetBool("HitKind", damageCriticalCut <= damage);
            ani.SetTrigger("Hit");
            //hitDelay 동안 피격 애니메이션 출력 안함
            StartCoroutine(IEHitAniDelay());
        }
        yield break;
        #endregion
    }
    IEnumerator IEHitAniDelay()
    {
        yield return new WaitForSeconds(hitDelay);
        ani.SetBool("HitDelay", false);
    }

    //사망
    public void PlayerDead()
    {
        if(state != E_Player_State.Dead)
        {
            Debug.Log("사망");
            state = E_Player_State.Dead;
            CancelInvoke("IdleAnimation");
            Player.Instance.Control.SetAllowAll(false);
            ani.SetTrigger("Dead");
        }
    }

}
