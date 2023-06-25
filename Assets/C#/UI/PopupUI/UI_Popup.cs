using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        GameManager.UiMng.SetCanvas(gameObject, true);
    }

    /**
     * @brief Popup 닫기
     */
    public virtual void ClosePopupUI()
    {
        GameManager.UiMng.ClosePopupUI(this);
    }
}
