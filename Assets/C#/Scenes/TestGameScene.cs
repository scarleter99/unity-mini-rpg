﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.TestGameScene;

        // DataManager test
        Dictionary<int, Data.Stat> statDic = Managers.DataMng.StatDict;
        
        gameObject.GetOrAddComponent<CursorController>();
        
        GameObject player = Managers.GameMng.Spawn(Define.WorldObject.Player, "UnityChan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        GameObject go = new GameObject { name = "SpawningPool" };
        SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        pool.SetKeepMonsterCount(5);
    }
    
    public override void Clear()
    {
        Debug.Log("GameScene Clear!");
    }
}