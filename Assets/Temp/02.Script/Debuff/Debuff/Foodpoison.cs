using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foodpoison : Debuff
{
    void Update()
    {
        Debuff_Destroy();
    }

    /* 12(G)�ð� �Ŀ� ���� */
    public override void Debuff_Destroy()
    {
        if (!Player.Instance._Debuff.isDebuff.isFoodpoison){ Destroy(this); }
        if ((info.careValue[0]) <= MaintenanceTime()) { Destroy(this); }
    }
}
