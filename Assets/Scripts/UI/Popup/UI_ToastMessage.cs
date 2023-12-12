using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class UI_ToastMessage : UI_Base
{
    private string _message;
    private Color _messageColor;

    private enum Images
    {
        Background
    }

    private enum Texts
    {
        ErrorMessageText,
    }

    public override bool Init()
    {
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        _init = true;
        UpdateUI();
        return true;
    }


    public void SetMessage(string message, Color color)
    {
        Debug.Log("set message");
        _message = message;
        _messageColor = color;
    }

    private void UpdateUI()
    {
        //todo optimization
        TextMeshProUGUI text = GetText((int)(Texts.ErrorMessageText));
        text.text = _message;
        text.color = _messageColor;
        Image background = GetImage((int)(Images.Background));
        DOTween.Sequence(background)
            .Append(background.transform.DOMoveY(background.rectTransform.rect.height, Constants.Time.ToastLengthShort))
            .OnComplete(() => Destroy(gameObject, Constants.Time.ToastLengthShort))
            .SetUpdate(true);
    }
}