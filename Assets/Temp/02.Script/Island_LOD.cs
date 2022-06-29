using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island_LOD : MonoBehaviour
{

    public GameObject parentObj;
    [Header("[ Cost ]")]
    public int line;        //���� ����
    [Header("[ ���� ��ġ�� �ڽ�Ʈ �� ]")]
    public int minCost;     //�ּ� �ڽ�Ʈ
    public int maxCost;     //�ִ� �ڽ�Ʈ

    public int cost;        //���� �ڽ�Ʈ

    public bool isPlayerSettle;    // �÷��̾ ���� �����ߴ��� üũ

    private void Start()
    {
        cost = Random.Range(minCost, maxCost + 1);
        if (line == 5)
        {
            cost = 10000;
        }
    }
    private void Update()
    {
        //���� manager�� �ڽ�Ʈ ���� ������ �� ��Ȱ��ȭ
        if (cost <= World.Instance.islandManager.cost)
        {
            parentObj.SetActive(false);
        }
    }
}
