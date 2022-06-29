using UnityEngine;

public class GameSingleInc : MonoBehaviour
{
    CSVManager m_CSVManager;
    ObjManager m_objManager;
    UIManager m_UIManager;
    XmlManager m_XmlManager;
    MakingManager m_MakingManager;
    WeaponManager m_WeaponManager;

    bool isTutorial;    //Ʃ�丮�� Ȱ��ȭ Ȯ��
    void Awake()
    {
        //���ӸŴ��� �ʱ�ȭ
        SoundManager soundManager = SoundManager.Instance;
        m_CSVManager = CSVManager.Instance;
        m_XmlManager = XmlManager.Instance;
        m_objManager = ObjManager.Instance;
        m_UIManager = UIManager.Instance;
        m_MakingManager = MakingManager.Instance;
        m_WeaponManager = WeaponManager.Instance;
    }

    private void Start()
    {
    }

    private void Update()
    {
        //Ʃ�丮�� Ȱ��ȭ
        if (!isTutorial)
        {
            if (!GameManager.Instance.isUpdating)
            {
                if (!FileData.Instance.isSaveLoad)
                {
                    isTutorial = true;
                    UIManager.Instance.pausePanel.ShowPause();
                    UIManager.Instance.pausePanel._On_TutorialButton(true);
                }
            }
        }


    }
}
