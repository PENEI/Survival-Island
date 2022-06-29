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

//���� ���� �̱��� Ŭ����
public class FileData : SingletonClass<FileData>
{
    public bool isSaveLoad = false;  //���̺� ���� �ҷ������� Ȯ��

    //���� ��ġ
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

//��ȣȭ Ŭ����
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

    //��ȣȭ
    public static string Encrypt(string value, string key)
    {
        RijndaelManaged aesval = GetAESKeyInput(key);           //Ű�� ����
        ICryptoTransform encrypt = aesval.CreateEncryptor();        //��ȣȭ �۾� ����

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

    //��ȣȭ
    public static string Decrypt(string value, string key)
    {
        RijndaelManaged aesval = GetAESKeyInput(key);               //Ű�� ����
        ICryptoTransform decryptor = aesval.CreateDecryptor();  //��ȣȭ �۾� ����

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

//���� ���� Ŭ����
public class BuildInfo
{
    public int IncID;   //�ν��Ͻ� ���̵�
    public bool isComBuild; //���� ��
    public int BuildObjID;  //����Ϸ�� ������Ʈ ���̵�
}

//ä�� ���� Ŭ����
public class GatheringInfo
{
    public int IncID;       //�ν��Ͻ� ���̵�
    public bool isCom;    //������Ʈ Ȱ��ȭ ����
    public bool isDelay;    //������ Ȯ��
    public float curDelay;  //���� ������ �ð�
}

//���� ���� Ŭ����
[System.Serializable]
public class DataInfo
{
    public int Cost;    //�� �ڽ�Ʈ
    public int SaveCount;                   //���� ���̺� Ƚ��
    public Vector3 PlayerPos;           //�÷��̾� ��ġ
    public List<ItemInfo> InvenItemList;    //�κ��丮
    public ItemInfo Tool;       //���
    public ItemInfo Armor;

    public Status status;
    public E_Disaster_Type disasterType;
    public CTime Time;
    public CISDebuff Debuff;

    public List<GatheringInfo> gatheringInfoList;       //ä�� ������Ʈ
    public List<GatheringInfo> delayInfoList;           //������ �ִ� ������Ʈ
    public List<BuildInfo> buildInfoList;                   //���� ������Ʈ


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

//�������ͽ� ����
[System.Serializable]
public class SingleStatus
{
    public float maxStatus;             //�������ͽ� �ִ�ġ
    public float statusValue;           //���� �������ͽ�
    public int cycleTime;               //���� �ֱ�
    public float sleepRecoveryValue;    //���� �� ȸ�� ���� ��
    [HideInInspector]
    public bool isZeroStatus;           //�������ͽ��� 0���� üũ
    public float[] minusValue;          //�������ͽ� ���� ġ
}

//player �������ͽ�
[System.Serializable]
public class Status
{
    public float normalAttackPower; //�븻 ���ݷ�
    public float attackPower;   //���ݷ�
    public float defensePower;  //����

    [Header("[�������ͽ�]")]
    public SingleStatus hydration;     //����
    public SingleStatus hunger;        //����
    public SingleStatus fatigue;       //�Ƿ�
    public SingleStatus hp;            //ü��

    //player�� �������ͽ��� ���� �������ͽ�UI �̹����� ��ȭ
    public static float PlayerStatusPercentage(SingleStatus status)
    {
        return status.statusValue / status.maxStatus;
    }
}

//�ð� ��
[System.Serializable]
public class CTime
{
    // �� �� �� �ʸ� �ʴ����� ����
    public float sTime;     //�� �� ��
    public string ampm;     //true : ���� || false : ����
    public float multiply;  //�ð� ���
    public bool isStar;
}

//����� Ȱ��ȭ
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
