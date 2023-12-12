using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class TutorialScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.GameScene;
        Managers.UI.ShowSceneUI<UI_Game>();

        Managers.SpellBook.Init();
        Managers.Soul.Init();
        Managers.Data.Init();
        ObjectPoolManager.Instance.Init();

        Instantiate(Resources.Load(Constants.Prefabs.Player));
        Instantiate(Resources.Load(Constants.Prefabs.MinimapCamera));
        Instantiate(Resources.Load(Constants.Prefabs.DayManager));
        Instantiate(Resources.Load(Constants.Tutorial.TutorialManager));

        return true;
    }
}
