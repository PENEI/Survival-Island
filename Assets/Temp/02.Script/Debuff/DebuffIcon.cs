using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DebuffIcon : MonoBehaviour
{
    [Header("[이미지]")]
    public Image[] image;
    [Header("교체 이미지")]
    public Sprite[] changeImage;

    private Sprite[] tempImage;


    private void Awake()
    {
        Array.Resize<Sprite>(ref tempImage, image.Length);
        for (int i = 0; i < image.Length; i++)
        {
            tempImage[i] = image[i].sprite;
        }
    }

    private void Update()
    {
        Debuff_Icon_Change(ref image[0], tempImage[0], changeImage[0], Player.Instance._Debuff.isDebuff.isBodyache);
        Debuff_Icon_Change(ref image[1], tempImage[1], changeImage[1], Player.Instance._Debuff.isDebuff.isCold);
        Debuff_Icon_Change(ref image[2], tempImage[2], changeImage[2], Player.Instance._Debuff.isDebuff.isDehydration);
        Debuff_Icon_Change(ref image[3], tempImage[3], changeImage[3], Player.Instance._Debuff.isDebuff.isFoodpoison);
        Debuff_Icon_Change(ref image[4], tempImage[4], changeImage[4], Player.Instance._Debuff.isDebuff.isStun);
        Debuff_Icon_Change(ref image[5], tempImage[5], changeImage[5], Player.Instance._Debuff.isDebuff.isWound);
    }

    //tf가 true면 활성화 이미지/flase면 비활성화 이미지 교채
    private void Debuff_Icon_Change(ref Image _sprite, Sprite onSprite,Sprite offSprite,bool tf)
    {
        _sprite.sprite = tf ? onSprite : offSprite;
    }

}
