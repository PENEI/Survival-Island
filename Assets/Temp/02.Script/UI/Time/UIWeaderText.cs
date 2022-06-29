using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaderText : MonoBehaviour
{
    public Image icon;
    public Text text;

    private void Update()
    {
        text.text = World.Instance.disaster.weader.weaderInfo._name;
        icon.sprite = World.Instance.disaster.weader.weaderInfo.icon;
    }
}
