using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상호작용
public interface ActionObj
{
    //상호작용 가능한지 확인
    public bool IsAction();

    //상호작용
    void Action();

    //상호작용 타입 얻기
    E_InteractionType GetInteractionType();

}
