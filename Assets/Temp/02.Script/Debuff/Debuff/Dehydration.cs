using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dehydration : Debuff
{
    void Update()
    {
        Debuff_Destroy();
    }

    /* 수분이 75이상 일때 회복 */
    public override void Debuff_Destroy()
    {
        if (!Player.Instance._Debuff.isDebuff.isDehydration) { Destroy(this); }
        if (Player.Instance._Status.status.hydration.statusValue >= info.careValue[0]) { Destroy(this); }
    }
}
