using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentScript : MonoBehaviour, ActionObj
{

    public bool IsAction()
    {
        return true;
    }

    public void Action()
    {
        UIManager.Instance.SetActiveUI(UIManager.Instance.SleepUI.gameObject, true);
    }

    public E_InteractionType GetInteractionType()
    {
        return E_InteractionType.Bed;
    }
}
