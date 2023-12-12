using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.GameClearScene;
        Managers.UI.ShowSceneUI<UI_GameClear>();

        return true;
    }
}
