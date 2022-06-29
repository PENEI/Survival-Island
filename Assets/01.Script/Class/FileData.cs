using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

//파일 정보 싱글톤 클래스
public class FileData : SingletonClass<FileData>
{
    public bool isSaveLoad = false;  //세이브 파일 불러오는지 확인

    //저장 위치
    public string DebugPath = "./Assets/08.Text/" + "userdata" + ".xml";
    public string BuildPath = "/userdata" + ".dat";

    public string FilePath { get; set; }

    protected override void SingletonInit()
    {
#if UNITY_EDITOR
        FilePath = DebugPath;
#else
FilePath = Application.persistentDataPath + FileData.Instance.BuildPath;
#endif

        //Debug.Log(FilePath);
    }
}

//암호화 클래스
public static class DataEncrypt
{
    static RijndaelManaged m_Aes = new RijndaelManaged();

    static RijndaelManaged GetAESKeyInput(string key)
    {
        m_Aes.BlockSize = 128;
        m_Aes.KeySize = 128;
        m_Aes.Padding = PaddingMode.PKCS7;
        m_Aes.Mode = CipherMode.ECB;
        m_Aes.Key = Encoding.UTF8.GetBytes(key);

        return m_Aes;
    }

    //암호화
    public static string Encrypt(string value, string key)
    {
        RijndaelManaged aesval = GetAESKeyInput(key);           //키값 생성
        ICryptoTransform encrypt = aesval.CreateEncryptor();        //암호화 작업 변수

        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (CryptoStream cryptStream = 
                new CryptoStream(memoryStream, encrypt, CryptoStreamMode.Write))
            {
                byte[] text_bytes = Encoding.UTF8.GetBytes(value);
                cryptStream.Write(text_bytes, 0, text_bytes.Length);
            }

            byte[] encrypted = memoryStream.ToArray();
            return Convert.ToBase64String(encrypted);
        } 
    }

    //복호화
    public static string Decrypt(string value, string key)
    {
        RijndaelManaged aesval = GetAESKeyInput(key);               //키값 생성
        ICryptoTransform decryptor = aesval.CreateDecryptor();  //암호화 작업 변수

        byte[] encrypted = Convert.FromBase64String(value);
        byte[] planeText = new byte[encrypted.Length];

        using (MemoryStream memoryStream = new MemoryStream(encrypted))
        {
            using (CryptoStream cryptStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                cryptStream.Read(planeText, 0, planeText.Length);
            }
            return Encoding.UTF8.GetString(planeText); 
        }
    }

}

//빌드 정보 클래스
public class BuildInfo
{
    public int IncID;   //인스턴스 아이디
    public bool isComBuild; //건축 완
    public int BuildObjID;  //건축완료된 오브젝트 아이디
}

//채집 정보 클래스
public class GatheringInfo
{
    public int IncID;       //인스턴스 아이디
    public bool isCom;    //오브젝트 활성화 여부
    public bool isDelay;    //딜레이 확인
    public float curDelay;  //현재 딜레이 시간
}

//파일 저장 클래스
[System.Serializable]
public class DataInfo
{
    public int Cost;    //섬 코스트
    public int SaveCount;                   //남은 세이브 횟수
    public Vector3 PlayerPos;           //플레이어 위치
    public List<ItemInfo> InvenItemList;    //인벤토리
    public ItemInfo Tool;       //장비
    public ItemInfo Armor;

    public Status status;
    public E_Disaster_Type disasterType;
    public CTime Time;
    public CISDebuff Debuff;

    public List<GatheringInfo> gatheringInfoList;       //채집 오브젝트
    public List<GatheringInfo> delayInfoList;           //딜레이 있는 오브젝트
    public List<BuildInfo> buildInfoList;                   //건축 오브젝트


    public DataInfo() 
    {
        Tool = new ItemInfo();
        Armor = new ItemInfo();

        InvenItemList = new List<ItemInfo>();
        gatheringInfoList = new List<GatheringInfo>();
        delayInfoList = new List<GatheringInfo>();
        buildInfoList = new List<BuildInfo>();

        status = new Status();
        Time = new CTime();
        Debuff = new CISDebuff();
    }
}

//--------------------------

//스테이터스 구성
[System.Serializable]
public class SingleStatus
{
    public float maxStatus;             //스테이터스 최대치
    public float statusValue;           //현재 스테이터스
    public int cycleTime;               //감소 주기
    public float sleepRecoveryValue;    //수면 시 회복 감소 값
    [HideInInspector]
    public bool isZeroStatus;           //스테이터스가 0인지 체크
    public float[] minusValue;          //스테이터스 감소 치
}

//player 스테이터스
[System.Serializable]
public class Status
{
    public float normalAttackPower; //노말 공격력
    public float attackPower;   //공격력
    public float defensePower;  //방어력

    [Header("[스테이터스]")]
    public SingleStatus hydration;     //수분
    public SingleStatus hunger;        //공복
    public SingleStatus fatigue;       //피로
    public SingleStatus hp;            //체력

    //player의 스테이터스에 따라 스테이터스UI 이미지의 변화
    public static float PlayerStatusPercentage(SingleStatus status)
    {
        return status.statusValue / status.maxStatus;
    }
}

//시간 값
[System.Serializable]
public class CTime
{
    // 일 시 분 초를 초단위로 저장
    public float sTime;     //일 시 분
    public string ampm;     //true : 오후 || false : 오전
    public float multiply;  //시간 배속
    public bool isStar;
}

//디버프 활성화
[System.Serializable]
public class CISDebuff
{
    public bool isBodyache;
    public bool isCold;
    public bool isDehydration;
    public bool isFoodpoison;
    public bool isStun;
    public bool isSwim;
    public bool isWound;

}
