using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.GameOverScene;
        Managers.UI.ShowSceneUI<UI_GameOver>();

        return true;
    }
}
