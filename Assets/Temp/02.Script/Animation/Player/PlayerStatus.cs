using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{ 
    private WorldTime worldTime;    //월드 시간
    public Status status;           //수분,공복,피로,체력 스테이터스
    private bool playerDead = false;    //플레이어 사망 여부
    [HideInInspector]
    public bool playerHit = false;  //플레이어 피격 상태
    private Animator ani;
    private AudioSource audio;      // 플레이어 오디오
    private float audioVolume;      // 기존 오디오 볼륨 값
    [Range(0, 1)]
    public float activeAudioVolume; // UI활성화 시 오디오 볼륨 값
    public Transform[] startPos;

    private void Awake()
    {
        worldTime = World.Instance.worldTime;
        ani = Player.Instance._Animation.ani;
        
    }

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        audioVolume = audio.volume;

        status.attackPower = status.normalAttackPower;

        //일정 간격으로 스테이터스 감소
        StartCoroutine(StartReductionStatus(StatusDelayTime(status.hydration), status.hydration.cycleTime));
        StartCoroutine(StartReductionStatus(StatusDelayTime(status.hunger), status.hunger.cycleTime));
        StartCoroutine(StartReductionStatus(StatusDelayTime(status.fatigue), status.fatigue.cycleTime));
        StartCoroutine(StartReductionStatus(StatusDelayTime(status.hp), status.hp.cycleTime));

        // 플레이어 시작 위치 랜덤 설정
        if (startPos.Length > 0 && !FileData.Instance.isSaveLoad) 
        {
            transform.position = startPos[UnityEngine.Random.Range(0, startPos.Length)].position;
        }
    }
    private void Update()
    {
        audio.volume = UIManager.Instance.InfoPanel.gameObject.activeSelf ? activeAudioVolume : audioVolume;

        if (status.hp.statusValue == 0) 
        {
            Player.Instance._Animation.PlayerDead();
            return;
        }

        //스테이터스가 0이하면 다음 스텟의 감소값 증가
        ZeroStatus(status.hydration, ref status.hunger.minusValue[0]);
        ZeroStatus(status.hunger, ref status.fatigue.minusValue[0]);
        ZeroStatus(status.fatigue, ref status.hp.minusValue[0]);

        //스테이터스 상한치
        StatusLimit(status.hydration);
        StatusLimit(status.hunger);
        StatusLimit(status.fatigue);
        StatusLimit(status.hp);
    }
 
    //스테이터스가 0보다 작거나 최대치 보다 커지지 않게 제한
    public void StatusLimit(SingleStatus status)
    {
        if (status.statusValue < 0)
            status.statusValue = 0;
        if (status.statusValue > 100)
            status.statusValue = status.maxStatus;
    }

    //스테이터스가 0이 됬을 시
    public void ZeroStatus(SingleStatus status, ref float minusvalue)
    {
        //스테이터스가 0보다 작아졌을떄 한번만 실행
        if (!status.isZeroStatus && status.statusValue <= 0)
        {
            status.isZeroStatus = true;
            //다음 스테이터스의 감소치
            minusvalue += 1;
            return;
        }
        //스테이터스가 0보다 클때 한번만 실행
        if (status.isZeroStatus && status.statusValue > 0)
        {
            status.isZeroStatus = false;
            minusvalue -= 1;
            return;
        }
    }

    //스테이터스를 주기에 맞게 실행
    IEnumerator StartReductionStatus(IEnumerator _action, int _reductionTime)
    {
        yield return new WaitUntil(() => (int)worldTime.GetTime("분") >= _reductionTime);
        StartCoroutine(_action);
    }

    //스테이터스 감소
    IEnumerator StatusDelayTime(SingleStatus status)
    {
        //반복
        while (true)
        {
            //월드 시간(분)이 감소 주기와 같을때 yield return
            yield return new WaitUntil(() => (((int)worldTime.GetTime("분") % status.cycleTime) == 0));
            //스테이터스가 0보다 클때만 감소
            if (status.statusValue > 0)
            {
                status.statusValue -= (status.minusValue[0] + status.minusValue[1]);
            }
            yield return new WaitUntil(() => (((int)worldTime.GetTime("분") % status.cycleTime) != 0));
        }
    }

    //스테이터스 회복
    public void StatusRecovery(SingleStatus status, float value)
    {
        status.statusValue += value;
    }

    //스테이터스 증감 값 증가/감소
    public void Hydration_Status_InDecrease(float value,bool tf = true)
    {
        Player.Instance._Status.status.hydration.minusValue[1] += tf ? value : -value;
    }
    public void Hunger_Status_InDecrease(float value, bool tf = true)
    {
        Player.Instance._Status.status.hunger.minusValue[1] += tf ? value : -value;
    }
    public void Fatigue_Status_InDecrease(float value, bool tf = true)
    {
        Player.Instance._Status.status.fatigue.minusValue[1] += tf ? value : -value;
    }
    public void HP_Status_InDecrease(float value, bool tf = true)
    {
        Player.Instance._Status.status.hp.minusValue[1] += tf ? value : -value;
    }


    //피격 시
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWeapon") && status.hp.statusValue>0)
        {
            //충돌한 몬스터의 공격력을 갖고 코루틴 시작
            StartCoroutine(Player.Instance._Animation.PlayerHitAnimation(other.GetComponentInParent<EnemyInfo>().attackPower));
            //피격(강한 데미지)를 받을때 플레이어가 밀려날 방향
            ani.GetBehaviour<Player_CtrDamage_Behaviour>().dir = (other.transform.position - transform.position) * -1;
        }
    }
}
