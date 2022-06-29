using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoEnding : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Player")
        {
            UIManager.Instance.CursorOnOff(true);
            Player.Instance.Control.SetAllowAll(false);
            SceneLoader.Instance.LoadGameScene("EndScene", false);

        }
    }

    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
