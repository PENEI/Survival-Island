using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineControl : MonoBehaviour
{
    Cinemachine.CinemachineVirtualCamera VirtualCamera;
    Cinemachine.CinemachinePOV PovAim;

    //��ǲ
    string InputVertical;           
    string InputHorizon;
    //���ǵ�
    float hor_value;
    float ver_value;
     
    bool isCameraUpdating;      //������Ʈ Ȯ��

    bool isinit;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        if(!isinit)
        {
            isinit = true;
            VirtualCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
            PovAim = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachinePOV>();
            //�ʱ� ��ǲ ��
            InputVertical = PovAim.m_VerticalAxis.m_InputAxisName;
            InputHorizon = PovAim.m_HorizontalAxis.m_InputAxisName;
            //�ʱ� ���ǵ� ��
            hor_value = PovAim.m_HorizontalAxis.m_MaxSpeed;
            ver_value = PovAim.m_VerticalAxis.m_MaxSpeed;
            isCameraUpdating = true;
        }
    }

    private void Update()
    {
        //������Ʈ ��Ȱ��ȭ �� ī�޶� �̵�X
        if(GameManager.Instance.isUpdating)
        {
            PovAim.m_VerticalAxis.m_InputAxisName = "";
            PovAim.m_HorizontalAxis.m_InputAxisName = "";
        }
        else
        {
            PovAim.m_VerticalAxis.m_InputAxisName = InputVertical;
            PovAim.m_HorizontalAxis.m_InputAxisName = InputHorizon;
        }
    }

    /// <summary>
    /// ȭ�� ������Ʈ ����
    /// </summary>
    /// <param name="_ismove"></param>
    public void SetAimMove(bool _ismove)
    {
        Init();
        if (_ismove)
        {
            //������Ʈ
            //�ʱ� ������ ����
            PovAim.m_VerticalAxis.m_InputAxisName = InputVertical;
            PovAim.m_HorizontalAxis.m_InputAxisName = InputHorizon;

            PovAim.m_HorizontalAxis.m_MaxSpeed = hor_value;
            PovAim.m_VerticalAxis.m_MaxSpeed = ver_value;
            isCameraUpdating = true;
        }
        else
        {
            //������ƮX
            //��ǲ�� �ȹ���
            PovAim.m_VerticalAxis.m_InputAxisName = "";
            PovAim.m_HorizontalAxis.m_InputAxisName = "";
            //�ӵ� 0
            PovAim.m_HorizontalAxis.m_MaxSpeed = 0;
            PovAim.m_VerticalAxis.m_MaxSpeed = 0;
            isCameraUpdating = false;
        }
    }
}
