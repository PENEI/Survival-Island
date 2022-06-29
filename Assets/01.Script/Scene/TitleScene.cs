using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    [SerializeField]
    Button m_LoadButton;        //불러오기 버튼
    [SerializeField]
    Resolution m_Resolution;     //옵션

    [SerializeField]
    GameObject m_Opation;

    [SerializeField]
    SoundOption[] soundOptionArr; 

    private void Awake()
    {
        SoundManager soundManager = SoundManager.Instance;

        m_Resolution.Init();

        FileInfo fileInfo = new FileInfo(FileData.Instance.FilePath);
        //파일 있는지 확인 있을때(true), 없으면(false)
        if (!fileInfo.Exists)
        {
            //파일 없으면 버튼 비활성화
            m_LoadButton.interactable = false;
        }

        //스크린 설정
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

    //새 게임 버튼
    public void _On_NewButton(string _scenename)
    {
        FileData.Instance.isSaveLoad = false;       //세이브 불러오기X
        SceneLoader.Instance.LoadGameScene(_scenename, true);
    }

    //불러오기 버튼
    public void _On_LoadButton(string _scenename)
    {
        FileData.Instance.isSaveLoad = true;        //세이브 불러오기O
        SceneLoader.Instance.LoadGameScene(_scenename, true);
    }

    public void _On_OptionButton(GameObject _object)
    {
        _object.SetActive(!_object.activeSelf);
    }

    public void _On_QuitButton()
    {
        //유니티 에디터
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;        //에디터 플레이 종료
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
