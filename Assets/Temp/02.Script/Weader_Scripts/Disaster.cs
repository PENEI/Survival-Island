using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Disaster : MonoBehaviour
{
    [Header("[����]")]
    public Weader weader = null;    // ���� ����
    private WorldTime timer;        // ���� �ð�
    public int safetyDay;           // ���ذ� �Ͼ�� �ʴ� ��

    [Header("[���� �߻� Ȯ��]")]       
    public E_Disaster_Type eWeader; // ���õ� ����
    public int sunnyRandom;
    public int earthQuakeRandom;
    public int heatwaveRandom;
    public int rainRandom;
    public int tidalwavesRandom;
    public int typhoonRandom;

    [Header("[ȭ�� ��ȭ�ϴ� ��]")]
    public int eruptionDay;

    [Header("[����]")][Header("[���� ����]")][Space]
    public WeaderInfo sunnyInfo;
    [Header("[����]")]
    [Space]
    public WeaderInfo earthQuakeInfo;
    [Header("[����]")]
    [Space]
    public WeaderInfo heatwaveInfo;
    [Header("[����]")]
    [Space]
    public WeaderInfo rainInfo;
    public GameObject rainParticle;
    [Header("[����]")]
    [Space]
    public WeaderInfo tidalwavesInfo;
    [Header("[��ǳ]")]
    [Space]
    public WeaderInfo typhoonInfo;
    [Header("[��ȭ]")]
    [Space]
    public WeaderInfo eruptionInfo;

    [HideInInspector]
    public AudioSource audio;
    [HideInInspector]
    public Coroutine soundCoroutine;

    public int minSoundDelay;
    public int maxSoundDelay;

    public IEnumerator SoundDelay()
    {
        while (true)
        {
            audio.Play();
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSoundDelay, maxSoundDelay));
        }
    }
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        timer = World.Instance.worldTime;

        // ó���� �⺻ ���� ���� ����
        weader = gameObject.AddComponent<Sunny>();
        weader.weaderInfo = sunnyInfo;
        if (soundCoroutine == null)
        {
            soundCoroutine = StartCoroutine(SoundDelay());
        }
    }

    // ���� �߻� ��¥
    private int beforeDay;

    private void Update()
    {
        // n�� ������ ���� ������ �߻�
        if (timer.GetTime("��") <= safetyDay) { return; }  

        // ���� ���� �Ǹ� ���� ������ ���� �� ����� 
        if (weader != null && beforeDay < (int)timer.GetTime("��"))
        {
            Destroy(gameObject.GetComponent<Weader>());
        }
        // ������ ������ ���ο� ���� ����
        if ((weader == null))
        {
            // ���� �߻� ��¥
            beforeDay = (int)timer.GetTime("��");
            // ���� ����
            RandomCreateDisaster();
        }
    }

    // ���� �߻�/������ �ð� ���� ����
    public WeaderInfo DisasterTimeSet(Weader weader)
    {
        WeaderInfo info = weader.weaderInfo;
        // ���� ��, �� ����
        weader.starth = UnityEngine.Random.Range(0, 20);
        weader.startm = UnityEngine.Random.Range(0, 60);
        // ���� ���� �ð� ���� ����
        info.maintainWeader = UnityEngine.Random.Range
            (info.startTimeMin, info.startTimeMax + 1);

        // ���� ������ ��, �� ����
        // ���� �߻� �ð�+���� �����ð��� 24�̻��̸� 24�� ����
        // �ƴϸ� "���� �߻� �ð�+���� �����ð�" ����
        weader.endh = weader.starth + info.maintainWeader >= 24 ?
            23 : weader.starth + info.maintainWeader;
        weader.endm = weader.startm;

        return info;
    }

    //������ ���� ����
    public E_Disaster_Type RandomWeader()
    {
        // ���� ������ ���̵Ǹ� ��ȭ ���� �ο�
        if (timer.GetTime("��") >= eruptionDay) 
        {
            return E_Disaster_Type.Eruption;
        }

        int total = sunnyRandom + earthQuakeRandom + heatwaveRandom +
            rainRandom + tidalwavesRandom + typhoonRandom;
        int num = UnityEngine.Random.Range(1, total + 1);
        int add = 0;

        //Ȯ�������� ���� ��ȯ
        if      (num < (add += sunnyRandom))        
            {  return E_Disaster_Type.Sunny; }
        else if (num < (add += earthQuakeRandom))   
            {  return E_Disaster_Type.Earthquake; }
        else if (num < (add += heatwaveRandom))     
            {  return E_Disaster_Type.Heatwave; }
        else if (num < (add += rainRandom))         
            {  return E_Disaster_Type.Rain; }
        else if (num < (add += tidalwavesRandom))   
            {  return E_Disaster_Type.Tidalwaves; }
        else if (num < (add += typhoonRandom))      
            {  return E_Disaster_Type.Typhoon; }

        return E_Disaster_Type.None;
    }
    public void CreateDistaster
        (E_Disaster_Type _weader = E_Disaster_Type.None)
    {
        if (_weader != E_Disaster_Type.None)
        {
            eWeader = _weader;
        }

        // ������ �´� ���۷�Ʈ ���� �� �ʱ�ȭ
        if (eWeader == E_Disaster_Type.Sunny)
        {
            weader = gameObject.AddComponent<Sunny>();
            weader.weaderInfo = sunnyInfo;
        }
        // ���� ���� �߻�
        else if (eWeader == E_Disaster_Type.Earthquake)
        {
            weader = gameObject.AddComponent<Earthquake>();
            weader.weaderInfo = earthQuakeInfo;

        }
        else if (eWeader == E_Disaster_Type.Eruption)
        {
            weader = gameObject.AddComponent<Eruption>();
            weader.weaderInfo = eruptionInfo;
            return;
        }
        else if (eWeader == E_Disaster_Type.Heatwave)
        {
            weader = gameObject.AddComponent<Heatwave>();
            weader.weaderInfo = heatwaveInfo;
        }
        else if (eWeader == E_Disaster_Type.Rain)
        {
            weader = gameObject.AddComponent<Rain>();
            weader.weaderInfo = rainInfo;
            weader.GetComponent<Rain>().rainParticle = rainParticle;
        }
        else if (eWeader == E_Disaster_Type.Tidalwaves)
        {
            weader = gameObject.AddComponent<Tidalwaves>();
            weader.weaderInfo = tidalwavesInfo;
        }
        else if (eWeader == E_Disaster_Type.Typhoon)
        {
            weader = gameObject.AddComponent<Typhoon>();
            weader.weaderInfo = typhoonInfo;
        }

        if (weader != null)
        {
            weader.weaderInfo = DisasterTimeSet(weader);
        }
    }

    public void RandomCreateDisaster()
    {
        eWeader = RandomWeader();
        CreateDistaster();
        if (weader != null && eWeader != E_Disaster_Type.Eruption) 
        {
            weader.weaderInfo = DisasterTimeSet(weader);
        }
    }
}
