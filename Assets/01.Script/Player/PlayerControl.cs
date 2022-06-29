using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    [Header("ī�޶� ���� ���"), SerializeField]
    bool isAllowCamera;

    [Header("�÷��̾� �̵��ӵ�")]
    public float MoveSpeed;     //�÷��̾� �̵��ӵ�
    [Header("�÷��̾� ȸ���ӵ�")]
    public float RotationSpeed; //�÷��̾� ȸ�� �ӵ�

    [Header("���ӵ�")]
    public float Accel;             //���ӵ�
    [Header("�ִ� �ӵ�")]
    public float MaxMoveSpeed;  //�ִ� �ӵ�

    [Header("ĳ���� ���� ���")]
    public bool isAllowCharMove;        //ĳ���� ������

    //

    [Header("��ȣ�ۿ� �Ÿ�")]
    public float RaycastDistance = 10;    //����ĳ��Ʈ �Ÿ�
    [Header("��ȣ�ۿ� ���")]
    public bool isAllowInteraction;
    [Header("���� ������ ��� ���")]
    public bool isAllowItem;
    [Header("���� ���")]
    public bool isAllowMaking;

    //

    [Header("�÷��̾� ������Ʈ")]
    public Transform PlayerModel; //�÷��̾� ������Ʈ

    [Header("���� ����")]
    public PlayerHandTool R_HandTool;   //���� 
    public PlayerHandTool L_HandTool;   //���� 

    //

    [Header("�� Ȯ�ο�")]
    public bool isCameraRot;    //ī�޶� ȸ�� Ȯ��
    public bool isMove;           //�̵�Ȯ��
    public bool isAccel;            //�޸��� Ȯ��
    public bool isMoveReduction;    //�̵��ӵ� ���� 

    [Header("���� �ӵ�")]
    [SerializeField]
    float m_CurMoveSpeed;       //���� �ӵ�
    [SerializeField]
    float m_MoveReduction;      //�̵��ӵ� ���� ��

    Camera m_MainCamera;                        //���� ī�޶�



    Dictionary<KeyCode, Action> m_KeyDic;   //Ű�Է� ��ųʸ�
    public ActionObj m_ActionObj;                  //��ȣ�ۿ�� ������Ʈ
    Rigidbody m_rigid;

    E_Effect_Player player_sound = E_Effect_Player.Max;     //���� ��� ����

    void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();

        m_MainCamera = Camera.main;
        
        //Ű ��ųʸ�, (Ű�ڵ�, �Լ�) 
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
        //���� �ӵ� = �⺻ �̵��ӵ�
        m_CurMoveSpeed = MoveSpeed;

        //���Ȯ��
        isAllowCharMove = true;
        isAllowInteraction = true;
        isAllowItem = true;
        isAllowMaking = true;

    }

    void Update()
    {
        KeyInput();                //Ű �Է�

        if (GameManager.Instance.isUpdating)
        {
            PlayerMoveSoundStop();

            return;
        }

        CameraRay();              //ī�޶󿡼� ��ȣ�ۿ� ����
        Move();                     //�̵�
    }

    //ī�޶� ������, ĳ���� ������ ���
    public void SetAllowMove(bool _isallow)
    {
        SetCamreMove(_isallow);
        isAllowCharMove = _isallow;
    }
    
    //ī�޶� ������, ĳ���� ������, ��ȣ�ۿ� ���, ���� ������ ��� ���, ���� ���
    public void SetAllowAll(bool _isallow)
    {
        SetCamreMove(_isallow);
        isAllowCharMove = _isallow;
        isAllowInteraction = _isallow;
        isAllowItem = _isallow;
        isAllowMaking = _isallow;
    }
    //��Ʈ��
    public void SetCamreMove(bool _isCameraMove)
    {
        if (isAllowCamera)
            return;
        CameraManager.Instance.cinemachineControl.SetAimMove(_isCameraMove);
    }

    /// <summary>
    /// �̵��ӵ� ���� ����
    /// </summary>
    /// <param name="_isReduction">�̵��ӵ� ���� true: �̵��ӵ� ����O, false: �̵��ӵ� ����X</param>
    /// <param name="_minusSpeed">���ҵ� �ӵ�</param>
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
        //��� ����
        SoundManager.Instance.StopSound(E_SoundType.Effect_NoOne);
        player_sound = E_Effect_Player.Max;

    }

    //Ű���� �̵�
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

        //�̵� Ȯ��
        if (hor == 0 && ver == 0)
        {
            isMove = false;
            m_CurMoveSpeed = MoveSpeed;         //���� �ӵ���
            PlayerMoveSoundStop();
            return;
        }
        else
        {
            isMove = true;
        }
        //�޸��� Ȯ��
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isAccel = true;
            float tempaccel = Accel * Time.deltaTime;
            //�ִ� �ӵ� Ȯ���ϰ� �ӵ� ����
            if (m_CurMoveSpeed + tempaccel < MaxMoveSpeed)
                m_CurMoveSpeed += tempaccel;
            else
                m_CurMoveSpeed = MaxMoveSpeed;
        }
        else
        {
            isAccel = false;
            m_CurMoveSpeed = MoveSpeed;         //���� �ӵ���
        }

        float tempreduction = 0;        //�ӽ� �̵��ӵ� ���Ұ�
        //�̵��ӵ� ���� ��
        if (isMoveReduction)
        {
            //���ӵ� �ӵ��� 0���� ũ�� �̵��ӵ� ������
            if (m_CurMoveSpeed + m_MoveReduction > 0)
            {
                tempreduction = m_MoveReduction;
            }
        }

        //�̵� ��
        Vector3 tempdir = ((Quaternion.Euler(new Vector3(0f, m_MainCamera.transform.localEulerAngles.y, 0f))
            * new Vector3(hor, 0f, ver).normalized));

        Vector3 rotdir = tempdir;
        Vector3 movedir = (tempdir
            * (m_CurMoveSpeed + tempreduction) * Time.deltaTime);

        //ȸ��
        Quaternion rot = Quaternion.LookRotation(rotdir);
        m_rigid.rotation = Quaternion.Slerp(m_rigid.rotation, rot, RotationSpeed * Time.deltaTime);

        //��ġ ����
        m_rigid.MovePosition(this.transform.position + movedir);

        //����
        E_Effect_Player cur_sound;
        //���� ����� ���� �˾Ƴ���
        if (!isAccel)
        {
            cur_sound = E_Effect_Player.Effect_Walk;
        }
        else
        {
            cur_sound = E_Effect_Player.Effect_Run;
        }

        //���� ����� ������ ���X
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

    //ī�޶� �������� ���̽��
    void CameraRay()
    {
        UIManager.Instance.SetActiveUI(UIManager.Instance.InteractionKey.gameObject, false);
        if (!isAllowInteraction)
        {
            return;
        }

        ActionObj tempActionObj = null;     //��ȣ�ۿ�� ������Ʈ

        int layerMask = (1 << LayerMask.NameToLayer("Player"));  // Everything���� Player ���̾ �����ϰ� �浹 üũ��
        layerMask = ~layerMask;

        Ray ray = new Ray(m_MainCamera.transform.position, m_MainCamera.transform.forward);
        RaycastHit[] hitinfo = Physics.RaycastAll(ray, RaycastDistance, layerMask);

        Debug.DrawRay(ray.origin, ray.direction * RaycastDistance, Color.blue);
        foreach(RaycastHit item in hitinfo)
        {
            if (item.transform.tag == "ActionObj")
            {
                tempActionObj = item.transform.GetComponent<ActionObj>();

                //��ȣ�ۿ� �������� Ȯ��
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

        //��ȣ�ۿ� ������Ʈ Ȯ��
        if (m_ActionObj != null)
            UIManager.Instance.SetActiveUI(UIManager.Instance.InteractionKey.gameObject, true);
    }

    //Ű �Է�
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


    //��ȣ�ۿ�
    void Interaction()
    {
        if (!isAllowInteraction)
        {
            return;
        }

        //��ȣ�ۿ����� ������Ʈ ����
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
