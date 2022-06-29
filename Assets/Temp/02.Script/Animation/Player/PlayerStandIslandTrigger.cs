using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandIslandTrigger : MonoBehaviour
{
    //[HideInInspector]
    public Island_LOD island;
    //[HideInInspector]
    public GameObject islandObj;


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Island"))
        {
            if (other.transform.GetComponent<Island_LOD>() != null)
            {
                islandObj = other.transform.parent.gameObject;
                island = other.transform.GetComponent<Island_LOD>();
                island.isPlayerSettle = true;
            }
        }
    }
    private void Update()
    {
        if (islandObj != null && !islandObj.activeSelf) 
        {
            UIManager.Instance.GameOver.SetGameOver("DRAWN");
            islandObj = null;
            island.isPlayerSettle = false;
            island = null;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Island"))
        {
            islandObj = null;
            island.isPlayerSettle = false;
            island = null;
        }
    }

}
