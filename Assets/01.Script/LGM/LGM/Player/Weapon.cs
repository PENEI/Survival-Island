using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ObjType
{
    None,
    Player,
    Enemy
}
public class Weapon : MonoBehaviour
{
    [HideInInspector]
    public EnemyInfo info;
    public ObjType type;
    private Dictionary<ObjType, string> _tag;

    private void Awake()
    {
        if (type == ObjType.Enemy)
        {
            info = transform.GetComponentInParent<EnemyInfo>();
        }
        _tag = new Dictionary<ObjType, string>();
        _tag.Add(ObjType.Player, "Monster");
        _tag.Add(ObjType.Enemy, "Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_tag[type])) 
        {
            if (type == ObjType.Player)
                UIManager.Instance.equipPanel.ReductionHand(); //무기 내구도 감소

            //Weapon을 갖고있는 오브젝트가 Enemy일때 
            else if (type == ObjType.Enemy) 
            {
                if (Random.Range(0, 100) < info.woundPercent)
                {
                    Player.Instance._Debuff.isDebuff.isWound = true;
                }
            }

            gameObject.SetActive(false);
        }
    }
}
