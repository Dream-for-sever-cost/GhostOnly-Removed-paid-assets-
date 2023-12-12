using DG.Tweening;
using UnityEngine;
using Util;

public class UI_SelectGame : UI_Popup
{
    private Sequence _openSequence;

    enum GameObjects
    {
        BackGroundImage,
    }

    enum Buttons
    {
        GoTutorialButton,
        GoGameButton,
        CloseButton,
    }

    enum Texts
    {
        GoTutorialText,
        GoGameText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.GoGameButton).BindEvent(() => OnClickedButton(Define.Scene.GameScene));
        GetButton((int)Buttons.GoTutorialButton).BindEvent(() => OnClickedButton(Define.Scene.TutorialScene));
        GetButton((int)Buttons.CloseButton).BindEvent(() => Managers.UI.ClosePopupUI(this));

        GetText((int)Texts.GoTutorialText).text = GetString(Constants.Setting.LOBBY_TUTORIAL);
        GetText((int)Texts.GoGameText).text = GetString(Constants.Setting.LOBBY_MAINGAME);
        GetObject((int)GameObjects.BackGroundImage).transform.localScale = UnityEngine.Vector3.zero;

        OpenSequence();

        return true;
    }

    private void OnClickedButton(Define.Scene SelectScene)
    {
        Managers.UI.ClosePopupUI(this);
        Managers.Scene.ChangeScene(SelectScene);
    }

    private void OpenSequence()
    {
        GameObject bg = GetObject((int)GameObjects.BackGroundImage);
        _openSequence = DOTween.Sequence()
            .Append(bg.transform.DOScale(1f, Constants.DOTween.OpenPopupDuration))
            .SetEase(Ease.Linear)
            .SetUpdate(true);
    }
}
