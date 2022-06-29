using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UITimeText : MonoBehaviour
{
    public Text text;
    public Image icon;
    public Sprite[] ampmIcon;
    
    private void Update()
    {
        text.text =
            World.Instance.worldTime.cTime.ampm + " "
        + (int)World.Instance.worldTime.GetTime("시") + "시 "
        + (int)World.Instance.worldTime.GetTime("분") + "분 ";

        icon.sprite = World.Instance.worldTime.GetTime("시") < 20 && 
            World.Instance.worldTime.GetTime("시") >= 8
            ? ampmIcon[0] : ampmIcon[1];
    }
}
