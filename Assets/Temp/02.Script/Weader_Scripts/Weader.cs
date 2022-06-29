using UnityEngine;
using System.Collections;
//����
public class Weader : MonoBehaviour
{
    protected WorldTime timer;
    protected PlayerStatus status;     //�÷��̾� �������ͽ�
    public WeaderInfo weaderInfo;   //������ �����ϴ� ����

    public int starth;
    public int startm;
    public int endh;
    public int endm;

    public bool distroyOn;          //���� ���� ����

    [HideInInspector]
    public AudioSource audio;
    

    //WeaderŬ���� �ʱ�ȭ
    public void Weader_Awake()
    {
        timer = GetComponent<WorldTime>();
        status = Player.Instance._Status;
        audio = GameObject.Find("Canvas").GetComponent<AudioSource>();
        
    }
    
    //Weader Ŭ���� ���� �� ����
    public void Weader_Deastroy()
    {
        audio.clip = null;
    }

    //�������ͽ� ���� ���� �� ����
    public void VariationValueStatus(SingleStatus status, float value)
    {
        status.minusValue[1] += value;
    }

    //����� ���� �� ���������� ����
    public bool StartDebuff()
    {
        //���� �ð��� ���� ���� �ð����� ũ�ų� ����
        //������ �ð����� ���� �� true
        //���� �ð��� ���� ���� �ð�(h,m)���� ũ�ų�,
        //���� �ð��� ���� ������ �ð�(h,m) ���� ���� �� true 
        if ((starth < (int)timer.GetTime("��") && 
            endh > (int)timer.GetTime("��")) ||
            ((starth == (int)timer.GetTime("��") && 
            startm <= (int)timer.GetTime("��")))||
            (endh == (int)timer.GetTime("��") && 
            endm >= (int)timer.GetTime("��")))
        {
            return true;
        }
        return false;
    }

    //���� ȿ�� ����
    public void WeaderApplication(bool tf, SingleStatus status = null)
    {
        //�� ħ��
        if (weaderInfo.closeCost != 0)
        {
            World.Instance.islandManager.cost += weaderInfo.closeCost;
        }

        //�������ͽ� ���� ��
        if (weaderInfo.value != 0)
        {
            //Ȱ��ȭ �� value��ŭ �������ͽ� ���� �� ���� �ƴ� �� ����
            VariationValueStatus(status, (tf ? weaderInfo.value : (-weaderInfo.value)));
        }

        //ī�޶� ����
        if (weaderInfo.fxFilter != null)
        {
            Camera.main.GetComponent<AmplifyColorBase>().LutTexture = tf ? weaderInfo.fxFilter : null;
        }

        // ���� �Ҹ� ����
        if (tf && weaderInfo.audio != null)
        {
            audio.clip = weaderInfo.audio;
            
        }
        else if (!tf || weaderInfo.audio == null)
        {
            audio.clip = null;
        }

        //�����
        if (tf && weaderInfo.debuff != null) 
        {
            //��� ����� ����
            for (int i = 0; i < weaderInfo.debuff.Length; i++)
            {
                switch(weaderInfo.debuff[i].type)
                {
                    case E_Debuff_Type.Bodyache:
                        Player.Instance._Debuff.isDebuff.isBodyache = true;
                        break;
                    case E_Debuff_Type.Cold:
                        Player.Instance._Debuff.isDebuff.isCold = true;
                        break;
                    case E_Debuff_Type.Dehydration:
                        Player.Instance._Debuff.isDebuff.isDehydration = true;
                        break;
                    case E_Debuff_Type.Foodpoison:
                        Player.Instance._Debuff.isDebuff.isFoodpoison = true;
                        break;
                    case E_Debuff_Type.Swim:
                        Player.Instance._Debuff.isDebuff.isSwim = true;
                        break;
                    case E_Debuff_Type.Wound:
                        Player.Instance._Debuff.isDebuff.isWound = true;
                        break;
                }
            }
        }
    }
}
