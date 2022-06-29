using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayObj : GatheringObj
{
    [Header("�ٽ� ä���ϱ���� �ɸ��� �ð�"), SerializeField]
    float Gathering_Delay = 2.0f;

    protected override void Update()
    {
        base.Update();

        if (GameManager.Instance.isUpdating)
            return;

        if(gatheringInfo.isDelay)
        {
            gatheringInfo.curDelay -= Time.deltaTime;
            if(gatheringInfo.curDelay <= 0)
            {
                gatheringInfo.curDelay = 0f;
                gatheringInfo.isDelay = false;
                isAllowGet = true;
            }
        }
    }

    protected override void CompleteProcess()
    {
        gatheringInfo.isDelay = true;
        gatheringInfo.curDelay = Gathering_Delay;
        isComplete = false;
        isgathering = false;
        isAllowGet = false;
    }

}
