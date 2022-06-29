using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldTime : MonoBehaviour
{
    private PlayerStatus playerStatus; //player������Ʈ
    [Space]
    [Header("[�ð�]")]
    public CTime cTime;
    [HideInInspector]
    public Dictionary<string, int[]> dhms;
    [HideInInspector]
    public float Angle360 = 360;       //�Ϸ絿�� ȸ���� ����

    [Space]
    [Header("[��ư]")]
    public GameObject sleepWindowObj;   //���� UI
    public GameObject barObj;              //UI ���� ��
    public GameObject fadeObj;              //���̵��ξƿ� ������Ʈ
    private GameObject[] sleepTimeMarkObj;  //����UI �ڴ� �ð� ǥ��
    private Image[] barBlock;            //UI ���� �� ���� ���
    public Text hourText;               //������ �ð� ��� �ؽ�Ʈ
    [HideInInspector]
    public int hour;                   //������ ���� �ð�

    [Space]
    [Header("[���]")]
    public ParticleSystem starPtc;
    [HideInInspector]
    public bool isStar;
    public GameObject lightObj;         //�¾�

    [Space]
    [Header("[���� �������ͽ� UI]")]
    [Header("[���� UI ������Ʈ]")]
    public GameObject sleepStatusObj;   //���� �������ͽ� UI ������Ʈ

    private Image[] statusImage;             //���� �������׽�        ü��, ����, ����, �Ƿ�
    private Image[] changeStatusImage;       //ȸ��/�Ҹ��� �������ͽ� �̹���
    private float[] beforeStatus;            //�������� �������ͽ�
    private Disaster disaster;

    private void Awake()
    {
        playerStatus = Player.Instance._Status;
        disaster = GetComponent<Disaster>();
        //���� UI �ڴ� �ð� ǥ�� �迭 �ʱ�ȭ
        Array.Resize<GameObject>(ref sleepTimeMarkObj, barObj.transform.childCount-1);
        for (int i = 0; i < sleepTimeMarkObj.Length; i++)
        {
            sleepTimeMarkObj[i] = barObj.transform.GetChild(i).GetChild(0).gameObject;
            sleepTimeMarkObj[i].SetActive(false);
        }

        //ù��° ǥ�ø� Ʈ�� üũ
        sleepTimeMarkObj[0].SetActive(true);

        //���� �پ��� ��ϵ��� ������ Ȯ���ϰ� �̹��� �޾ƿ���
        Array.Resize<Image>(ref barBlock, barObj.transform.childCount - 2);
        for (int i = 0; i < barBlock.Length; i++)
        {
            barBlock[i] = barObj.transform.GetChild(i+1).GetComponent<Image>();
        }
        //barBlock = bar.GetComponentsInChildren<Image>();
        //��ųʸ� �ʱ�ȭ �� �� ���� �ʴ����� ����
        dhms = new Dictionary<string, int[]>()
        {
            {"��",new int[]{ 86400,1 } },
            {"��",new int[]{ 3600,24} },
            {"��",new int[]{ 60,60} },
            {"��",new int[]{ 1,1} },
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
        //���� UI Ȱ��ȭ���¿� ���� ���� Ÿ�� ����/����
        //ActiveSleepUI();

        //�ð� ���
        cTime.sTime += Time.deltaTime * cTime.multiply;

        //���� ���� ����
        SetAMPM();

        //�¾��� ����
        RotationLight();

        //�� Ȱ��ȭ
        StarActive();
    }

    //���� ���� ����
    public void SetAMPM()
    {
        //12�� �̻���� ����
        if (GetTime("��") < 12)
        {
            cTime.ampm = "AM";
        }
        //12�� �����̸� ����
        else if (GetTime("��") >= 12)
        {
            cTime.ampm = "PM";
        }
    }
    //�ð� ����
    public void AddTime(string s,int time)
    {
        int[] a;
        dhms.TryGetValue(s, out a);
        //��, ��, ��, �ʸ� �ʴ����� ���� ���� Ÿ�� ����
        cTime.sTime += time * a[0];
    }

    //�ð� ��ȯ
    public float GetTime(string s)
    {
        int[] a;
        dhms.TryGetValue(s, out a);
        //�� �ϰ�� ���� �ð��� 60���� ���������� ����
        if(string.Equals(s,"��"))
        {
            return (cTime.sTime % 60);
        }
        else if(string.Equals(s, "��"))
        {
            return (cTime.sTime / a[0]);
        }
        //�� �� ��
        return (cTime.sTime / a[0]) % a[1];
    }

    //�ð��� ���� �¾��� ����
    public void RotationLight()
    {
        //�Ϸ縦 �ʴ����� �ɰ� GetTime("��") * �Ϸ絿�� ȸ���� 360��
        lightObj.transform.rotation = Quaternion.Euler(GetTime("��")
            * (float)Angle360, 0, 0);
    }

    //-----��ư-------
    //���� UI Ȱ��ȭ ���¿� ����  Ÿ�̸� ����/����
    public void ActiveSleepUI()
    {
        IsTickTimer(sleepWindowObj.activeSelf ? 0 : 1);
    }
    //���� �ð� ����
    public void SelectTimeClick(int a)
    {
        hour = a;
        hourText.text = (hour).ToString()+"�ð� ��ħ�ϰڽ��ϱ�?";
        //���� �ð��� �ٲ� ������ ȸ��/�Ҹ� ��ŭ ���� �������ͽ��� �̹��� ���� 
        SelectTimeClickStatusChange();
        //������ �ð���ŭ ��ĥ
        for (int i = 0; i < hour; i++) 
        {
            barBlock[i].color = new Color(0.8f, 0.8f, 1f);
        }
        //������ ���� ȸ������
        for (int i = a; i < barBlock.Length; i++) 
        {
            barBlock[i].color = new Color(0.5f, 0.5f, 0.5f);
        }
        //����UI�� �ڴ� �ð� ǥ�ø� �ڴ� �ð��� �°� Ȱ��ȭ
        for(int i=0;i< sleepTimeMarkObj.Length;i++)
        {
            sleepTimeMarkObj[i].SetActive(false);
        }
        sleepTimeMarkObj[hour].SetActive(true);
    }
    //���� �ð��� �ٲ� ������ ȸ��/�Ҹ� ǥ�� 
    public void SelectTimeClickStatusChange()
    {
        statusImage[0].fillAmount = Status.PlayerStatusPercentage(playerStatus.status.hp);
        statusImage[3].fillAmount = Status.PlayerStatusPercentage(playerStatus.status.fatigue);

        //ȸ���ϴ� �������ͽ�
        changeStatusImage[0].fillAmount = statusImage[0].fillAmount + 
            GetHourStatus(playerStatus.status.hp) / playerStatus.status.hp.maxStatus;
        changeStatusImage[3].fillAmount = statusImage[3].fillAmount + 
            GetHourStatus(playerStatus.status.fatigue) / playerStatus.status.fatigue.maxStatus;

        changeStatusImage[1].fillAmount = Status.PlayerStatusPercentage(playerStatus.status.hunger);
        changeStatusImage[2].fillAmount = Status.PlayerStatusPercentage(playerStatus.status.hydration);

        //�پ��� �������ͽ�
        statusImage[1].fillAmount = changeStatusImage[1].fillAmount - 
            GetHourStatus(playerStatus.status.hunger) / playerStatus.status.hunger.maxStatus;
        statusImage[2].fillAmount = changeStatusImage[2].fillAmount - 
            GetHourStatus(playerStatus.status.hydration) / playerStatus.status.hydration.maxStatus;
    }

    //�Էµ� �ð���ŭ ����Ÿ�� ����
    public void SelectButtonClick()
    {
        //���� �ð��� �������� �� Ŭ�� ����
        if (hour != 0)
        {
            //���� �� ���Ǵ� �������ͽ��� ���� �������ͽ� ���� ���� �� �Լ��� �������� ����.
            if (GetHourStatus(playerStatus.status.hydration)> playerStatus.status.hydration.statusValue||
                GetHourStatus(playerStatus.status.hunger)> playerStatus.status.hunger.statusValue)
            {
                return;
            }

            //CancleButtonClick�޼ҵ忡�� hour�� 0���� �ʱ�ȭ������ ���� ����
            int h = hour;
            UIFadeInOut fa = fadeObj.GetComponent<UIFadeInOut>();

            //UIFadeInOut�� �ִ� fadeSpeed �ð� ��ŭ ����Ŀ� hour�ð� ��ŭ ���� ��Ŵ 
            StartCoroutine(IEAddTimeDelay(fa.fadeSpeed, h));
          
            //UI�ݱ�
            CancleButtonClick();
            //���̵� �� �ƿ� ����
            fadeObj.SetActive(true);
        }
    }

    //���� �� ȸ���ϴ� �������ͽ� ��
    public float GetHourStatus(SingleStatus status)
    {
        return (status.sleepRecoveryValue * hour);
    }

    //��� �� �ð� �������ͽ� ����
    public IEnumerator IEAddTimeDelay(float delayTime, int h)
    {
        yield return new WaitForSeconds(delayTime);
        //���� �ð� ���ϱ� ���� �ð� + ���� �ð��� 0��(�������� �Ѿ����)�̻��̸� ���� �缳��
        if (((int)GetTime("��")) + h > 23)
        {
            disaster.RandomCreateDisaster();
        }
        //�ð�����
        AddTime("��", h);
        //�������ͽ� ����
        SleepRecoveryStatus(h);
    }
    //��� ��ư
    public void CancleButtonClick()
    {
        hour = 0;
        hourText.text = "";
        for (int i = 0; i < barBlock.Length; i++)
        {
            barBlock[i].color = new Color(0.5f, 0.5f, 0.5f);
        }
        //����UI�� �ڴ� �ð��� 0��°�� ������ ��� ǥ�ø� ��Ȱ��ȭ
        for (int i = 0; i < sleepTimeMarkObj.Length; i++)
        {
            sleepTimeMarkObj[i].SetActive(false);
        }
        sleepTimeMarkObj[0].SetActive(true);
        //���� UI ��Ȱ��ȭ
        UIManager.Instance.SetActiveUI(sleepWindowObj, false);
        //sleepWindowObj.SetActive(false);
    }
    //���� �� �������ͽ� ��ȭ
    public void SleepRecoveryStatus(int h)
    {
        //ȸ�� : ü��, �Ƿ�
        playerStatus.StatusRecovery(playerStatus.status.hp,
                            playerStatus.status.hp.sleepRecoveryValue * h);
        playerStatus.StatusRecovery(playerStatus.status.fatigue,
                            playerStatus.status.fatigue.sleepRecoveryValue * h);
        //�Ҹ� : ����, ����
        playerStatus.StatusRecovery(playerStatus.status.hunger,
                            -(playerStatus.status.hunger.sleepRecoveryValue * h));
        playerStatus.StatusRecovery(playerStatus.status.hydration,
                            -(playerStatus.status.hydration.sleepRecoveryValue * h));
    }


    //----��------
    public void StarActive()
    {
        //�㿡�� ���� ���̵��� 20~23��, 0~5�ÿ� �� ����
        if (!isStar && (GetTime("��") >= 20 || GetTime("��") < 6))
        {
            starPtc.Play();
            isStar = true;
        }
        //��ħ���� ���� ������ �ʵ��� 6~19�ñ��� �� ����
        else if (isStar && (GetTime("��") >= 6 && GetTime("��") < 20)) 
        {
            starPtc.Stop();
            isStar = false;
        }
    }

    //Ÿ�̸� ����/����
    public void IsTickTimer(int i)
    {
        //i�� 0�̸� ���� 1�̸� ����
        Time.timeScale = i;
        //���� UI�� Ȱ��ȭ �� ���� UI �̹��� �ʱ�ȭ
        if (i == 1)
        {
            //���� �ð��� �ٲ� ������ ȸ��/�Ҹ� ��ŭ ���� �������ͽ��� �̹��� ���� 
            SelectTimeClickStatusChange();
            changeStatusImage[0].fillAmount = 0f;
            changeStatusImage[3].fillAmount = 0f;
            statusImage[1].fillAmount = Status.PlayerStatusPercentage(playerStatus.status.hunger);
            statusImage[2].fillAmount = Status.PlayerStatusPercentage(playerStatus.status.hydration);
        }
    }
}
