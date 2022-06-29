using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    [SerializeField]
    string TitleSceneName;
    void Awake()
    {
    }

    void Start()
    {
    }

    public void LoadTitleScene()
    {
        SceneLoader.Instance.LoadGameScene(TitleSceneName, false);
    }
}
