using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineControl : MonoBehaviour
{
    Cinemachine.CinemachineVirtualCamera VirtualCamera;
    Cinemachine.CinemachinePOV PovAim;

    //인풋
    string InputVertical;           
    string InputHorizon;
    //스피드
    float hor_value;
    float ver_value;
     
    bool isCameraUpdating;      //업데이트 확인

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
            //초기 인풋 값
            InputVertical = PovAim.m_VerticalAxis.m_InputAxisName;
            InputHorizon = PovAim.m_HorizontalAxis.m_InputAxisName;
            //초기 스피드 값
            hor_value = PovAim.m_HorizontalAxis.m_MaxSpeed;
            ver_value = PovAim.m_VerticalAxis.m_MaxSpeed;
            isCameraUpdating = true;
        }
    }

    private void Update()
    {
        //업데이트 비활성화 시 카메라 이동X
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
    /// 화면 업데이트 세팅
    /// </summary>
    /// <param name="_ismove"></param>
    public void SetAimMove(bool _ismove)
    {
        Init();
        if (_ismove)
        {
            //업데이트
            //초기 값으로 돌림
            PovAim.m_VerticalAxis.m_InputAxisName = InputVertical;
            PovAim.m_HorizontalAxis.m_InputAxisName = InputHorizon;

            PovAim.m_HorizontalAxis.m_MaxSpeed = hor_value;
            PovAim.m_VerticalAxis.m_MaxSpeed = ver_value;
            isCameraUpdating = true;
        }
        else
        {
            //업데이트X
            //인풋값 안받음
            PovAim.m_VerticalAxis.m_InputAxisName = "";
            PovAim.m_HorizontalAxis.m_InputAxisName = "";
            //속도 0
            PovAim.m_HorizontalAxis.m_MaxSpeed = 0;
            PovAim.m_VerticalAxis.m_MaxSpeed = 0;
            isCameraUpdating = false;
        }
    }
}
