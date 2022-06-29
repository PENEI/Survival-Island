using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : Singleton<World>
{
    [HideInInspector]
    public WorldTime worldTime;
    [HideInInspector]
    public Disaster disaster;       //³¯¾¾
    [HideInInspector]
    public IslandManager islandManager;       //¼¶°ü·Ã ¸Å´ÏÀú

    bool isInit;
    private void Awake()
    {
        Init();
    }

    protected override void SingletonInit()
    {
        Init();
    }

    void Init()
    {
        if(!isInit)
        {
            worldTime = GetComponent<WorldTime>();
            disaster = GetComponent<Disaster>();
            islandManager = GetComponent<IslandManager>();
            isInit = true;
        }
    }
}
