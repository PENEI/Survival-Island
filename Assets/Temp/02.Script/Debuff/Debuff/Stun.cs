using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : Debuff
{
    void Update()
    {
        Debuff_Destroy();
    }

    /* 3(G)�� �Ŀ� ���� */
    public override void Debuff_Destroy()
    {
        if (!Player.Instance._Debuff.isDebuff.isStun)
        {
            Destroy(this);
        }
        if (info.careValue[0] <= MaintenanceTime()) { Destroy(this); }
    }
}
