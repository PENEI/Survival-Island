using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//지진
public class Earthquake : Weader
{
    private Camera mainCamera;

    //지진이 일어날 간격(h=시, m=분)
    public int h = 0;
    public int m = 0;

    public float shackingTime = 3f;    //지진 유지 시간

    private void Awake()
    {
        Weader_Awake();
        mainCamera = Camera.main;
        //랜덤 시간 설정
        RandomTimeSet();
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
                //침수 카운트 증가
                World.Instance.islandManager.cost += weaderInfo.closeCost;
                //----------------------------------
            }
            //-----------실행되는 동안 실행---------------
            //현재 시간이 지진 발생시간을 초과하거나 같을 경우 지진 발생
            if (((int)timer.GetTime("시") > h) ||
                ((int)timer.GetTime("시") == h && (int)timer.GetTime("분") >= m))
            {
                //지진 발생
                StartCoroutine(CameraShaking());
            }
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
                //행동 가능
                //Player.Instance.Control.isAllowCharMove = true;
                Player.Instance._Debuff.isDebuff.isStun = false;
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
            //행동 가능
            //Player.Instance.Control.isAllowCharMove = true;
            Player.Instance._Debuff.isDebuff.isStun = false;
        }
    }

    //카메라 흔들기
    public void CameraShake()
    {
        if(!World.Instance.disaster.audio.isPlaying)
        {
            World.Instance.disaster.audio.Play();
        }
        CameraManager.Instance.Impulse();
        //지진중 체크
        World.Instance.islandManager.isQuake = true;
        //카메라 흔들기
        float x = UnityEngine.Random.Range(-0.1f, 0.1f);
        float y = UnityEngine.Random.Range(-0.1f, 0.1f);
        mainCamera.transform.position =
            Vector3.Lerp(mainCamera.transform.position,
            new Vector3(mainCamera.transform.position.x + x,
            mainCamera.transform.position.y + y,
            mainCamera.transform.position.z),
            Time.deltaTime);
        //행동불가
        //Player.Instance.Control.isAllowCharMove = false;
        Player.Instance._Debuff.isDebuff.isStun = true;
    }

    //지진 발생 간격 설정
    public void RandomTimeSet()
    {
        //30~120분의 랜덤 값을 설정
        int randomTime = Random.Range(30, 120);

        //1시간 이상이면 현재시간에 랜덤한 시간(시, 분)을 더해 저장
        if (randomTime >= 60)
        {
            h = (int)timer.GetTime("시") + (randomTime / 60);
            m = (int)timer.GetTime("분") + (randomTime % 60);
        }
        //1시간 미만이면 현재 시간에 랜덤한 시간(분)을 더해 저장
        else
        {
            h = (int)timer.GetTime("시");
            m = (int)timer.GetTime("분") + randomTime;
        }
    }
    
    //지진
    private IEnumerator CameraShaking()
    {
        //Player.Instance.Control.SetAllowAll(false);
        //지진 발생
        CameraShake();
        
        yield return new WaitForSeconds(shackingTime);
        //Player.Instance.Control.SetAllowAll(true);
        //Player.Instance._Debuff.isStun = false;
        World.Instance.islandManager.isQuake = false;
        //다음 지진 발생 시간 설정
        RandomTimeSet();
    }
}
