using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bodyache : Debuff
{
    private void Update()
    {
        Debuff_Destroy();
    }

    /* 피로가 40이상일 때 회복 */
    public override void Debuff_Destroy()
    {
        if(!Player.Instance._Debuff.isDebuff.isBodyache){ Destroy(this); }
        if (Player.Instance._Status.status.fatigue.statusValue >= info.careValue[0]) { Destroy(this); }
    }
}
