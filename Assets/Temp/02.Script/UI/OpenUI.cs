using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUI : MonoBehaviour
{
    public GameObject ui;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            if (ui.activeSelf)
                ui.SetActive(false);
            else if(!ui.activeSelf)
                ui.SetActive(true);
        }
    }
}
