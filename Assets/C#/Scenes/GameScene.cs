using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.GameScene;

        gameObject.GetOrAddComponent<CursorController>();
        
        // UIManager test
        Managers.UIMng.ShowSceneUI<UI_Inven>();

        // DataManager test
        Dictionary<int, Data.Stat> statDic = Managers.DataMng.StatDic;
    }
    
    public override void Clear()
    {
        Debug.Log("GameScene Clear!");
    }
}