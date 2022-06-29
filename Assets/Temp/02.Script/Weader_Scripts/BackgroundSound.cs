using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip[] audioClip;
    public int moringTime;  // ��ħ�� ���۵Ǵ� �ð�
    public int nightTime;   // ���� ���۵Ǵ� �ð�
    private WorldTime time;

    public int randomTimeM = 0;     // ���� �Ҹ� ����(��)
    [Space]
    public int m;

    private void Start()
    {
        time = World.Instance.worldTime;

        //��ħ, �㿡 ���� ���� ���� 
        BackgroundSoundChange();
    }
    private void Update()
    {
        //��ħ, �㿡 ���� ���� ���� 
        BackgroundSoundChange();

        m = (int)time.GetTime("��");
    }
    private void BackgroundSoundChange()
    {
        //��ħ, �㿡 ���� ���� ���� 
        audio.clip = time.GetTime("��") > nightTime ||
            time.GetTime("��") < moringTime
            ? audioClip[1] : audioClip[0];

        if (!audio.isPlaying && ((int)time.GetTime("��")) == randomTimeM)         
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
