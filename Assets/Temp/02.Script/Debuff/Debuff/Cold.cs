using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cold : Debuff
{
    private void Update()
    {
        Debuff_Destroy();
    }

    /* 12(G)�ð� �Ŀ� ȸ�� */
    public override void Debuff_Destroy()
    {
        //isCold�� false�϶� ����
        if (!Player.Instance._Debuff.isDebuff.isCold){ Destroy(this); }
        if ((info.careValue[0]) <= MaintenanceTime()) { Destroy(this); }
    }
}
