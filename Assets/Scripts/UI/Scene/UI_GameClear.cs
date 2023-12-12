using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Util;

public class UI_GameClear : UI_Scene
{
    private Sequence _textSequence;
    private TextMeshProUGUI _text;
    private string _inputText;
    private enum Texts
    {
        DescriptText,
    }

    private void Start()
    {
        Init();
        GameClearStep();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
      
        BindText(typeof(Texts));  

        return true;
    }

    private void StartText()
    {
        _inputText = GetString(Constants.StringRes.Clear_1);
    }

    private void FirstText()
    {
        _inputText = GetString(Constants.StringRes.Clear_2);
    }

    private void TextSequence()
    {
        _text = GetText((int)Texts.DescriptText);

        _textSequence = DOTween.Sequence().OnStart(() =>
        {
            _text.DOKill();
            _text.text = string.Empty;
        })
        .Append(_text.DOText(_inputText, 3f).SetEase(Ease.Linear));
    }

    private void GameClearStep()
    {
        StartText();
        TextSequence();
        DOVirtual.DelayedCall(6f, FirstText);
        DOVirtual.DelayedCall(6f, TextSequence);
        DOVirtual.DelayedCall(13f, ShowPopup);
    }

    private void ShowPopup()
    {
        Managers.UI.ShowPopupUI<UI_Result>();
    }
}
