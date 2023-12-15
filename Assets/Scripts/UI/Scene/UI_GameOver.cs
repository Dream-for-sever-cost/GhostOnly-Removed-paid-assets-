using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Util;

public class UI_GameOver : UI_Scene
{
    private enum GameObjects
    {        
        Scene2,
        GhostImage,
    }

    private enum Texts
    {
        DescriptText,
    }

    private Sequence _textSequence;
    private TextMeshProUGUI _text;
    private string _inputText;
    private int _die = Animator.StringToHash(Constants.AniParams.Die);


    private void Start()
    {
        Init();
        GameOverStep();
    }
    private void OnDisable()
    {
        DOTween.KillAll(this);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));    

        GetObject((int)GameObjects.Scene2).SetActive(false);

        return true;
    }

    private void StartText()
    {
        _inputText = GetString(Constants.StringRes.Over_1);
    }

    private void FirstText()
    {
        _inputText = GetString(Constants.StringRes.Over_2);
    }

    private void TextSequence()
    {
        _text = GetText((int)Texts.DescriptText);
        
        _textSequence = DOTween.Sequence().OnStart(() =>
        {
            _text.DOKill();
            _text.text = "";
        })
        .Append(_text.DOText(_inputText, 3f).SetEase(Ease.Linear));
    }

    private void OnScene2()
    {
        GameObject scene = GetObject((int)GameObjects.Scene2);
        scene.SetActive(true);
    }

    private void SetGhostDie()
    {    
        Animator ghost = GetObject((int)GameObjects.GhostImage).GetComponent<Animator>();
        ghost.SetBool(_die, true);
    }

    private void GameOverStep()
    {
        StartText();
        TextSequence();
        DOVirtual.DelayedCall(7f, OnScene2);
        DOVirtual.DelayedCall(7f, FirstText);
        DOVirtual.DelayedCall(7f, TextSequence);
        DOVirtual.DelayedCall(11f, SetGhostDie);
        DOVirtual.DelayedCall(13f, ShowPopup);    
    }

    private void ShowPopup()
    {
        Managers.UI.ShowPopupUI<UI_Result>();
    }
}
