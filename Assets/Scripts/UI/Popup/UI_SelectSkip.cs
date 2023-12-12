using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class UI_SelectSkip : UI_Popup
{
    enum Buttons
    {
        YesButton,
        NoButton
    }

    enum Texts
    {
        AskText,
        ToLobbyText,
        YesText,
        NoText,
    }

    enum GameObjects
    {
        BackGroundImage,
    }

    private Sequence _openSequence;

    public override bool Init()
    {

        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.YesButton).BindEvent(() => OnClickYesButton());
        GetButton((int)Buttons.NoButton).BindEvent(() => OnClickNoButton());

        GetText((int)Texts.AskText).text = GetString(Constants.StringRes.AskText);
        GetText((int)Texts.ToLobbyText).text = GetString(Constants.StringRes.ToLobbyText);
        GetText((int)Texts.YesText).text = GetString(Constants.StringRes.OkId);
        GetText((int)Texts.NoText).text = GetString(Constants.StringRes.CancelId);
        GetObject((int)GameObjects.BackGroundImage).transform.localScale = Vector2.zero;
        OpenSequence();
        return true;
    }

    private void OnClickYesButton()
    {
        Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
    }

    private void OnClickNoButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OpenSequence()
    {
        GameObject panel = GetObject((int)GameObjects.BackGroundImage);
        _openSequence = DOTween.Sequence()
            .Append(panel.transform.DOScale(1, Constants.Coffin.ClickPanelAppendScaleDuration)).SetEase(Ease.Linear).SetUpdate(true);
    }
}