using UnityEngine;
using Util;
using DG.Tweening;

public class UI_Lobby : UI_Scene
{
    #region Enums

    enum Buttons
    {
        StartButton,
        SettingButton,
        QuitButton,
        CreditButton,
    }

    enum Texts
    {
        StartText,
        SettingText,
        QuitText,
    }

    #endregion

    private bool _isSubscribeLoadEvent = false;

    private Animator _startButtonAnim;
    private Animator _settingButtonAnim;
    private Animator _quitButtonAnim;

    private const string HOVER = "Hover";

    void Start()
    {
        Init();

        if (Managers.Data.IsLoaded)
        {
            PlaySound();
        }
        else
        {
            _isSubscribeLoadEvent = true;
            Managers.Data.LoadAllDataSetEvent += PlaySound;
            Managers.Data.Init();
        }
    }

    private void PlaySound()
    {
        Managers.Sound.Play(Data.SoundType.LobbyBGM, Define.Sound.Bgm);
    }

    private void OnDisable()
    {
        if (_isSubscribeLoadEvent)
        {
            Managers.Data.LoadAllDataSetEvent -= PlaySound;
        }

        Managers.GameManager.OnAppSettingChanged -= SetText;
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        _startButtonAnim = GetButton((int)Buttons.StartButton).transform.GetChild(0).GetComponent<Animator>();
        _settingButtonAnim = GetButton((int)Buttons.SettingButton).transform.GetChild(0).GetComponent<Animator>();
        _quitButtonAnim = GetButton((int)Buttons.QuitButton).transform.GetChild(0).GetComponent<Animator>();

        GetButton((int)Buttons.StartButton).BindEvent(()=> { _startButtonAnim.SetBool(HOVER, true); GetText((int)Texts.StartText).DOFontSize(80, 0.25f); }, Define.UIEvent.PointerEnter);
        GetButton((int)Buttons.StartButton).BindEvent(() => { _startButtonAnim.SetBool(HOVER, false); GetText((int)Texts.StartText).DOFontSize(70, 0.25f); }, Define.UIEvent.PointerExit);

        GetButton((int)Buttons.SettingButton).BindEvent(() => { _settingButtonAnim.SetBool(HOVER, true); GetText((int)Texts.SettingText).DOFontSize(80, 0.25f); }, Define.UIEvent.PointerEnter);
        GetButton((int)Buttons.SettingButton).BindEvent(() => { _settingButtonAnim.SetBool(HOVER, false); GetText((int)Texts.SettingText).DOFontSize(70, 0.25f); }, Define.UIEvent.PointerExit);

        GetButton((int)Buttons.QuitButton).BindEvent(() => { _quitButtonAnim.SetBool(HOVER, true); GetText((int)Texts.QuitText).DOFontSize(80, 0.25f); }, Define.UIEvent.PointerEnter);
        GetButton((int)Buttons.QuitButton).BindEvent(() => { _quitButtonAnim.SetBool(HOVER, false); GetText((int)Texts.QuitText).DOFontSize(70, 0.25f); }, Define.UIEvent.PointerExit);

        GetButton((int)Buttons.StartButton).BindEvent(OnClickedStartButton);
        GetButton((int)Buttons.SettingButton).BindEvent(OnClickedSettingButton);
        GetButton((int)Buttons.QuitButton).BindEvent(OnClickedExitButton);

        GetButton((int)Buttons.CreditButton).BindEvent(() => { Managers.Scene.ChangeScene(Define.Scene.CreditScene); });

        Managers.GameManager.OnAppSettingChanged += SetText;
        SetText(null);
        return true;
    }

    private void SetText(AppSetting _)
    {
        GetText((int)Texts.StartText).text = GetString(Constants.Setting.LOBBY_START);
        GetText((int)Texts.SettingText).text = GetString(Constants.Setting.LOBBY_SETTING);
        GetText((int)Texts.QuitText).text = GetString(Constants.Setting.LOBBY_QUIT);
    }

    private void OnClickedStartButton()
    {
        Managers.Sound.PlaySound(Data.SoundType.Interaction);
        Managers.UI.ShowPopupUI<UI_SelectGame>();
    }

    private void OnClickedSettingButton()
    {
        Managers.Sound.PlaySound(Data.SoundType.Interaction);
        Managers.UI.ShowPopupUI<UI_Settings>();
    }

    private void OnClickedExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}