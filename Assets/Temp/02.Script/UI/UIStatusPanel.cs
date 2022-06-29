using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIStatusPanel : MonoBehaviour
{
    public Text hp;
    public Text hunger;
    public Text water;
    public Text fatigue;

    private Status status;

    void Start()
    {
        status = Player.Instance._Status.status;
    }

    void Update()
    {
        hp.text = (status.hp.statusValue + "/" + status.hp.maxStatus + "(%)");
        hunger.text = (status.hunger.statusValue + "/" + status.hunger.maxStatus + "(%)");
        water.text = (status.hydration.statusValue + "/" + status.hydration.maxStatus + "(%)");
        fatigue.text = (status.fatigue.statusValue + "/" + status.fatigue.maxStatus + "(%)");
    }
}
