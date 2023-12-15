using Manager;
using UnityEngine.UI;
using DG.Tweening;
using Data;
using UI.Popup;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using Application = UnityEngine.Application;
using Text = TMPro.TextMeshProUGUI;

public class UI_Settings : UI_Popup
{
    private Sequence _openSequence;

    private enum Sliders
    {
        MasterVolumeSlider,
        EffectVolumeSlider,
        BgmVolumeSlider
    }

    private enum Images
    {
        KoreanBorderImage,
        AmericanBorderImage,
    }

    private enum Toggles
    {
        MasterVolumeToggle,
        EffectVolumeToggle,
        BgmVolumeToggle,
        MinimapToggle,
    }

    //todo language button make to sub_item for reuse
    private enum Buttons
    {
        CloseButton,
        LobbyButton,
        KoreanButton,
        AmericanButton,
        ExitButton,
        HelpButton,
        LicenseButton,
        HelpBackButton,
        LicenseBackButton,
    }

    private enum Texts
    {
        MasterVolumeText,
        MasterVolumeValueText,
        EffectVolumeText,
        EffectVolumeValueText,
        BgmVolumeText,
        BgmVolumeValueText,
        LanguageText,
        MinimapText,
        HelpText,
        LicenseText,
        LobbyButtonText,
        ExitButtonText,

        TextMove,
        TextPossess,
        TextEquip,
        TextInteract,
        TextAction,
        TextStatus,
        TextMinimap,
        TextSetting,
    }

    private enum GameObjects
    {
        Background,
        PanelHelp,
        PanelLicense,
    }

    //===========Views===========//
    private Text _masterVolumeText;
    private Text _effectVolumeText;
    private Text _bgmVolumeText;

    private Slider _masterSlider;
    private Slider _bgmSlider;
    private Slider _effectSlider;

    private Toggle _minimapToggle;

    private Toggle _masterVolumeToggle;
    private Toggle _effectVolumeToggle;
    private Toggle _bgmVolumeToggle;

    private Image _koreanBorderImage;
    private Image _englishBorderImage;

    //===========States===========//
    //MVC 적용 하려면 이거 필요함 근데 지금 안함 

    public override bool Init()
    {
        gameObject.SetActive(false);
        Bind<Slider>(typeof(Sliders));
        Bind<Text>(typeof(Texts));
        Bind<Toggle>(typeof(Toggles));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        BindObject(typeof(GameObjects));

        _minimapToggle = Get<Toggle>((int)Toggles.MinimapToggle);
        _minimapToggle.onValueChanged.AddListener(ChangeMinimapPosition);

        //=======Volume Sliders=======//
        _masterSlider = Get<Slider>((int)Sliders.MasterVolumeSlider);
        _masterSlider.onValueChanged.AddListener(ChangeMasterVolume);
        _effectSlider = Get<Slider>((int)Sliders.EffectVolumeSlider);
        _effectSlider.onValueChanged.AddListener(ChangeEffectVolume);
        _bgmSlider = Get<Slider>((int)Sliders.BgmVolumeSlider);
        _bgmSlider.onValueChanged.AddListener(ChangeBgmVolume);

        //=======Volume Texts=======//
        _masterVolumeText = Get<Text>((int)Texts.MasterVolumeValueText);
        _effectVolumeText = Get<Text>((int)Texts.EffectVolumeValueText);
        _bgmVolumeText = Get<Text>((int)Texts.BgmVolumeValueText);

        _masterVolumeToggle = Get<Toggle>((int)Toggles.MasterVolumeToggle);
        _effectVolumeToggle = Get<Toggle>((int)Toggles.EffectVolumeToggle);
        _bgmVolumeToggle = Get<Toggle>((int)Toggles.BgmVolumeToggle);

        _masterVolumeToggle.onValueChanged.AddListener(ToggleMasterVolume);
        _effectVolumeToggle.onValueChanged.AddListener(ToggleEffectVolume);
        _bgmVolumeToggle.onValueChanged.AddListener(ToggleBgmVolume);

        //=======Buttons=======//
#if UNITY_WEBGL
        GetButton((int)Buttons.ExitButton).gameObject.SetActive(false);
#endif

        GetButton((int)Buttons.ExitButton).BindEvent(Exit);
        GetButton((int)Buttons.CloseButton).BindEvent(ClosePopup);
        GetButton((int)Buttons.HelpButton).BindEvent(SetActiveHelp);
        GetButton((int)Buttons.HelpBackButton).BindEvent(SetUnActiveHelp);
        GetButton((int)Buttons.LicenseButton).BindEvent(SetActiveLicense);
        GetButton((int)Buttons.LicenseBackButton).BindEvent(SetUnActiveLicense);

        Button lobbyButton = GetButton((int)Buttons.LobbyButton);
        bool isLobbyScene = SceneManager.GetActiveScene().name == Define.Scene.LobbyScene.ToString();
        lobbyButton.gameObject.SetActive(!isLobbyScene);
        if (!isLobbyScene)
        {
            lobbyButton.BindEvent(ShowLobbyAlertDialog);
        }

        //=======Languages=======//
        _koreanBorderImage = GetImage((int)Images.KoreanBorderImage);
        _englishBorderImage = GetImage((int)Images.AmericanBorderImage);
        GetButton((int)Buttons.KoreanButton).BindEvent(SelectKorean);
        GetButton((int)Buttons.AmericanButton).BindEvent(SelectEnglish);

        UpdateUI();
        UpdateTextUI();

        gameObject.SetActive(true);
        GetObject((int)GameObjects.Background).SetActive(false);
        GetObject((int)GameObjects.PanelHelp).SetActive(false);
        GetObject((int)GameObjects.PanelLicense).SetActive(false);
        SetOpenSequence();
        _init = true;
        return true;
    }

    private void ChangeMasterVolume(float volume)
    {
        PreferencesManager.SetMasterVolume(volume);
        _masterVolumeText.text = volume.ToString("P0");
    }

    private void ChangeEffectVolume(float volume)
    {
        PreferencesManager.SetEffectVolume(volume);
        _effectVolumeText.text = volume.ToString("P0");
    }

    private void ChangeBgmVolume(float volume)
    {
        PreferencesManager.SetBgmVolume(volume);
        _bgmVolumeText.text = volume.ToString("P0");
    }

    private void ChangeMinimapPosition(bool isMinimapLeft)
    {
        PreferencesManager.SetMinimapLeft(isMinimapLeft);
    }

    private void UpdateTextUI()
    {
        GetText((int)Texts.MasterVolumeText).text = GetString(Constants.Setting.SETTING_MASTER_VOLUME_TEXT);
        GetText((int)Texts.EffectVolumeText).text = GetString(Constants.Setting.SETTTING_EFFECT_VOLUME_TEXT);
        GetText((int)Texts.BgmVolumeText).text = GetString(Constants.Setting.SETTING_BGM_VOLUME_TEXT);
        GetText((int)Texts.LanguageText).text = GetString(Constants.Setting.SETTING_LANGUAGE_TEXT);
        GetText((int)Texts.MinimapText).text = GetString(Constants.Setting.SETTING_MINIMAP_TEXT);
        GetText((int)Texts.LobbyButtonText).text = GetString(Constants.Setting.SETTING_LOBBY_BUTTON_TEXT);
        GetText((int)Texts.ExitButtonText).text = GetString(Constants.Setting.SETTING_EXIT_BUTTON_TEXT);
        GetText((int)Texts.HelpText).text = GetString(Constants.Setting.SETTING_HELP_TEXT);
        GetText((int)Texts.LicenseText).text = GetString(Constants.Setting.SETTING_LICENSE_TEXT);

        GetText((int)Texts.TextMove).text = GetString(Constants.Setting.SETTING_HELP_MOVE);
        GetText((int)Texts.TextPossess).text = GetString(Constants.Setting.SETTING_HELP_POSSESS);
        GetText((int)Texts.TextEquip).text = GetString(Constants.Setting.SETTING_HELP_EQUIP);
        GetText((int)Texts.TextInteract).text = GetString(Constants.Setting.SETTING_HELP_INTERACT);
        GetText((int)Texts.TextAction).text = GetString(Constants.Setting.SETTING_HELP_ACTION);
        GetText((int)Texts.TextStatus).text = GetString(Constants.Setting.SETTING_HELP_STATUS);
        GetText((int)Texts.TextMinimap).text = GetString(Constants.Setting.SETTING_HELP_MINIMAP);
        GetText((int)Texts.TextSetting).text = GetString(Constants.Setting.SETTING_HELP_SETTING);
    }

    private void UpdateUI()
    {
        //=======Minimap=======//
        bool isMinimapLeft = PreferencesManager.IsMinimapLeft();
        _minimapToggle.SetIsOnWithoutNotify(isMinimapLeft);

        //======Volume Sliders======//
        float masterVolume = PreferencesManager.GetMasterVolume();
        float effectVolume = PreferencesManager.GetEffectVolume();
        float bgmVolume = PreferencesManager.GetBgmVolume();
        _masterSlider.SetValueWithoutNotify(masterVolume);
        _effectSlider.SetValueWithoutNotify(effectVolume);
        _bgmSlider.SetValueWithoutNotify(bgmVolume);

        _masterVolumeText.text = masterVolume.ToString("P0");
        _effectVolumeText.text = effectVolume.ToString("P0");
        _bgmVolumeText.text = bgmVolume.ToString("P0");

        //=======Language=======//
        Language language = PreferencesManager.GetLanguage();
        _englishBorderImage.gameObject.SetActive(language == Language.ENGLISH);
        _koreanBorderImage.gameObject.SetActive(language == Language.KOREAN);

        //=======Volume Toggles=======//
        bool isMasterVolumeOn = PreferencesManager.IsMasterVolumeOn();
        bool iEffectVolumeOn = PreferencesManager.IsEffectVolumeOn();
        bool isBgmVolumeOn = PreferencesManager.IsBgmVolumeOn();

        // Not 인 이유 : 꺼졌을 때 토글이 켜짐
        _masterVolumeToggle.SetIsOnWithoutNotify(!isMasterVolumeOn);
        _effectVolumeToggle.SetIsOnWithoutNotify(!iEffectVolumeOn);
        _bgmVolumeToggle.SetIsOnWithoutNotify(!isBgmVolumeOn);
    }

    private void SetOpenSequence()
    {
        GameObject window = GetObject((int)GameObjects.Background);
        _openSequence = DOTween.Sequence()
            .SetAutoKill(false)
            .OnStart(() =>
            {
                window.SetActive(true);
                window.transform.localScale = Vector3.zero;
                window.GetComponent<CanvasGroup>().alpha = 0;
            })
            .Append(window.transform.DOScale(1, Constants.Setting.OpenAppendScaleDuration))
            .SetEase(Ease.Linear)
            .Join(window.GetComponent<CanvasGroup>().DOFade(1, Constants.Setting.OpenJoinFadeDuration))
            .SetUpdate(true);
    }

    private void ClosePopup()
    {
        _openSequence.Kill();
        Managers.Sound.PlaySound(Data.SoundType.Click);
        ClosePopupUI();
    }

    private void Exit()
    {
#if UNITY_EDITOR
        Debug.Log("종료되었습니다.");
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    private void ShowLobbyAlertDialog()
    {
        Managers.Sound.PlaySound(Data.SoundType.Interaction);
        UI_AlertDialog dialog = Managers.UI.ShowPopupUI<UI_AlertDialog>();
        dialog.Alert(
            messageId: Constants.StringRes.LobbyMessageId,
            positiveTextId: Constants.StringRes.OkId,
            negativeTextId: Constants.StringRes.CancelId,
            onPositive: GoToLobby);
    }

    private void GoToLobby()
    {
        Managers.Sound.GetCurrent().Stop();
        DOTween.KillAll(true);
        //todo isLoading to show loading state       
        SceneManager.LoadSceneAsync("LobbyScene");
    }

    private void SelectEnglish()
    {
        PreferencesManager.SetLanguage(Language.ENGLISH);
        UpdateTextUI();
        _englishBorderImage.gameObject.SetActive(true);
        _koreanBorderImage.gameObject.SetActive(false);
    }

    private void SelectKorean()
    {
        PreferencesManager.SetLanguage(Language.KOREAN);
        UpdateTextUI();
        _englishBorderImage.gameObject.SetActive(false);
        _koreanBorderImage.gameObject.SetActive(true);
    }

    private void ToggleMasterVolume(bool isOff)
    {
        bool isVolumeOn = !isOff;
        PreferencesManager.SetMasterVolumeSwitch(isVolumeOn);
    }

    private void ToggleEffectVolume(bool isOff)
    {
        bool isVolumeOn = !isOff;
        PreferencesManager.SetEffectVolumeSwitch(isVolumeOn);
    }

    private void ToggleBgmVolume(bool isOff)
    {
        bool isVolumeOn = !isOff;
        PreferencesManager.SetBgmVolumeSwitch(isVolumeOn);
    }

    private void SetActiveHelp()
    {
        Managers.Sound.PlaySound(SoundType.Interaction);
        GetObject((int)GameObjects.PanelHelp).SetActive(true);
    }

    private void SetUnActiveHelp()
    {
        Managers.Sound.PlaySound(SoundType.Click);
        GetObject((int)GameObjects.PanelHelp).SetActive(false);
    }

    private void SetActiveLicense()
    {
        Managers.Sound.PlaySound(SoundType.Interaction);
        GetObject((int)GameObjects.PanelLicense).SetActive(true);
        //Managers.Scene.ChangeScene(Define.Scene.CreditScene);
    }

    private void SetUnActiveLicense()
    {
        Managers.Sound.PlaySound(SoundType.Click);
        GetObject((int)GameObjects.PanelLicense).SetActive(false);
    }
}