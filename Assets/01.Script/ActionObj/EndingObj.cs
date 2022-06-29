using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingObj : MonoBehaviour, ActionObj
{
    [Header("상호작용 허용")]
    public bool isAllowAction;

    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

    //게임종료
    public void Action()
    {
        //컷신으로
        Player.Instance.Control.SetAllowAll(false);
        SceneLoader.Instance.LoadGameScene("Credit", false);
    }

    public E_InteractionType GetInteractionType()
    {
        return E_InteractionType.Ending;
    }

    public bool IsAction()
    {
        //상호작용 가능한지 확인
        if (isAllowAction)
            return true;

        return false;
    }

}
