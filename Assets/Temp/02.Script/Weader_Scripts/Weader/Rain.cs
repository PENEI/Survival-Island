using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����
public class Rain : Weader
{
    public GameObject rainParticle;

    private void Awake()
    {
        Weader_Awake();
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
                rainParticle.SetActive(true);
                //----------------------------------
            }
            //-----------����Ǵ� ���� ����---------------
            Debug.Log("\n���� �߻� ��!");
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
                rainParticle.SetActive(false);
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
            Debug.Log("����"); 
            if (rainParticle != null)
            {
                rainParticle.SetActive(false);
            }
            WeaderApplication(false);
        }
    }
}
