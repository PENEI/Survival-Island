using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    [Header("Ʃ�丮�� �̹���")]
    public Sprite Img_Explain;
    public Button button { get; private set; }  //��ư

    private void Awake()
    {
        button = GetComponent<Button>();
    }
}
