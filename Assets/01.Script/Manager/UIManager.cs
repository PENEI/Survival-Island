using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    public DragSlot dragSlotObj;        //�巡�� ����
    public Message messageUI;           //�˸�â
    public InvenPanel invenPanel;        //�κ��丮 �г�
    public RectTransform Crosshair;    //ũ�ν����
    public RectTransform InfoPanel;            //����, ����, �κ��丮 �� �г�
    public TempInvenPanel TempInven;         //�ӽ� �κ��丮
    public RectTransform InteractionKey;     //��ȣ�ۿ�Ű ������
    public MakingPanel InvenMakingPanel;     //�κ��丮�����г�
    public CountSelectPanel CountSelect;     //���������г�
    public TrashPanel trashPanel;               //������Ȯ���г�
    public EquipPanel equipPanel;              //����г�
    public MakingPanelList makingPanelList; //�����г� ����Ʈ 
    public BuildPanel BuildPanel;                //�����г� ����Ʈ
    public RectTransform SleepUI;               //���� UI
    public PausePanel pausePanel;               //�Ͻ�����
    public ItemTooltip itemTooltip;             //������ ����
    public RecipeGuide recipeGuide;            //���� ���̵�
    public GameOver GameOver;         //���ӿ���

    [Header("������")]
    public int DurabilityCount;             //������
    public Color DurabilityColor;           //������ ��


    [Space(10)]
    public bool isSplit;                //����Ȯ��

    [SerializeField]
    List<GameObject> ActiveUIList;

    void Awake()
    {
        ActiveUIList = new List<GameObject>();
    }

    private void Start()
    {
        //���콺Ŀ�� �����
        CursorOnOff(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            isSplit = true;
        else
            isSplit = false;
    }

    public void CursorOnOff(bool _active)
    {
        if(_active)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (Player.Instance!=null)
            {
                Player.Instance.Control.SetCamreMove(false);
                //*****
               /* Player.Instance._animation.b_attack = false;*/
            }
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (Player.Instance != null)
            {
                Player.Instance.Control.SetCamreMove(true);
                //*****
                /*Player.Instance._animation.b_attack = true;*/
            }
        }
    }


    public void SetActiveUI(GameObject _ui, bool _isactive)
    {
        _ui.SetActive(_isactive);
        if (_ui == InteractionKey.gameObject)
            return;
        
        if(_isactive)
            ActiveUIList.Add(_ui);
        else
            ActiveUIList.Remove(_ui);

        if (ActiveUIList.Count >= 1)
            CursorOnOff(true);
        else
            CursorOnOff(false);
    }

    //�κ��丮 ����/�ݱ�
    public void ActiveInven()
    {
        if (GameManager.Instance.isUpdating)
            return;

        SetActiveUI(InfoPanel.gameObject, !InfoPanel.gameObject.activeSelf);
        if (InfoPanel.gameObject.activeSelf)
            Player.Instance.Control.SetCamreMove(false); 
        else
            Player.Instance.Control.SetCamreMove(true); 
    }

    //������, �κ��丮 ����/�ݱ�
    public void ActiveUINInven(GameObject _UI)
    {
        SetActiveUI(_UI, !_UI.activeSelf);

        SetActiveUI(InfoPanel.gameObject, _UI.activeSelf);

        if (_UI.activeSelf)
            Player.Instance.Control.SetCamreMove(false);
        else
            Player.Instance.Control.SetCamreMove(true);
    }

    public void ActiveMakingNInven(GameObject _UI)
    {
        // �����̰� �������� ���
        if(!_UI.activeSelf)
        {
            // ���� ���� Ȯ��
            if (!Player.Instance.Control.isAllowMaking)
            {
                return;
            }
        }

        SetActiveUI(_UI, !_UI.activeSelf);
        SetActiveUI(InfoPanel.gameObject, _UI.activeSelf);

        if (_UI.activeSelf)
            Player.Instance.Control.SetCamreMove(false);
        else
            Player.Instance.Control.SetCamreMove(true);
    }

    public void ActivePause()
    {
        if (pausePanel.gameObject.activeSelf)
        {
            pausePanel.HidePause();
        }
        else
        {
            if (GameManager.Instance.isUpdating)
                return;
            pausePanel.ShowPause();
        }
    }

    protected override void SingletonInit()
    {
        base.SingletonInit();
        InfoPanel.gameObject.SetActive(true);
        invenPanel.Init();
        equipPanel.Init();
        TempInven.gameObject.SetActive(true);
        TempInven.gameObject.SetActive(false);
        InfoPanel.gameObject.SetActive(false);

    }

}
