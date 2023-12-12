using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class GameScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.GameScene;
        Managers.UI.ShowSceneUI<UI_Game>();
        Managers.GameManager.StartNewGame();
        Instantiate(Resources.Load(Constants.Prefabs.Player));
        Instantiate(Resources.Load(Constants.Prefabs.MinimapCamera));
        Instantiate(Resources.Load(Constants.Prefabs.DayManager));
        return true;
    }
}
