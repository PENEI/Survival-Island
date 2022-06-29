using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDayText : MonoBehaviour
{
    public Text text;

    private void Update()
    {
        text.text = ((int)World.Instance.worldTime.GetTime("¿œ")).ToString() + "¿œ";
    }
}
