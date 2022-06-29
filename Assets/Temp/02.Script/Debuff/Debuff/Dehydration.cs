using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dehydration : Debuff
{
    void Update()
    {
        Debuff_Destroy();
    }

    /* ������ 75�̻� �϶� ȸ�� */
    public override void Debuff_Destroy()
    {
        if (!Player.Instance._Debuff.isDebuff.isDehydration) { Destroy(this); }
        if (Player.Instance._Status.status.hydration.statusValue >= info.careValue[0]) { Destroy(this); }
    }
}
