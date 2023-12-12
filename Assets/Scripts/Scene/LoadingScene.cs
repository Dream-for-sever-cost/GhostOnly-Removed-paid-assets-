using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.LoadingScene;
        Debug.Log("show scene ui ");
        Managers.UI.ShowSceneUI<UI_Loading>();
        Debug.Log("load version");
        return true;

    }
}
