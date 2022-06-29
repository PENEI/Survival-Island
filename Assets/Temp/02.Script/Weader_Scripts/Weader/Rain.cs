using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//폭우
public class Rain : Weader
{
    public GameObject rainParticle;

    private void Awake()
    {
        Weader_Awake();
    }
    private void Update()
    {
        //재해 발생 (StartDebuff가 true를 반환했으면 재해 실행)
        if (StartDebuff())
        {
            //처음 시작 시 실행
            if (!distroyOn)
            {
                distroyOn = StartDebuff();
                //-----------처음 실행---------------
                WeaderApplication(true);
                rainParticle.SetActive(true);
                //----------------------------------
            }
            //-----------실행되는 동안 실행---------------
            Debug.Log("\n재해 발생 중!");
            //-------------------------------------------
        }
        //재해 발생이 끝날 시 실행
        //재해가 발생하고 있지 않을 때 (StartDebuff가 false 반환)
        else if (!StartDebuff())
        {
            if (distroyOn)
            {
                distroyOn = StartDebuff();
                //----------재해 끝날 시 실행-------------
                rainParticle.SetActive(false);
                WeaderApplication(false);
                //---------------------------------------
            }
        }
    }

    private void OnDestroy()
    {
        //끝날 때 초기화가 되기전에 끝날 시 초기화
        if (distroyOn)
        {
            Debug.Log("삭제"); 
            if (rainParticle != null)
            {
                rainParticle.SetActive(false);
            }
            WeaderApplication(false);
        }
    }
}
