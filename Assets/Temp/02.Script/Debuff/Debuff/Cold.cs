using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cold : Debuff
{
    private void Update()
    {
        Debuff_Destroy();
    }

    /* 12(G)시간 후에 회복 */
    public override void Debuff_Destroy()
    {
        //isCold가 false일때 삭제
        if (!Player.Instance._Debuff.isDebuff.isCold){ Destroy(this); }
        if ((info.careValue[0]) <= MaintenanceTime()) { Destroy(this); }
    }
}
