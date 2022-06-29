using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    [SerializeField]
    Button m_LoadButton;        //�ҷ����� ��ư
    [SerializeField]
    Resolution m_Resolution;     //�ɼ�

    [SerializeField]
    GameObject m_Opation;

    [SerializeField]
    SoundOption[] soundOptionArr; 

    private void Awake()
    {
        SoundManager soundManager = SoundManager.Instance;

        m_Resolution.Init();

        FileInfo fileInfo = new FileInfo(FileData.Instance.FilePath);
        //���� �ִ��� Ȯ�� ������(true), ������(false)
        if (!fileInfo.Exists)
        {
            //���� ������ ��ư ��Ȱ��ȭ
            m_LoadButton.interactable = false;
        }

        //��ũ�� ����
        bool full = System.Convert.ToBoolean(Mathf.Clamp(PlayerPrefs.GetInt("FullScreen", 1),0,1));
        int screen = PlayerPrefs.GetInt("Screen", 2);
        m_Resolution.ScreenResolutionInx(screen, full);

        m_Opation.SetActive(true);
        foreach (SoundOption item in soundOptionArr)
        {
            item.Init();
        }
        m_Opation.SetActive(false);

    }

    private void Start()
    {
        foreach (SoundOption item in soundOptionArr)
        {
            item.SettingSound();
        }
    }

    //�� ���� ��ư
    public void _On_NewButton(string _scenename)
    {
        FileData.Instance.isSaveLoad = false;       //���̺� �ҷ�����X
        SceneLoader.Instance.LoadGameScene(_scenename, true);
    }

    //�ҷ����� ��ư
    public void _On_LoadButton(string _scenename)
    {
        FileData.Instance.isSaveLoad = true;        //���̺� �ҷ�����O
        SceneLoader.Instance.LoadGameScene(_scenename, true);
    }

    public void _On_OptionButton(GameObject _object)
    {
        _object.SetActive(!_object.activeSelf);
    }

    public void _On_QuitButton()
    {
        //����Ƽ ������
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;        //������ �÷��� ����
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }
}
