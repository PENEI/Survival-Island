using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//½½·ÔÆÐ³Î
public class SlotPanel : MonoBehaviour
{
    public List<Slot> SlotList;     //½½·Ô ¸®½ºÆ®

    void Awake()
    {
        //ÀÚ½Ä ½½·Ô °¡Á®¿È
        SlotList.AddRange(this.gameObject.GetComponentsInChildren<Slot>());
    }

}
