using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����
public class Heatwave : Weader
{
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
                WeaderApplication(true, status.status.hydration);
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
                WeaderApplication(false, status.status.hydration);
                //---------------------------------------
            }
        }
    }

    private void OnDestroy()
    {
        //���� �� �ʱ�ȭ�� �Ǳ����� ���� �� �ʱ�ȭ
        if(distroyOn)
        {
            Debug.Log("����");
            WeaderApplication(false, status.status.hydration);
        }
    }
}
