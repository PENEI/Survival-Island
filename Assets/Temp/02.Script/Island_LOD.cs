using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island_LOD : MonoBehaviour
{

    public GameObject parentObj;
    [Header("[ Cost ]")]
    public int line;        //섬의 라인
    [Header("[ 섬이 닫치는 코스트 값 ]")]
    public int minCost;     //최소 코스트
    public int maxCost;     //최대 코스트

    public int cost;        //섬의 코스트

    public bool isPlayerSettle;    // 플레이어가 섬에 정착했는지 체크

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
        //만약 manager의 코스트 보다 낮으면 섬 비활성화
        if (cost <= World.Instance.islandManager.cost)
        {
            parentObj.SetActive(false);
        }
    }
}
