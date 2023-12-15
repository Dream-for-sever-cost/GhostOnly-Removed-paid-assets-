using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Credit : UI_Scene
{
    private Sequence _titleSequence;
    private Sequence _creditSequence;
    private enum Texts
    {
        TitleText,
    }

    private enum Buttons
    {
        SkipButton,
    }

    private void Start()
    {
        Init();
        CreditSequence();
    }

    private void OnDisable()
    {
        DOTween.KillAll(this);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetText((int)Texts.TitleText).gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        GetText((int)Texts.TitleText).gameObject.SetActive(false);
        GetButton((int)Buttons.SkipButton).BindEvent(() => { Skip(); });

        return true;
    }

    private void Skip() 
    {
        DOTween.KillAll(this);
        Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
    }

    private void OnTitle()
    {
        TitleSequence();
        GetText((int)Texts.TitleText).gameObject.SetActive(true);
    }

    private void TitleSequence()
    {
        GameObject title = GetText((int)Texts.TitleText).gameObject;
        _titleSequence = DOTween.Sequence()
            .Append(title.GetComponent<CanvasGroup>().DOFade(1f, 1f));
    }

    private void CreditSequence()
    {
        _creditSequence = DOTween.Sequence()
            .OnStart(() => 
            {
                DOVirtual.DelayedCall(24f, OnTitle);
                DOVirtual.DelayedCall(27f, Skip);
            });
    }
}
