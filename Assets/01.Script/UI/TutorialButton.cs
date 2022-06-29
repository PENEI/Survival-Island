using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    [Header("튜토리얼 이미지")]
    public Sprite Img_Explain;
    public Button button { get; private set; }  //버튼

    private void Awake()
    {
        button = GetComponent<Button>();
    }
}
