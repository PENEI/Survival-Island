/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIStatusPanelText : MonoBehaviour
{
    public Text[] text;

    private void Awake()
    {
        Array.Resize<Text>(ref text, transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            text[i] = transform.GetChild(i).GetChild(1).GetComponent<Text>();
        }
    }
    private void Update()
    {
        text[0].text = Math.Truncate((Player.Instance._Status.Hp / Player.Instance._Status.maxHp * 100)).ToString() + "/100 (%)";
        text[1].text = Math.Truncate((Player.Instance._Status.Hunger / Player.Instance._Status.maxHunger * 100)).ToString() + "/100 (%)";
        text[2].text = Math.Truncate((Player.Instance._Status.Hydration / Player.Instance._Status.maxHydration * 100)).ToString() + "/100 (%)";
        text[3].text = Math.Truncate((Player.Instance._Status.Fatigue / Player.Instance._Status.maxFatigue * 100)).ToString() + "/100 (%)";
    }
}
*/