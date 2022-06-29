using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldTime : MonoBehaviour
{
    private PlayerStatus playerStatus; //player오브젝트
    [Space]
    [Header("[시간]")]
    public CTime cTime;
    [HideInInspector]
    public Dictionary<string, int[]> dhms;
    [HideInInspector]
    public float Angle360 = 360;       //하루동안 회전할 각도

    [Space]
    [Header("[버튼]")]
    public GameObject sleepWindowObj;   //수면 UI
    public GameObject barObj;              //UI 수면 바
    public GameObject fadeObj;              //페이드인아웃 오브젝트
    private GameObject[] sleepTimeMarkObj;  //수면UI 자는 시간 표시
    private Image[] barBlock;            //UI 수면 바 안의 블록
    public Text hourText;               //선택한 시간 출력 텍스트
    [HideInInspector]
    public int hour;                   //선택한 수면 시간

    [Space]
    [Header("[배경]")]
    public ParticleSystem starPtc;
    [HideInInspector]
    public bool isStar;
    public GameObject lightObj;         //태양

    [Space]
    [Header("[수면 스테이터스 UI]")]
    [Header("[수면 UI 오브젝트]")]
    public GameObject sleepStatusObj;   //수면 스테이터스 UI 오브젝트

    private Image[] statusImage;             //현재 스테이테스        체력, 공복, 수분, 피로
    private Image[] changeStatusImage;       //회복/소모할 스테이터스 이미지
    private float[] beforeStatus;            //변경전의 스테이터스
    private Disaster disaster;

    private void Awake()
    {
        playerStatus = Player.Instance._Status;
        disaster = GetComponent<Disaster>();
        //수면 UI 자는 시간 표시 배열 초기화
        Array.Resize<GameObject>(ref sleepTimeMarkObj, barObj.transform.childCount-1);
        for (int i = 0; i < sleepTimeMarkObj.Length; i++)
        {
            sleepTimeMarkObj[i] = barObj.transform.GetChild(i).GetChild(0).gameObject;
            sleepTimeMarkObj[i].SetActive(false);
        }

        //첫번째 표시만 트루 체크
        sleepTimeMarkObj[0].SetActive(true);

        //수면 바안의 블록들의 갯수를 확인하고 이미지 받아오기
        Array.Resize<Image>(ref barBlock, barObj.transform.childCount - 2);
        for (int i = 0; i < barBlock.Length; i++)
        {
            barBlock[i] = barObj.transform.GetChild(i+1).GetComponent<Image>();
        }
        //barBlock = bar.GetComponentsInChildren<Image>();
        //딕셔너리 초기화 일 시 분을 초단위로 저장
        dhms = new Dictionary<string, int[]>()
        {
            {"일",new int[]{ 86400,1 } },
            {"시",new int[]{ 3600,24} },
            {"분",new int[]{ 60,60} },
            {"초",new int[]{ 1,1} },
        };

        Array.Resize<Image>(ref changeStatusImage, sleepStatusObj.transform.childCount);
        Array.Resize<Image>(ref statusImage, sleepStatusObj.transform.childCount);
        Array.Resize<float>(ref beforeStatus, changeStatusImage.Length);
        for (int i = 0; i < statusImage.Length; i++)
        {
            Transform obj = sleepStatusObj.transform.GetChild(i).GetChild(1);
            changeStatusImage[i] = obj.GetComponent<Image>();
            statusImage[i] = obj.GetChild(0).GetComponent<Image>();
        }

    }

    void Update()
    {
        //수면 UI 활성화상태에 따른 월드 타임 시작/정지
        //ActiveSleepUI();

        //시간 경과
        cTime.sTime += Time.deltaTime * cTime.multiply;

        //오전 오후 설정
        SetAMPM();

        //태양의 자전
        RotationLight();

        //별 활성화
        StarActive();
    }

    //오전 오후 세팅
    public void SetAMPM()
    {
        //12시 이상부턴 오전
        if (GetTime("시") < 12)
        {
            cTime.ampm = "AM";
        }
        //12시 이전이면 오후
        else if (GetTime("시") >= 12)
        {
            cTime.ampm = "PM";
        }
    }
    //시간 증가
    public void AddTime(string s,int time)
    {
        int[] a;
        dhms.TryGetValue(s, out a);
        //일, 시, 분, 초를 초단위로 만들어서 월드 타임 증가
        cTime.sTime += time * a[0];
    }

    //시간 반환
    public float GetTime(string s)
    {
        int[] a;
        dhms.TryGetValue(s, out a);
        //초 일경우 현재 시간을 60나눈 나머지값을 리턴
        if(string.Equals(s,"초"))
        {
            return (cTime.sTime % 60);
        }
        else if(string.Equals(s, "일"))
        {
            return (cTime.sTime / a[0]);
        }
        //일 시 분
        return (cTime.sTime / a[0]) % a[1];
    }

    //시간에 따른 태양의 자전
    public void RotationLight()
    {
        //하루를 초단위로 쪼갠 GetTime("일") * 하루동안 회전할 360도
        lightObj.transform.rotation = Quaternion.Euler(GetTime("일")
            * (float)Angle360, 0, 0);
    }

    //-----버튼-------
    //수면 UI 활성화 상태에 따른  타이머 정지/시작
    public void ActiveSleepUI()
    {
        IsTickTimer(sleepWindowObj.activeSelf ? 0 : 1);
    }
    //수면 시간 설정
    public void SelectTimeClick(int a)
    {
        hour = a;
        hourText.text = (hour).ToString()+"시간 취침하겠습니까?";
        //수면 시간이 바뀔 때마다 회복/소모량 만큼 수면 스테이터스바 이미지 변경 
        SelectTimeClickStatusChange();
        //선택한 시간만큼 색칠
        for (int i = 0; i < hour; i++) 
        {
            barBlock[i].color = new Color(0.8f, 0.8f, 1f);
        }
        //나머지 색은 회색으로
        for (int i = a; i < barBlock.Length; i++) 
        {
            barBlock[i].color = new Color(0.5f, 0.5f, 0.5f);
        }
        //수면UI에 자는 시간 표시를 자는 시간에 맞게 활성화
        for(int i=0;i< sleepTimeMarkObj.Length;i++)
        {
            sleepTimeMarkObj[i].SetActive(false);
        }
        sleepTimeMarkObj[hour].SetActive(true);
    }
    //수면 시간이 바뀔 때마다 회복/소모량 표시 
    public void SelectTimeClickStatusChange()
    {
        statusImage[0].fillAmount = Status.PlayerStatusPercentage(playerStatus.status.hp);
        statusImage[3].fillAmount = Status.PlayerStatusPercentage(playerStatus.status.fatigue);

        //회복하는 스테이터스
        changeStatusImage[0].fillAmount = statusImage[0].fillAmount + 
            GetHourStatus(playerStatus.status.hp) / playerStatus.status.hp.maxStatus;
        changeStatusImage[3].fillAmount = statusImage[3].fillAmount + 
            GetHourStatus(playerStatus.status.fatigue) / playerStatus.status.fatigue.maxStatus;

        changeStatusImage[1].fillAmount = Status.PlayerStatusPercentage(playerStatus.status.hunger);
        changeStatusImage[2].fillAmount = Status.PlayerStatusPercentage(playerStatus.status.hydration);

        //줄어드는 스테이터스
        statusImage[1].fillAmount = changeStatusImage[1].fillAmount - 
            GetHourStatus(playerStatus.status.hunger) / playerStatus.status.hunger.maxStatus;
        statusImage[2].fillAmount = changeStatusImage[2].fillAmount - 
            GetHourStatus(playerStatus.status.hydration) / playerStatus.status.hydration.maxStatus;
    }

    //입력된 시간만큼 월드타임 증가
    public void SelectButtonClick()
    {
        //수면 시간을 선택했을 시 클릭 가능
        if (hour != 0)
        {
            //수면 시 사용되는 스테이터스가 현재 스테이터스 보다 많을 시 함수를 실행하지 않음.
            if (GetHourStatus(playerStatus.status.hydration)> playerStatus.status.hydration.statusValue||
                GetHourStatus(playerStatus.status.hunger)> playerStatus.status.hunger.statusValue)
            {
                return;
            }

            //CancleButtonClick메소드에서 hour이 0으로 초기화됨으로 따로 저장
            int h = hour;
            UIFadeInOut fa = fadeObj.GetComponent<UIFadeInOut>();

            //UIFadeInOut에 있는 fadeSpeed 시간 만큼 대기후에 hour시간 만큼 증가 시킴 
            StartCoroutine(IEAddTimeDelay(fa.fadeSpeed, h));
          
            //UI닫기
            CancleButtonClick();
            //페이드 인 아웃 시작
            fadeObj.SetActive(true);
        }
    }

    //수면 시 회복하는 스테이터스 값
    public float GetHourStatus(SingleStatus status)
    {
        return (status.sleepRecoveryValue * hour);
    }

    //대기 후 시간 스테이터스 증가
    public IEnumerator IEAddTimeDelay(float delayTime, int h)
    {
        yield return new WaitForSeconds(delayTime);
        //수면 시간 더하기 전의 시간 + 수면 시간이 0시(다음날로 넘어감으로)이상이면 날씨 재설정
        if (((int)GetTime("시")) + h > 23)
        {
            disaster.RandomCreateDisaster();
        }
        //시간증가
        AddTime("시", h);
        //스테이터스 증가
        SleepRecoveryStatus(h);
    }
    //취소 버튼
    public void CancleButtonClick()
    {
        hour = 0;
        hourText.text = "";
        for (int i = 0; i < barBlock.Length; i++)
        {
            barBlock[i].color = new Color(0.5f, 0.5f, 0.5f);
        }
        //수면UI에 자는 시간을 0번째를 제외한 모든 표시를 비활성화
        for (int i = 0; i < sleepTimeMarkObj.Length; i++)
        {
            sleepTimeMarkObj[i].SetActive(false);
        }
        sleepTimeMarkObj[0].SetActive(true);
        //수면 UI 비활성화
        UIManager.Instance.SetActiveUI(sleepWindowObj, false);
        //sleepWindowObj.SetActive(false);
    }
    //수면 시 스테이터스 변화
    public void SleepRecoveryStatus(int h)
    {
        //회복 : 체력, 피로
        playerStatus.StatusRecovery(playerStatus.status.hp,
                            playerStatus.status.hp.sleepRecoveryValue * h);
        playerStatus.StatusRecovery(playerStatus.status.fatigue,
                            playerStatus.status.fatigue.sleepRecoveryValue * h);
        //소모 : 공복, 수분
        playerStatus.StatusRecovery(playerStatus.status.hunger,
                            -(playerStatus.status.hunger.sleepRecoveryValue * h));
        playerStatus.StatusRecovery(playerStatus.status.hydration,
                            -(playerStatus.status.hydration.sleepRecoveryValue * h));
    }


    //----별------
    public void StarActive()
    {
        //밤에만 별이 보이도록 20~23시, 0~5시에 별 시작
        if (!isStar && (GetTime("시") >= 20 || GetTime("시") < 6))
        {
            starPtc.Play();
            isStar = true;
        }
        //아침에는 별이 보이지 않도록 6~19시까지 별 멈춤
        else if (isStar && (GetTime("시") >= 6 && GetTime("시") < 20)) 
        {
            starPtc.Stop();
            isStar = false;
        }
    }

    //타이머 시작/정지
    public void IsTickTimer(int i)
    {
        //i가 0이면 정지 1이면 시작
        Time.timeScale = i;
        //수면 UI가 활성화 시 수면 UI 이미지 초기화
        if (i == 1)
        {
            //수면 시간이 바뀔 때마다 회복/소모량 만큼 수면 스테이터스바 이미지 변경 
            SelectTimeClickStatusChange();
            changeStatusImage[0].fillAmount = 0f;
            changeStatusImage[3].fillAmount = 0f;
            statusImage[1].fillAmount = Status.PlayerStatusPercentage(playerStatus.status.hunger);
            statusImage[2].fillAmount = Status.PlayerStatusPercentage(playerStatus.status.hydration);
        }
    }
}
