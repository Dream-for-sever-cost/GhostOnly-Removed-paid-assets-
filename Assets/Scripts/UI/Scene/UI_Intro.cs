using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using Util;

public class UI_Intro : UI_Scene
{
    private Sequence _textSequence;
    private TextMeshProUGUI _text;
    private string _inputText;
    private Sequence _sequence;

    enum Texts
    {
        DescriptionText,
        DialogueText,
    }

    enum Buttons
    {
        SkipButton,
    }

    enum GameObjects
    {
        DialogueBG,
    }

    private void Start()
    {
        Init();
        IntroStep();  
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
        BindObject(typeof(GameObjects));
        GetButton((int)Buttons.SkipButton).onClick.AddListener(SkipScene);
        GetObject((int)GameObjects.DialogueBG).SetActive(false);

        GetText((int)Texts.DialogueText).text = GetString(Constants.StringRes.IntroDialogue);

        return true;
    }

    private void TextSequence()
    {       
        _text = GetText((int)Texts.DescriptionText);
        _textSequence = DOTween.Sequence().OnStart(() =>
        {
            _text.DOKill();
            _text.text = "";
        })
        .Append(_text.DOText(_inputText, 2f).SetEase(Ease.Linear));
    }

    private void StartText()
    {
        _inputText = GetString(Constants.StringRes.Intro_1);
    }
    private void FirstText()
    {
        _inputText = GetString(Constants.StringRes.Intro_2);
    }

    private void SecondText()
    {
        _inputText = GetString(Constants.StringRes.Intro_3);
    }

    private void ThirdText()
    {
        _inputText = GetString(Constants.StringRes.Intro_4);
    }

    private void FourthText()
    {
        _inputText = GetString(Constants.StringRes.Intro_5);
    }

    private void OnDialogue()
    {
        GetObject((int)GameObjects.DialogueBG).SetActive(true);
    }

    private void OffDialogue()
    {
        GetObject((int)GameObjects.DialogueBG).SetActive(false);
    }

    public void SkipScene()
    {    
        DOTween.KillAll(this);
        Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
    }

    private void IntroStep()
    {
        _sequence = DOTween.Sequence().OnStart(() =>
        {
            StartText();
            TextSequence();
            DOVirtual.DelayedCall(4f, FirstText);
            DOVirtual.DelayedCall(4f, TextSequence);
            DOVirtual.DelayedCall(8f, SecondText);
            DOVirtual.DelayedCall(8f, TextSequence);
            DOVirtual.DelayedCall(11f, ThirdText);
            DOVirtual.DelayedCall(11f, TextSequence);
            DOVirtual.DelayedCall(12f, OnDialogue);
            DOVirtual.DelayedCall(15f, OffDialogue);
            DOVirtual.DelayedCall(22f, FourthText);
            DOVirtual.DelayedCall(22f, TextSequence);
            DOVirtual.DelayedCall(30f, SkipScene);
        });
    }
}
