using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    public AnimationClip[] playerAniClip;
    public Dictionary<string, float> animationTime;

    protected override void SingletonInit()
    {
        base.SingletonInit();

        animationTime = new Dictionary<string, float>();
        //�ִϸ��̼� ���� �ð� ����
        foreach (var ani in playerAniClip)
        {
            animationTime.Add(ani.name, ani.length);
        }
    }
}
