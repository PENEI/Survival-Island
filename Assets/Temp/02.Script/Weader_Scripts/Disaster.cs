using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Disaster : MonoBehaviour
{
    [Header("[날씨]")]
    public Weader weader = null;    // 현재 날씨
    private WorldTime timer;        // 월드 시간
    public int safetyDay;           // 재해가 일어나지 않는 날

    [Header("[날씨 발생 확률]")]       
    public E_Disaster_Type eWeader; // 선택된 날씨
    public int sunnyRandom;
    public int earthQuakeRandom;
    public int heatwaveRandom;
    public int rainRandom;
    public int tidalwavesRandom;
    public int typhoonRandom;

    [Header("[화산 분화하는 날]")]
    public int eruptionDay;

    [Header("[맑음]")][Header("[재해 정보]")][Space]
    public WeaderInfo sunnyInfo;
    [Header("[지진]")]
    [Space]
    public WeaderInfo earthQuakeInfo;
    [Header("[폭염]")]
    [Space]
    public WeaderInfo heatwaveInfo;
    [Header("[폭우]")]
    [Space]
    public WeaderInfo rainInfo;
    public GameObject rainParticle;
    [Header("[해일]")]
    [Space]
    public WeaderInfo tidalwavesInfo;
    [Header("[태풍]")]
    [Space]
    public WeaderInfo typhoonInfo;
    [Header("[분화]")]
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

        // 처음의 기본 날씨 맑음 설정
        weader = gameObject.AddComponent<Sunny>();
        weader.weaderInfo = sunnyInfo;
        if (soundCoroutine == null)
        {
            soundCoroutine = StartCoroutine(SoundDelay());
        }
    }

    // 재해 발생 날짜
    private int beforeDay;

    private void Update()
    {
        // n일 전에는 맑음 날씨만 발생
        if (timer.GetTime("일") <= safetyDay) { return; }  

        // 다음 날이 되면 현재 날씨가 있을 시 지우기 
        if (weader != null && beforeDay < (int)timer.GetTime("일"))
        {
            Destroy(gameObject.GetComponent<Weader>());
        }
        // 날씨가 없으면 새로운 날씨 생성
        if ((weader == null))
        {
            // 재해 발생 날짜
            beforeDay = (int)timer.GetTime("일");
            // 날씨 생성
            RandomCreateDisaster();
        }
    }

    // 재해 발생/끝나는 시간 랜덤 설정
    public WeaderInfo DisasterTimeSet(Weader weader)
    {
        WeaderInfo info = weader.weaderInfo;
        // 랜덤 시, 분 설정
        weader.starth = UnityEngine.Random.Range(0, 20);
        weader.startm = UnityEngine.Random.Range(0, 60);
        // 재해 유지 시간 랜덤 설정
        info.maintainWeader = UnityEngine.Random.Range
            (info.startTimeMin, info.startTimeMax + 1);

        // 재해 끝나는 시, 분 설정
        // 재해 발생 시간+재해 유지시간이 24이상이면 24로 저장
        // 아니면 "재해 발생 시간+재해 유지시간" 저장
        weader.endh = weader.starth + info.maintainWeader >= 24 ?
            23 : weader.starth + info.maintainWeader;
        weader.endm = weader.startm;

        return info;
    }

    //랜덤한 날씨 설정
    public E_Disaster_Type RandomWeader()
    {
        // 게임 마지막 날이되면 분화 날씨 부여
        if (timer.GetTime("일") >= eruptionDay) 
        {
            return E_Disaster_Type.Eruption;
        }

        int total = sunnyRandom + earthQuakeRandom + heatwaveRandom +
            rainRandom + tidalwavesRandom + typhoonRandom;
        int num = UnityEngine.Random.Range(1, total + 1);
        int add = 0;

        //확률에따라 날씨 반환
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

        // 날씨에 맞는 컴퍼런트 생성 및 초기화
        if (eWeader == E_Disaster_Type.Sunny)
        {
            weader = gameObject.AddComponent<Sunny>();
            weader.weaderInfo = sunnyInfo;
        }
        // 지진 재해 발생
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
