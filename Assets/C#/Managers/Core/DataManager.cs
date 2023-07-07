using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IData<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.Stat> StatDic { get; private set; }

    public void Init()
    {
        StatDic = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();
    }

    /**
     * path 위치의 Json 파일을 TextAsset 타입으로 로드
     */
    Data LoadJson<Data, Key, Value>(string path) where Data : IData<Key, Value>
    {
        TextAsset textAsset = Managers.ResourceMng.Load<TextAsset>($"Datas/{path}");
        
        return JsonUtility.FromJson<Data>(textAsset.text);
    }
}
