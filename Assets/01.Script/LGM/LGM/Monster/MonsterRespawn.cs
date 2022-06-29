using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRespawn : MonoBehaviour
{
    public GameObject pf_Monster;
    private GameObject monster;
    public float respawnDelay;
    
    private void Awake()
    {
        pf_Monster.transform.position = transform.position;
        monster = Instantiate(pf_Monster);
        monster.transform.SetParent(transform.parent);
        
    }
    // Update is called once per frame
    void Update()
    {
        if (monster != null)
        {
            if (monster.activeSelf == false)
            {
                Destroy(monster);
                StartCoroutine(RespawnDelay());
            }
        }
    }
    IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        monster = Instantiate(pf_Monster);
        monster.transform.position = transform.position;
    }
}
