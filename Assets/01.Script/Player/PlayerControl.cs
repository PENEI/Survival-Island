using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    [Header("카메라 무빙 허용"), SerializeField]
    bool isAllowCamera;

    [Header("플레이어 이동속도")]
    public float MoveSpeed;     //플레이어 이동속도
    [Header("플레이어 회전속도")]
    public float RotationSpeed; //플레이어 회전 속도

    [Header("가속도")]
    public float Accel;             //가속도
    [Header("최대 속도")]
    public float MaxMoveSpeed;  //최대 속도

    [Header("캐릭터 무빙 허용")]
    public bool isAllowCharMove;        //캐릭터 움직임

    //

    [Header("상호작용 거리")]
    public float RaycastDistance = 10;    //레이캐스트 거리
    [Header("상호작용 허용")]
    public bool isAllowInteraction;
    [Header("음식 아이템 사용 허용")]
    public bool isAllowItem;
    [Header("제작 허용")]
    public bool isAllowMaking;

    //

    [Header("플레이어 오브젝트")]
    public Transform PlayerModel; //플레이어 오브젝트

    [Header("도구 장착")]
    public PlayerHandTool R_HandTool;   //도구 
    public PlayerHandTool L_HandTool;   //도구 

    //

    [Header("값 확인용")]
    public bool isCameraRot;    //카메라 회전 확인
    public bool isMove;           //이동확인
    public bool isAccel;            //달리기 확인
    public bool isMoveReduction;    //이동속도 감소 

    [Header("현재 속도")]
    [SerializeField]
    float m_CurMoveSpeed;       //현재 속도
    [SerializeField]
    float m_MoveReduction;      //이동속도 감소 값

    Camera m_MainCamera;                        //메인 카메라



    Dictionary<KeyCode, Action> m_KeyDic;   //키입력 딕셔너리
    public ActionObj m_ActionObj;                  //상호작용된 오브젝트
    Rigidbody m_rigid;

    E_Effect_Player player_sound = E_Effect_Player.Max;     //사운드 재생 상태

    void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();

        m_MainCamera = Camera.main;
        
        //키 딕셔너리, (키코드, 함수) 
        m_KeyDic = new Dictionary<KeyCode, Action>
        {
            { KeyCode.E, Interaction },
            { KeyCode.Tab, UIManager.Instance.ActiveInven },
            { KeyCode.C, () => UIManager.Instance.ActiveMakingNInven(UIManager.Instance.InvenMakingPanel.gameObject)},
            { KeyCode.Escape,  UIManager.Instance.ActivePause },
            { KeyCode.Q , UIManager.Instance.equipPanel.OffHandTool },

        };
    }
    void Start()
    {
        //현재 속도 = 기본 이동속도
        m_CurMoveSpeed = MoveSpeed;

        //허용확인
        isAllowCharMove = true;
        isAllowInteraction = true;
        isAllowItem = true;
        isAllowMaking = true;

    }

    void Update()
    {
        KeyInput();                //키 입력

        if (GameManager.Instance.isUpdating)
        {
            PlayerMoveSoundStop();

            return;
        }

        CameraRay();              //카메라에서 상호작용 레이
        Move();                     //이동
    }

    //카메라 움직임, 캐릭터 움직임 허용
    public void SetAllowMove(bool _isallow)
    {
        SetCamreMove(_isallow);
        isAllowCharMove = _isallow;
    }
    
    //카메라 움직임, 캐릭터 움직임, 상호작용 허용, 음식 아이템 사용 허용, 제작 허용
    public void SetAllowAll(bool _isallow)
    {
        SetCamreMove(_isallow);
        isAllowCharMove = _isallow;
        isAllowInteraction = _isallow;
        isAllowItem = _isallow;
        isAllowMaking = _isallow;
    }
    //컨트롤
    public void SetCamreMove(bool _isCameraMove)
    {
        if (isAllowCamera)
            return;
        CameraManager.Instance.cinemachineControl.SetAimMove(_isCameraMove);
    }

    /// <summary>
    /// 이동속도 감소 세팅
    /// </summary>
    /// <param name="_isReduction">이동속도 감소 true: 이동속도 감소O, false: 이동속도 감소X</param>
    /// <param name="_minusSpeed">감소된 속도</param>
    public void SetMoveReduction(bool _isReduction, float _minusSpeed)
    {
        if (_isReduction)
        {
            isMoveReduction = _isReduction;
            m_MoveReduction = _minusSpeed;
        }
        else
        {
            isMoveReduction = _isReduction;
            m_MoveReduction = 0;
        }
        /*isMoveReduction = _isReduction;
        m_MoveReduction += _minusSpeed;*/
    }

    void PlayerMoveSoundStop()
    {
        //재생 종료
        SoundManager.Instance.StopSound(E_SoundType.Effect_NoOne);
        player_sound = E_Effect_Player.Max;

    }

    //키보드 이동
    void Move()
    {
        if(!isAllowCharMove)
        {
            isMove = false;
            PlayerMoveSoundStop();
            return;
        }

        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        //이동 확인
        if (hor == 0 && ver == 0)
        {
            isMove = false;
            m_CurMoveSpeed = MoveSpeed;         //원래 속도로
            PlayerMoveSoundStop();
            return;
        }
        else
        {
            isMove = true;
        }
        //달리기 확인
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isAccel = true;
            float tempaccel = Accel * Time.deltaTime;
            //최대 속도 확인하고 속도 정함
            if (m_CurMoveSpeed + tempaccel < MaxMoveSpeed)
                m_CurMoveSpeed += tempaccel;
            else
                m_CurMoveSpeed = MaxMoveSpeed;
        }
        else
        {
            isAccel = false;
            m_CurMoveSpeed = MoveSpeed;         //원래 속도로
        }

        float tempreduction = 0;        //임시 이동속도 감소값
        //이동속도 감소 시
        if (isMoveReduction)
        {
            //감속된 속도가 0보다 크면 이동속도 증감소
            if (m_CurMoveSpeed + m_MoveReduction > 0)
            {
                tempreduction = m_MoveReduction;
            }
        }

        //이동 값
        Vector3 tempdir = ((Quaternion.Euler(new Vector3(0f, m_MainCamera.transform.localEulerAngles.y, 0f))
            * new Vector3(hor, 0f, ver).normalized));

        Vector3 rotdir = tempdir;
        Vector3 movedir = (tempdir
            * (m_CurMoveSpeed + tempreduction) * Time.deltaTime);

        //회전
        Quaternion rot = Quaternion.LookRotation(rotdir);
        m_rigid.rotation = Quaternion.Slerp(m_rigid.rotation, rot, RotationSpeed * Time.deltaTime);

        //위치 변경
        m_rigid.MovePosition(this.transform.position + movedir);

        //사운드
        E_Effect_Player cur_sound;
        //현재 재생할 사운드 알아내기
        if (!isAccel)
        {
            cur_sound = E_Effect_Player.Effect_Walk;
        }
        else
        {
            cur_sound = E_Effect_Player.Effect_Run;
        }

        //이전 사운드와 같으면 재생X
        if(cur_sound != player_sound)
        {
            if (!isAccel)
            {
                SoundManager.Instance.PlaySound(E_SoundType.Effect_NoOne, E_Effect_Player.Effect_Walk);
                player_sound = E_Effect_Player.Effect_Walk;
            }
            else
            {
                SoundManager.Instance.PlaySound(E_SoundType.Effect_NoOne, E_Effect_Player.Effect_Run);
                player_sound = E_Effect_Player.Effect_Run;
            }
        }
    }

    //카메라 기준으로 레이쏘기
    void CameraRay()
    {
        UIManager.Instance.SetActiveUI(UIManager.Instance.InteractionKey.gameObject, false);
        if (!isAllowInteraction)
        {
            return;
        }

        ActionObj tempActionObj = null;     //상호작용된 오브젝트

        int layerMask = (1 << LayerMask.NameToLayer("Player"));  // Everything에서 Player 레이어만 제외하고 충돌 체크함
        layerMask = ~layerMask;

        Ray ray = new Ray(m_MainCamera.transform.position, m_MainCamera.transform.forward);
        RaycastHit[] hitinfo = Physics.RaycastAll(ray, RaycastDistance, layerMask);

        Debug.DrawRay(ray.origin, ray.direction * RaycastDistance, Color.blue);
        foreach(RaycastHit item in hitinfo)
        {
            if (item.transform.tag == "ActionObj")
            {
                tempActionObj = item.transform.GetComponent<ActionObj>();

                //상호작용 가능한지 확인
                if(tempActionObj.IsAction())
                {
                    break;
                }
                else
                {
                    tempActionObj = null;
                }
            }
        }
        m_ActionObj = tempActionObj;

        //상호작용 오브젝트 확인
        if (m_ActionObj != null)
            UIManager.Instance.SetActiveUI(UIManager.Instance.InteractionKey.gameObject, true);
    }

    //키 입력
    void KeyInput()
    {
        if(Input.anyKeyDown)
        {
            foreach(var dic in m_KeyDic)
            {
                if (GameManager.Instance.isUpdating
                    && !Input.GetKeyDown(KeyCode.Escape))
                    return;

                if(Input.GetKeyDown(dic.Key))
                {
                    dic.Value();
                }
            }
        }

    }


    //상호작용
    void Interaction()
    {
        if (!isAllowInteraction)
        {
            return;
        }

        //상호작용중인 오브젝트 실행
        if (m_ActionObj != null)
        {
            m_ActionObj.Action();
            m_ActionObj = null;
        }
     }

    //---------------------
    public float GetSpeed()
    {
        return m_CurMoveSpeed;
    }
}
