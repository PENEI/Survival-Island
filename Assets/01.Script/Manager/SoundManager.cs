using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : SingletonDontDestroy<SoundManager>
{
    [SerializeField]
    string[] mixserNameArr;
    [SerializeField]
    string[] optionNameArr;

    string audioClip_Path = "Sound";
    string audioClip_Player_Path = "Sound/Player";
    string audioClip_Monster_Path = "Sound/Monster";
    string audioClip_Basic_Path = "Sound/Basic";
    string audioClip_Weather_Path = "Sound/Weather";

    List<AudioSource> audioSourceList;                          //오디오 소스 리스트
    Dictionary<int, AudioClip> AudioClip_ID_Dic;                //오디오 딕셔너리 (아이디, 오디오 클립)
    Dictionary<string, AudioClip> AudioClip_Name_Dic;
    Dictionary<E_Effect_Player, AudioClip> AudioClip_Player_Dic;    
    Dictionary<E_Effect_Monster, AudioClip> AudioClip_Monster_Dic;
    Dictionary<E_BGM_Basic, AudioClip> AudioClip_Basic_Dic;
    Dictionary<E_BGM_Wather, AudioClip> AudioClip_Weather_Dic;

    bool isinit;
    float sound;


    [SerializeField]
    AudioMixer m_audioMixer;

    void Awake()
    {
        if (m_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        if(!isinit)
        {
            SingletonDontDestroyInit();
        }
    }

    private void Start()
    {
        if (!audioSourceList[(int)E_SoundType.BGM].isPlaying)
        {
            SettingSound();
            PlaySound(E_SoundType.BGM, E_BGM_Basic.BGM);
        }
    }

    void SettingSound()
    {
        for (int i = 0; i < mixserNameArr.Length; i++)
        {
            int count = Mathf.Clamp(PlayerPrefs.GetInt(optionNameArr[i], 100), 0, 100);
            sound = count;
            SetSound(mixserNameArr[i], count);
        }
    }

    public void SetSound(string _name, int count)
    {
        float c = count * 0.01f;
        if (count == 0)
            c = 0.0001f * 0.01f;

        m_audioMixer.SetFloat(_name, Mathf.Log10(c) * 20);

    }


    protected override void SingletonDontDestroyInit()
    {
        base.SingletonDontDestroyInit();
        isinit = true;



        audioSourceList = new List<AudioSource>();
        AudioClip_ID_Dic = new Dictionary<int, AudioClip>();
        AudioClip_Name_Dic = new Dictionary<string, AudioClip>();
        AudioClip_Player_Dic = new Dictionary<E_Effect_Player, AudioClip>();
        AudioClip_Monster_Dic = new Dictionary<E_Effect_Monster, AudioClip>();
        AudioClip_Basic_Dic = new Dictionary<E_BGM_Basic, AudioClip>();
        AudioClip_Weather_Dic = new Dictionary<E_BGM_Wather, AudioClip>();

        //오디오 소스
        AudioSource[] audioarr = GetComponentsInChildren<AudioSource>();
        audioSourceList.AddRange(audioarr);
        if (audioSourceList.Count < (int)E_SoundType.Max)
        {
            for (int i = audioSourceList.Count; i < (int)E_SoundType.Max; i++)
            {
                GameObject temp = new GameObject(string.Format("audioSource{0}", i), typeof(AudioSource));
                temp.transform.parent = this.transform;
                audioSourceList.Add(temp.GetComponent<AudioSource>());
            }
        }

        //오디오 클립
        AudioClip[] clipArr = Resources.LoadAll<AudioClip>(audioClip_Path);
        foreach (AudioClip item in clipArr)
        {
            string[] strarr = item.name.Split('_');

            if (int.TryParse(strarr[0], out int id))
            {
                AudioClip_ID_Dic.Add(id, item);
            }
            AudioClip_Name_Dic.Add(item.name, item);
        }

        //플레이어 클립
        clipArr = Resources.LoadAll<AudioClip>(audioClip_Player_Path);
        for (int i = 0; i < clipArr.Length; i++)
        {
            if(i < (int)E_Effect_Player.Max)
            {
                AudioClip_Player_Dic.Add((E_Effect_Player)i, clipArr[i]);
            }
        }

        //몬스터
        clipArr = Resources.LoadAll<AudioClip>(audioClip_Monster_Path);
        for (int i = 0; i < clipArr.Length; i++)
        {
            if (i < (int)E_Effect_Monster.Max)
            {
                AudioClip_Monster_Dic.Add((E_Effect_Monster)i, clipArr[i]);
            }
        }

        //기본 배경음
        clipArr = Resources.LoadAll<AudioClip>(audioClip_Basic_Path);
        for (int i = 0; i < clipArr.Length; i++)
        {
            if (i < (int)E_BGM_Basic.Max)
            {
                AudioClip_Basic_Dic.Add((E_BGM_Basic)i, clipArr[i]);
            }
        }

        //날씨
        clipArr = Resources.LoadAll<AudioClip>(audioClip_Weather_Path);
        for (int i = 0; i < clipArr.Length; i++)
        {
            if (i < (int)E_BGM_Wather.Max)
            {
                AudioClip_Weather_Dic.Add((E_BGM_Wather)i, clipArr[i]);
            }
        }

        audioSourceList[(int)E_SoundType.BGM].loop = true;
        audioSourceList[(int)E_SoundType.Effect_NoOne].loop = true;
        audioSourceList[(int)E_SoundType.Effect_Swim].loop = true;

        foreach (AudioSource item in audioSourceList)
        {

        }
    }

    /// <summary>
    /// 오디오 재생
    /// </summary>
    /// <param name="audio">오디오 소스</param>
    /// <param name="_id">아이디</param>
    public void PlaySound(AudioSource audio, int _id)
    {
        if (AudioClip_ID_Dic.TryGetValue(_id, out AudioClip clip))
        {
            audio.clip = clip;
            audio.Play();
        }
    }

    public void PlaySoundOneShot(AudioSource audio, E_BGM_Wather _id)
    {
        if (AudioClip_Weather_Dic.TryGetValue(_id, out AudioClip clip))
        {
            audio.PlayOneShot(clip);
        }
    }


    public void PlaySoundOneShot(AudioSource audio, E_Effect_Player _id)
    {
        if (AudioClip_Player_Dic.TryGetValue(_id, out AudioClip clip))
        {
            audio.PlayOneShot(clip);
        }
    }

    public void PlaySoundOneShot(AudioSource audio, E_Effect_Monster _id)
    {
        if (AudioClip_Monster_Dic.TryGetValue(_id, out AudioClip clip))
        {
            audio.PlayOneShot(clip);
        }
    }


    public void PlaySound(AudioSource audio, E_BGM_Wather _id)
    {
        if (AudioClip_Weather_Dic.TryGetValue(_id, out AudioClip clip))
        { 
            audio.clip = clip;
            audio.Play();
        }
    }


    public void PlaySound(AudioSource audio, E_Effect_Player _id)
    {
        if (AudioClip_Player_Dic.TryGetValue(_id, out AudioClip clip))
        {
            audio.clip = clip;
            audio.Play();
        }
    }

    public void PlaySound(AudioSource audio, E_Effect_Monster _id)
    {
        if (AudioClip_Monster_Dic.TryGetValue(_id, out AudioClip clip))
        {
            audio.clip = clip;
            audio.Play();
        }
    }

    /// <summary>
    /// 오디오 재생
    /// </summary>
    /// <param name="type">오디오 소스 타입</param>
    /// <param name="_id"></param>
    public void PlaySound(E_SoundType type, E_Effect_Player _id)
    {
        if (AudioClip_Player_Dic.TryGetValue(_id, out AudioClip clip))
        {
            switch (type)
            {
                case E_SoundType.Effect:
                    audioSourceList[(int)type].PlayOneShot(clip);
                    break;
                case E_SoundType.Effect_NoOne:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
                case E_SoundType.BGM_Weather:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
                case E_SoundType.BGM:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
                case E_SoundType.Effect_Swim:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;

            }
        }
    }

    /// <summary>
    /// 오디오 재생
    /// </summary>
    /// <param name="type">오디오 소스 타입</param>
    /// <param name="_id"></param>
    public void PlaySound(E_SoundType type, E_Effect_Monster _id)
    {
        if (AudioClip_Monster_Dic.TryGetValue(_id, out AudioClip clip))
        {
            switch (type)
            {
                case E_SoundType.Effect:
                    audioSourceList[(int)type].PlayOneShot(clip);
                    break;
                case E_SoundType.Effect_NoOne:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
                case E_SoundType.BGM_Weather:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
                case E_SoundType.BGM:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
            }
        }
    }
    public void PlaySound(E_SoundType type, E_BGM_Basic _id)
    {
        if (AudioClip_Basic_Dic.TryGetValue(_id, out AudioClip clip))
        {
            switch (type)
            {
                case E_SoundType.Effect:
                    audioSourceList[(int)type].PlayOneShot(clip);
                    break;
                case E_SoundType.Effect_NoOne:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
                case E_SoundType.BGM_Weather:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
                case E_SoundType.BGM:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
            }
        }
    }
    public void PlaySound(E_SoundType type, E_BGM_Wather _id)
    {
        if (AudioClip_Weather_Dic.TryGetValue(_id, out AudioClip clip))
        {
            switch (type)
            {
                case E_SoundType.Effect:
                    audioSourceList[(int)type].PlayOneShot(clip);
                    break;
                case E_SoundType.Effect_NoOne:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
                case E_SoundType.BGM_Weather:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
                case E_SoundType.BGM:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
            }
        }
    }


    public void PlaySound(E_SoundType type, int _id)
    {
        if (AudioClip_ID_Dic.TryGetValue(_id, out AudioClip clip))
        {
            //Debug.LogFormat("{0}재생", _id);

            switch (type)
            {
                case E_SoundType.Effect:
                    audioSourceList[(int)type].PlayOneShot(clip);
                    break;
                case E_SoundType.Effect_NoOne:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
                case E_SoundType.BGM_Weather:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
                case E_SoundType.BGM:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
            }
        }
    }

    public void PlaySound(E_SoundType type, string _id)
    {
        if (AudioClip_Name_Dic.TryGetValue(_id, out AudioClip clip))
        {
            switch (type)
            {
                case E_SoundType.Effect:
                    audioSourceList[(int)type].PlayOneShot(clip);
                    break;
                case E_SoundType.Effect_NoOne:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
                case E_SoundType.BGM_Weather:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
                case E_SoundType.BGM:
                    audioSourceList[(int)type].clip = clip;
                    audioSourceList[(int)type].Play();
                    break;
            }
        }
    }

    /// <summary>
    /// 오디오 재생 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="_id"></param>
    /// <param name="_isloop">루프 확인</param>
    public void PlaySound(E_SoundType type, int _id, bool _isloop)
    {
        if (AudioClip_ID_Dic.TryGetValue(_id, out AudioClip clip))
        {
            audioSourceList[(int)type].loop = _isloop;
            audioSourceList[(int)type].clip = clip;
            audioSourceList[(int)type].Play();
        }
    }

    /// <summary>
    /// 오디오 재생 설정 지점
    /// </summary>
    /// <param name="_id">오디오 아이디</param>
    /// <param name="_pos">위치</param>
    public void PlaySound(int _id, Vector3 _pos)
    {
        if (AudioClip_ID_Dic.TryGetValue(_id, out AudioClip clip))
        {
            AudioSource.PlayClipAtPoint(clip, _pos);
        }
    }

    /// <summary>
    /// 오디오 종료
    /// </summary>
    /// <param name="type"></param>
    public void StopSound(E_SoundType type)
    {
        audioSourceList[(int)type].Stop();
    }
}
