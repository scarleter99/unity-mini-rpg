using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @brief TEST CODE
 */
public class Test : MonoBehaviour
{
    private void Start()
    {
        //TestMethod();
        
        //TestCube();
        //TestPopupUI();
        //TestSceneUI();
    }

    public void TestMethod()
    {
        Debug.Log("1");
        Debug.Log("2");
    }
    
    public void TestCube()
    {
        List<GameObject> go = new List<GameObject>();
        for (int i = 0; i < 4; i++)
            go.Add(GameManager.ResourceMng.Instantiate("Cube"));
    }

    public void TestPopupUI()
    {
        GameManager.UIMng.ShowPopupUI<UI_TestPopup>();
    }

    public void TestSceneUI()
    {
        GameManager.UIMng.ShowSceneUI<UI_Inven>();
    }
}