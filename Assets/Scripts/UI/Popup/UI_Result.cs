using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class UI_Result : UI_Popup
{
    private enum Texts
    {
        DayText,
        PlayTimeText,
        SoulText,
        HeroDefeatText,
        ResultText,

        DayHeaderText,
        PlayTimeHeaderText,
        SoulHeaderText,
        HeroDefeatHeaderText,
        BackText
    }

    private enum Buttons
    {
        BackButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.BackButton).BindEvent(LoadGameScene);
        SetText();

        return true;
    }

    private void SetText()
    {
        GetText((int)Texts.DayHeaderText).text = GetString(Constants.StringRes.ResultDay);

        GetText((int)Texts.PlayTimeHeaderText).text = Managers.UI.GetString(Constants.StringRes.ResultPlaytime);
        GetText((int)Texts.SoulHeaderText).text = Managers.UI.GetString(Constants.StringRes.ResultSoul);
        GetText((int)Texts.HeroDefeatHeaderText).text = Managers.UI.GetString(Constants.StringRes.ResultHuman);
        GetText((int)Texts.BackText).text = Managers.UI.GetString(Constants.StringRes.ResultBack);

        GetText((int)Texts.ResultText).text = Managers.GameManager.isGameClear ? Constants.Setting.GameClear : Constants.Setting.GameOver;
        GetText((int)Texts.SoulText).text = Managers.Soul.EarnedSoul.ToString();
        GetText((int)Texts.DayText).text = Managers.GameManager.currentDay.ToString();
        GetText((int)Texts.PlayTimeText).text = $"{(int)(Managers.GameManager.realTime / 60)} : {(int)(Managers.GameManager.realTime % 60)}";
        GetText((int)Texts.HeroDefeatText).text = Managers.GameManager.heroDeathCount.ToString();
    }

    private void LoadGameScene()
    {
        DOTween.KillAll();
        Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
        //SceneManager.LoadSceneAsync("LobbyScene");
    }
}
