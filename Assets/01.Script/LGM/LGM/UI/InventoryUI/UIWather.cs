using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWather : MonoBehaviour
{
    public Image icon;  //재해 아이콘
    public Text text;   //재해 이름

    [Header("[재해 이미지]")]
    public Dictionary<string, Sprite> dicWather;
    public Sprite[] disasterImage;
    public string[] disasterName;

    private void Awake()
    {
        dicWather = new Dictionary<string, Sprite>();

        for (int i = 0; i < disasterImage.Length; i++)
        {
            dicWather.Add(disasterName[i], disasterImage[i]);
        }

    }

    private void Start()
    {
        icon = transform.GetComponentInChildren<Image>();
        /*Debug.Log(dicWather.ContainsKey("맑음"));
        Debug.Log(dicWather.ContainsKey("폭우"));
        Debug.Log(dicWather.ContainsKey("태풍"));
        Debug.Log(dicWather.ContainsKey("지진"));
        Debug.Log(dicWather.ContainsKey("해일"));
        Debug.Log(dicWather.ContainsKey("폭염"));
        Debug.Log(dicWather.ContainsKey("분화"));*/
    }
   /* private void Update()
    {
        //현재 날씨에 따라 텍스트와 아이콘 변경
        switch(World.Instance.disaster.weatherInfo.weather)
        {
            case E_Disaster_Type.None:
                icon.sprite = disasterImage[0];
                text.text = disasterName[0];
                break;
            case E_Disaster_Type.Rain:
                icon.sprite = disasterImage[1];
                text.text = disasterName[1];
                break;
            case E_Disaster_Type.Typhoon:
                icon.sprite = disasterImage[2];
                text.text = disasterName[2];
                break;
            case E_Disaster_Type.Earthquake:
                icon.sprite = disasterImage[3];
                text.text = disasterName[3];
                break;
            case E_Disaster_Type.Tidalwaves:
                icon.sprite = disasterImage[4];
                text.text = disasterName[4];
                break;
            case E_Disaster_Type.Heatwave:
                icon.sprite = disasterImage[5];
                text.text = disasterName[5];
                break;
            case E_Disaster_Type.Eruption:
                icon.sprite = disasterImage[6];
                text.text = disasterName[6];
                break;
            default:
                icon.sprite = disasterImage[0];
                text.text = disasterName[0];
                break;
        }
    }*/

}
