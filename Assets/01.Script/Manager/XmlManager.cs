using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class XmlManager : Singleton<XmlManager>
{
    [Header("저장 남은 횟수")]
    public int SaveCount;

    int curcount;       //현재 남은 저장 횟수

    string FilePath;        //파일 위치
    public DataInfo dataInfo { get; private set; }
    string aescode = "54E9968CF96EBACE";        //암호화 코드
            
    bool isencrypt = true;     //암호화 하는지 확인

    private void Awake()
    {

    }

    protected override void SingletonInit()
    {
        curcount = SaveCount;       
        //파일 위치
        FilePath = FileData.Instance.FilePath;
        //데이터로드,파일 확인  불러오기
        FileInfo fileInfo = new FileInfo(FilePath);
        if (FileData.Instance.isSaveLoad
            && fileInfo.Exists)
        {
            Load();
        }
        else
        {
            dataInfo = new DataInfo();
        }

        UIManager.Instance.pausePanel.SetSaveCountText(curcount);       //카운트 텍스트 세팅
    }

    public void DeleteFile()
    {
        FileInfo fileInfo = new FileInfo(FilePath);
        if (fileInfo.Exists)
        {
            File.Delete(FilePath);
        }
    }

    /// <summary>
    /// 저장
    /// </summary>
    /// <returns>남은 저장 횟수</returns>
    public int Save()
    {
        //0이면 저장X
        if (curcount <= 0)
            return curcount;
        --curcount;     //저장할때 마다 감소
        UIManager.Instance.pausePanel.SetSaveCountText(curcount);   //저장 카운트 세팅

        dataInfo.SaveCount = curcount;          //저장 카운트
        dataInfo.PlayerPos = Player.Instance.transform.position;        //플레이어 위치
        dataInfo.Cost = World.Instance.islandManager.cost;

        //스테이터스
        dataInfo.status = Player.Instance._Status.status;       //스테이터스
        dataInfo.Debuff = Player.Instance._Debuff.isDebuff;    //디버프
        dataInfo.disasterType = World.Instance.disaster.eWeader;    //날씨
        dataInfo.Time = World.Instance.worldTime.cTime;     //시간

        //저장
        using (StreamWriter wr = new StreamWriter(FilePath))
        {
            string datastr = SaveXMLSerialization(dataInfo);            //시리얼라이즈
            if(isencrypt)
                datastr = DataEncrypt.Encrypt(datastr, aescode); //암호화
            wr.Write(datastr);                                                 //저장
        }
        return curcount;
    }

    //XML읽기
    void Load()
    {
        //로드
        using (StreamReader sr = new StreamReader(FilePath))
        {
            string datastr = sr.ReadToEnd();                                  //파일읽기
            if (isencrypt)
                datastr = DataEncrypt.Decrypt(datastr, aescode);  //복호화
            dataInfo = LoadXMLDeserialization<DataInfo>(datastr);       //시리얼 라이즈
        }

        SaveCountLoad(dataInfo.SaveCount);  //카운트 세팅
        PlayerLoad(dataInfo.PlayerPos);         //플레이어 위치 세팅
        World.Instance.islandManager.cost = dataInfo.Cost;

        Player.Instance._Status.status = dataInfo.status;  //스테이터스
        Player.Instance._Debuff.isDebuff = dataInfo.Debuff; //디버프
        World.Instance.disaster.CreateDistaster(dataInfo.disasterType); //날씨
        World.Instance.worldTime.cTime = dataInfo.Time;     //시간
    }

    //저장 카운트
    void SaveCountLoad(int count)
    {
        curcount = count;
    }

    //플레이어
    void PlayerLoad( Vector3 _vec)
    {
        Player.Instance.transform.position = _vec;
    }

    string SaveXMLSerialization<T>(T data)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        using (StringWriter textWriter = new StringWriter())
        {
            xmlSerializer.Serialize(textWriter, data);
            return textWriter.ToString();
        }
    }
    T LoadXMLDeserialization<T>(string text)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        using (StringReader textReader = new StringReader(text))
        {
            return (T)xmlSerializer.Deserialize(textReader);
        }
    }
}
