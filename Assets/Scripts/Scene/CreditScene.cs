using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.CreditScene;
        Managers.UI.ShowSceneUI<UI_Credit>();

        return true;
    }
}
