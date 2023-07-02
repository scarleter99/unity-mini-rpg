using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.GameScene;
        
        // UIManager test
        GameManager.UIMng.ShowSceneUI<UI_Inven>();
        
        // ResourceManager test
        for (int i = 0; i < 2; i++)
            GameManager.ResourceMng.Instantiate("Cube");
        
        // DataManager test
        Dictionary<int, Stat> statDic = GameManager.DataMng.StatDic;
    }
    
    public override void Clear()
    {
        Debug.Log("GameScene Clear!");
    }
}