using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����
public class Earthquake : Weader
{
    private Camera mainCamera;

    //������ �Ͼ ����(h=��, m=��)
    public int h = 0;
    public int m = 0;

    public float shackingTime = 3f;    //���� ���� �ð�

    private void Awake()
    {
        Weader_Awake();
        mainCamera = Camera.main;
        //���� �ð� ����
        RandomTimeSet();
    }
    private void Update()
    {
        //���� �߻� (StartDebuff�� true�� ��ȯ������ ���� ����)
        if (StartDebuff())
        {
            //ó�� ���� �� ����
            if (!distroyOn)
            {
                distroyOn = StartDebuff();
                //-----------ó�� ����---------------
                WeaderApplication(true);
                //ħ�� ī��Ʈ ����
                World.Instance.islandManager.cost += weaderInfo.closeCost;
                //----------------------------------
            }
            //-----------����Ǵ� ���� ����---------------
            //���� �ð��� ���� �߻��ð��� �ʰ��ϰų� ���� ��� ���� �߻�
            if (((int)timer.GetTime("��") > h) ||
                ((int)timer.GetTime("��") == h && (int)timer.GetTime("��") >= m))
            {
                //���� �߻�
                StartCoroutine(CameraShaking());
            }
            //-------------------------------------------
        }
        //���� �߻��� ���� �� ����
        //���ذ� �߻��ϰ� ���� ���� �� (StartDebuff�� false ��ȯ)
        else if (!StartDebuff())
        {
            if (distroyOn)
            {
                distroyOn = StartDebuff();
                //----------���� ���� �� ����-------------
                //�ൿ ����
                //Player.Instance.Control.isAllowCharMove = true;
                Player.Instance._Debuff.isDebuff.isStun = false;
                WeaderApplication(false);
                //---------------------------------------
            }
        }
    }

    private void OnDestroy()
    {
        //���� �� �ʱ�ȭ�� �Ǳ����� ���� �� �ʱ�ȭ
        if (distroyOn)
        {
            //�ൿ ����
            //Player.Instance.Control.isAllowCharMove = true;
            Player.Instance._Debuff.isDebuff.isStun = false;
        }
    }

    //ī�޶� ����
    public void CameraShake()
    {
        if(!World.Instance.disaster.audio.isPlaying)
        {
            World.Instance.disaster.audio.Play();
        }
        CameraManager.Instance.Impulse();
        //������ üũ
        World.Instance.islandManager.isQuake = true;
        //ī�޶� ����
        float x = UnityEngine.Random.Range(-0.1f, 0.1f);
        float y = UnityEngine.Random.Range(-0.1f, 0.1f);
        mainCamera.transform.position =
            Vector3.Lerp(mainCamera.transform.position,
            new Vector3(mainCamera.transform.position.x + x,
            mainCamera.transform.position.y + y,
            mainCamera.transform.position.z),
            Time.deltaTime);
        //�ൿ�Ұ�
        //Player.Instance.Control.isAllowCharMove = false;
        Player.Instance._Debuff.isDebuff.isStun = true;
    }

    //���� �߻� ���� ����
    public void RandomTimeSet()
    {
        //30~120���� ���� ���� ����
        int randomTime = Random.Range(30, 120);

        //1�ð� �̻��̸� ����ð��� ������ �ð�(��, ��)�� ���� ����
        if (randomTime >= 60)
        {
            h = (int)timer.GetTime("��") + (randomTime / 60);
            m = (int)timer.GetTime("��") + (randomTime % 60);
        }
        //1�ð� �̸��̸� ���� �ð��� ������ �ð�(��)�� ���� ����
        else
        {
            h = (int)timer.GetTime("��");
            m = (int)timer.GetTime("��") + randomTime;
        }
    }
    
    //����
    private IEnumerator CameraShaking()
    {
        //Player.Instance.Control.SetAllowAll(false);
        //���� �߻�
        CameraShake();
        
        yield return new WaitForSeconds(shackingTime);
        //Player.Instance.Control.SetAllowAll(true);
        //Player.Instance._Debuff.isStun = false;
        World.Instance.islandManager.isQuake = false;
        //���� ���� �߻� �ð� ����
        RandomTimeSet();
    }
}
