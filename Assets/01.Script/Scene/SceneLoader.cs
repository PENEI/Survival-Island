using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : SingletonDontDestroy<SceneLoader>
{
    public float fadeDuration = 2;      //���̵� �ξƿ� �ð�

    [SerializeField]
    CanvasGroup m_BackGound;        //���
    [SerializeField]
    Image m_ProgressBar;        //�ε���
    [SerializeField]
    Text m_LoadingText;         //�ε� �ۼ�Ʈ �ؽ�Ʈ

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
        SceneManager.sceneLoaded += OnSceneLoaded; //�� �ε� �̺�Ʈ�� �߰�

        m_ProgressBar.gameObject.SetActive(false);
        m_LoadingText.gameObject.SetActive(false);
        m_BackGound.gameObject.SetActive(false);
    }

    //�� ���� ��ư
    public void _On_LoadButton(string _scenename)
    {
        SceneLoader.Instance.LoadGameScene(_scenename, false);
    }

    /// <summary>
    /// �� �ҷ�����
    /// </summary>
    /// <param name="_SceneName">�� �̸�</param>
    /// <param name="isUIAction">�ۼ�Ʈ ������ �����ִ��� | üũ true: ������, false: �Ⱥ�����</param>
    public void LoadGameScene(string _SceneName, bool isUIAction)
    {
        //���̵� ��
        m_BackGound.DOFade(1, fadeDuration)
       .OnStart(() => {
           m_BackGound.gameObject.SetActive(true);
           m_BackGound.blocksRaycasts = true; //�Ʒ� ����ĳ��Ʈ ����
    })
       .OnComplete(() => {
           if (isUIAction)
               StartCoroutine(UIActionLoadAsyncScene(_SceneName));
           else
               StartCoroutine(LoadAsyncScene(_SceneName));
       });
    }

    //�ε� ������ ������
    IEnumerator UIActionLoadAsyncScene(string _sceneName)
    {
        m_ProgressBar.gameObject.SetActive(true);
        m_LoadingText.gameObject.SetActive(true);

        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(_sceneName);
        asyncOper.allowSceneActivation = false;     //�� ��ȯ�غ� X

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
                    asyncOper.allowSceneActivation = true; //�� ��ȯ �غ� �Ϸ�
                }
            }
            else
            {
                percentage = Mathf.Lerp(percentage, asyncOper.progress * 100f, past_time);
                if (percentage >= 90) past_time = 0;
            }
            m_ProgressBar.fillAmount = percentage * 0.01f;
            m_LoadingText.text = percentage.ToString("0") + "%"; //�ε� �ۼ�Ʈ ǥ��
        }
    }

    //�Ⱥ�����
    IEnumerator LoadAsyncScene(string _sceneName)
    {
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(_sceneName);

        while (asyncOper.isDone)
        {
            yield return null;
        }
    }

    //�� �ε� �� �Լ�
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameManager.Instance.isUpdating = true;
        //���̵� �ƿ�
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
