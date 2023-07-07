using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPanel
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPanel = GetGameObject((int)GameObjects.GridPanel);
        foreach (Transform child in gridPanel.transform)
            Managers.ResourceMng.Destroy(child.gameObject);

        // todo TestCode
        for (int i = 0; i < 8; i++)
        {
            GameObject go = Managers.UIMng.MakeSubItemUI<UI_Inven_Item>(gridPanel.transform).gameObject;
            UI_Inven_Item invenItem = go.GetOrAddComponent<UI_Inven_Item>();
            invenItem.SetInfo($"Item{i}");
        }
    }
}
