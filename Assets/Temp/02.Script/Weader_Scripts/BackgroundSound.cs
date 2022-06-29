using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip[] audioClip;
    public int moringTime;  // 아침이 시작되는 시간
    public int nightTime;   // 밤이 시작되는 시간
    private WorldTime time;

    public int randomTimeM = 0;     // 다음 소리 간격(초)
    [Space]
    public int m;

    private void Start()
    {
        time = World.Instance.worldTime;

        //아침, 밤에 따라 사운드 변경 
        BackgroundSoundChange();
    }
    private void Update()
    {
        //아침, 밤에 따라 사운드 변경 
        BackgroundSoundChange();

        m = (int)time.GetTime("분");
    }
    private void BackgroundSoundChange()
    {
        //아침, 밤에 따라 사운드 변경 
        audio.clip = time.GetTime("시") > nightTime ||
            time.GetTime("시") < moringTime
            ? audioClip[1] : audioClip[0];

        if (!audio.isPlaying && ((int)time.GetTime("분")) == randomTimeM)         
        {
            audio.Play();
            randomTimeM = Random.Range(randomTimeM + 1, randomTimeM + 11);
            if (randomTimeM >= 60) 
            {
                randomTimeM = 0;
            }
        }
    }
}
