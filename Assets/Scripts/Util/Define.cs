using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        PointerEnter,
        PointerExit,
    }

    public enum Scene
    {
        Unknown,
        LobbyScene,
        TutorialScene,
        GameScene,
        LabTestScene,
        LoadingScene,
        IntroScene,
        GameOverScene,
        GameClearScene,
        CreditScene,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        Speech,
        Max,
    }
}