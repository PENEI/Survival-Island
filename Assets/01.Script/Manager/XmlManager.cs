using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class XmlManager : Singleton<XmlManager>
{
    [Header("���� ���� Ƚ��")]
    public int SaveCount;

    int curcount;       //���� ���� ���� Ƚ��

    string FilePath;        //���� ��ġ
    public DataInfo dataInfo { get; private set; }
    string aescode = "54E9968CF96EBACE";        //��ȣȭ �ڵ�
            
    bool isencrypt = true;     //��ȣȭ �ϴ��� Ȯ��

    private void Awake()
    {

    }

    protected override void SingletonInit()
    {
        curcount = SaveCount;       
        //���� ��ġ
        FilePath = FileData.Instance.FilePath;
        //�����ͷε�,���� Ȯ��  �ҷ�����
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

        UIManager.Instance.pausePanel.SetSaveCountText(curcount);       //ī��Ʈ �ؽ�Ʈ ����
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
    /// ����
    /// </summary>
    /// <returns>���� ���� Ƚ��</returns>
    public int Save()
    {
        //0�̸� ����X
        if (curcount <= 0)
            return curcount;
        --curcount;     //�����Ҷ� ���� ����
        UIManager.Instance.pausePanel.SetSaveCountText(curcount);   //���� ī��Ʈ ����

        dataInfo.SaveCount = curcount;          //���� ī��Ʈ
        dataInfo.PlayerPos = Player.Instance.transform.position;        //�÷��̾� ��ġ
        dataInfo.Cost = World.Instance.islandManager.cost;

        //�������ͽ�
        dataInfo.status = Player.Instance._Status.status;       //�������ͽ�
        dataInfo.Debuff = Player.Instance._Debuff.isDebuff;    //�����
        dataInfo.disasterType = World.Instance.disaster.eWeader;    //����
        dataInfo.Time = World.Instance.worldTime.cTime;     //�ð�

        //����
        using (StreamWriter wr = new StreamWriter(FilePath))
        {
            string datastr = SaveXMLSerialization(dataInfo);            //�ø��������
            if(isencrypt)
                datastr = DataEncrypt.Encrypt(datastr, aescode); //��ȣȭ
            wr.Write(datastr);                                                 //����
        }
        return curcount;
    }

    //XML�б�
    void Load()
    {
        //�ε�
        using (StreamReader sr = new StreamReader(FilePath))
        {
            string datastr = sr.ReadToEnd();                                  //�����б�
            if (isencrypt)
                datastr = DataEncrypt.Decrypt(datastr, aescode);  //��ȣȭ
            dataInfo = LoadXMLDeserialization<DataInfo>(datastr);       //�ø��� ������
        }

        SaveCountLoad(dataInfo.SaveCount);  //ī��Ʈ ����
        PlayerLoad(dataInfo.PlayerPos);         //�÷��̾� ��ġ ����
        World.Instance.islandManager.cost = dataInfo.Cost;

        Player.Instance._Status.status = dataInfo.status;  //�������ͽ�
        Player.Instance._Debuff.isDebuff = dataInfo.Debuff; //�����
        World.Instance.disaster.CreateDistaster(dataInfo.disasterType); //����
        World.Instance.worldTime.cTime = dataInfo.Time;     //�ð�
    }

    //���� ī��Ʈ
    void SaveCountLoad(int count)
    {
        curcount = count;
    }

    //�÷��̾�
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
