using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.IntroScene;
        Managers.UI.ShowSceneUI<UI_Intro>();

        return true;
    }
}
