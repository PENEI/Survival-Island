using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wound : Debuff
{
    void Update()
    {
        Debuff_Destroy();
    }

    /* 24��(G) ���� �� ���� */
    public override void Debuff_Destroy()
    {
        if (!Player.Instance._Debuff.isDebuff.isWound) { Destroy(this); }
        if (info.careValue[0] <= MaintenanceTime()) { Destroy(this); }
    } 
}
