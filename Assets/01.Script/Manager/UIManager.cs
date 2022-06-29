using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    public DragSlot dragSlotObj;        //드래그 슬롯
    public Message messageUI;           //알림창
    public InvenPanel invenPanel;        //인벤토리 패널
    public RectTransform Crosshair;    //크로스헤어
    public RectTransform InfoPanel;            //상태, 날씨, 인벤토리 등 패널
    public TempInvenPanel TempInven;         //임시 인벤토리
    public RectTransform InteractionKey;     //상호작용키 유아이
    public MakingPanel InvenMakingPanel;     //인벤토리제작패널
    public CountSelectPanel CountSelect;     //갯수선택패널
    public TrashPanel trashPanel;               //버리기확인패널
    public EquipPanel equipPanel;              //장비패널
    public MakingPanelList makingPanelList; //제작패널 리스트 
    public BuildPanel BuildPanel;                //건축패널 리스트
    public RectTransform SleepUI;               //수면 UI
    public PausePanel pausePanel;               //일시정지
    public ItemTooltip itemTooltip;             //아이템 툴팁
    public RecipeGuide recipeGuide;            //제작 가이드
    public GameOver GameOver;         //게임오버

    [Header("내구도")]
    public int DurabilityCount;             //내구도
    public Color DurabilityColor;           //내구도 색


    [Space(10)]
    public bool isSplit;                //분할확인

    [SerializeField]
    List<GameObject> ActiveUIList;

    void Awake()
    {
        ActiveUIList = new List<GameObject>();
    }

    private void Start()
    {
        //마우스커서 숨기기
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

    //인벤토리 열기/닫기
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

    //유아이, 인벤토리 열기/닫기
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
        // 유아이가 꺼져있을 경우
        if(!_UI.activeSelf)
        {
            // 제작 가능 확인
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
