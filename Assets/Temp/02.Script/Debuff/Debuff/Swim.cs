using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim : Debuff
{
    void Update()
    {
        Debuff_Destroy();
    }

    /* �÷��̾��� Y�� ���̰� ���� ���̸� ���� �� */
    public override void Debuff_Destroy()
    {
        if (!Player.Instance._Debuff.isDebuff.isSwim){ Destroy(this); }
        if (Player.Instance.transform.position.y > info.conditionValue[0]) { Destroy(this); }
    }
}
