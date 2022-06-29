using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : SingletonDontDestroy<SceneLoader>
{
    public float fadeDuration = 2;      //페이드 인아웃 시간

    [SerializeField]
    CanvasGroup m_BackGound;        //배경
    [SerializeField]
    Image m_ProgressBar;        //로딩바
    [SerializeField]
    Text m_LoadingText;         //로딩 퍼센트 텍스트

    void Awake()
    {
        if (m_instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
    }

    void Start()
    {
        m_ProgressBar.fillAmount = 0;
        SceneManager.sceneLoaded += OnSceneLoaded; //씬 로드 이벤트에 추가

        m_ProgressBar.gameObject.SetActive(false);
        m_LoadingText.gameObject.SetActive(false);
        m_BackGound.gameObject.SetActive(false);
    }

    //새 게임 버튼
    public void _On_LoadButton(string _scenename)
    {
        SceneLoader.Instance.LoadGameScene(_scenename, false);
    }

    /// <summary>
    /// 씬 불러오기
    /// </summary>
    /// <param name="_SceneName">씬 이름</param>
    /// <param name="isUIAction">퍼센트 유아이 보여주는지 | 체크 true: 보여줌, false: 안보여줌</param>
    public void LoadGameScene(string _SceneName, bool isUIAction)
    {
        //페이드 인
        m_BackGound.DOFade(1, fadeDuration)
       .OnStart(() => {
           m_BackGound.gameObject.SetActive(true);
           m_BackGound.blocksRaycasts = true; //아래 레이캐스트 막기
    })
       .OnComplete(() => {
           if (isUIAction)
               StartCoroutine(UIActionLoadAsyncScene(_SceneName));
           else
               StartCoroutine(LoadAsyncScene(_SceneName));
       });
    }

    //로딩 유아이 보여줌
    IEnumerator UIActionLoadAsyncScene(string _sceneName)
    {
        m_ProgressBar.gameObject.SetActive(true);
        m_LoadingText.gameObject.SetActive(true);

        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(_sceneName);
        asyncOper.allowSceneActivation = false;     //씬 전환준비 X

        float past_time = 0;
        float percentage = 0;

        while (!asyncOper.isDone)
        {
            yield return null;

            past_time += Time.deltaTime;

            if (percentage >= 90)
            {
                percentage = Mathf.Lerp(percentage, 100, past_time);

                if (percentage == 100)
                {
                    asyncOper.allowSceneActivation = true; //씬 전환 준비 완료
                }
            }
            else
            {
                percentage = Mathf.Lerp(percentage, asyncOper.progress * 100f, past_time);
                if (percentage >= 90) past_time = 0;
            }
            m_ProgressBar.fillAmount = percentage * 0.01f;
            m_LoadingText.text = percentage.ToString("0") + "%"; //로딩 퍼센트 표기
        }
    }

    //안보여줌
    IEnumerator LoadAsyncScene(string _sceneName)
    {
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(_sceneName);

        while (asyncOper.isDone)
        {
            yield return null;
        }
    }

    //씬 로드 시 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameManager.Instance.isUpdating = true;
        //페이드 아웃
        m_BackGound.DOFade(0, fadeDuration).SetEase(Ease.OutCubic)
        .OnStart(() => {
            m_ProgressBar.gameObject.SetActive(false);
            m_LoadingText.gameObject.SetActive(false);
        })
        .OnComplete(() => {
            m_BackGound.blocksRaycasts = false;
            m_BackGound.gameObject.SetActive(false);
            GameManager.Instance.isUpdating = false;
        });
    }
}
