using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{ 
    private WorldTime worldTime;    //���� �ð�
    public Status status;           //����,����,�Ƿ�,ü�� �������ͽ�
    private bool playerDead = false;    //�÷��̾� ��� ����
    [HideInInspector]
    public bool playerHit = false;  //�÷��̾� �ǰ� ����
    private Animator ani;
    private AudioSource audio;      // �÷��̾� �����
    private float audioVolume;      // ���� ����� ���� ��
    [Range(0, 1)]
    public float activeAudioVolume; // UIȰ��ȭ �� ����� ���� ��
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

        //���� �������� �������ͽ� ����
        StartCoroutine(StartReductionStatus(StatusDelayTime(status.hydration), status.hydration.cycleTime));
        StartCoroutine(StartReductionStatus(StatusDelayTime(status.hunger), status.hunger.cycleTime));
        StartCoroutine(StartReductionStatus(StatusDelayTime(status.fatigue), status.fatigue.cycleTime));
        StartCoroutine(StartReductionStatus(StatusDelayTime(status.hp), status.hp.cycleTime));

        // �÷��̾� ���� ��ġ ���� ����
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

        //�������ͽ��� 0���ϸ� ���� ������ ���Ұ� ����
        ZeroStatus(status.hydration, ref status.hunger.minusValue[0]);
        ZeroStatus(status.hunger, ref status.fatigue.minusValue[0]);
        ZeroStatus(status.fatigue, ref status.hp.minusValue[0]);

        //�������ͽ� ����ġ
        StatusLimit(status.hydration);
        StatusLimit(status.hunger);
        StatusLimit(status.fatigue);
        StatusLimit(status.hp);
    }
 
    //�������ͽ��� 0���� �۰ų� �ִ�ġ ���� Ŀ���� �ʰ� ����
    public void StatusLimit(SingleStatus status)
    {
        if (status.statusValue < 0)
            status.statusValue = 0;
        if (status.statusValue > 100)
            status.statusValue = status.maxStatus;
    }

    //�������ͽ��� 0�� ���� ��
    public void ZeroStatus(SingleStatus status, ref float minusvalue)
    {
        //�������ͽ��� 0���� �۾������� �ѹ��� ����
        if (!status.isZeroStatus && status.statusValue <= 0)
        {
            status.isZeroStatus = true;
            //���� �������ͽ��� ����ġ
            minusvalue += 1;
            return;
        }
        //�������ͽ��� 0���� Ŭ�� �ѹ��� ����
        if (status.isZeroStatus && status.statusValue > 0)
        {
            status.isZeroStatus = false;
            minusvalue -= 1;
            return;
        }
    }

    //�������ͽ��� �ֱ⿡ �°� ����
    IEnumerator StartReductionStatus(IEnumerator _action, int _reductionTime)
    {
        yield return new WaitUntil(() => (int)worldTime.GetTime("��") >= _reductionTime);
        StartCoroutine(_action);
    }

    //�������ͽ� ����
    IEnumerator StatusDelayTime(SingleStatus status)
    {
        //�ݺ�
        while (true)
        {
            //���� �ð�(��)�� ���� �ֱ�� ������ yield return
            yield return new WaitUntil(() => (((int)worldTime.GetTime("��") % status.cycleTime) == 0));
            //�������ͽ��� 0���� Ŭ���� ����
            if (status.statusValue > 0)
            {
                status.statusValue -= (status.minusValue[0] + status.minusValue[1]);
            }
            yield return new WaitUntil(() => (((int)worldTime.GetTime("��") % status.cycleTime) != 0));
        }
    }

    //�������ͽ� ȸ��
    public void StatusRecovery(SingleStatus status, float value)
    {
        status.statusValue += value;
    }

    //�������ͽ� ���� �� ����/����
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


    //�ǰ� ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWeapon") && status.hp.statusValue>0)
        {
            //�浹�� ������ ���ݷ��� ���� �ڷ�ƾ ����
            StartCoroutine(Player.Instance._Animation.PlayerHitAnimation(other.GetComponentInParent<EnemyInfo>().attackPower));
            //�ǰ�(���� ������)�� ������ �÷��̾ �з��� ����
            ani.GetBehaviour<Player_CtrDamage_Behaviour>().dir = (other.transform.position - transform.position) * -1;
        }
    }
}
