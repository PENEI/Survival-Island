using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim : Debuff
{
    void Update()
    {
        Debuff_Destroy();
    }

    /* 플레이어의 Y축 높이가 일정 높이를 넘을 때 */
    public override void Debuff_Destroy()
    {
        if (!Player.Instance._Debuff.isDebuff.isSwim){ Destroy(this); }
        if (Player.Instance.transform.position.y > info.conditionValue[0]) { Destroy(this); }
    }
}
