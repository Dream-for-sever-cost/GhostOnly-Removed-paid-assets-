using Analytics;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.Services.Analytics;
using Util;

public class UI_Game : UI_Scene
{
    #region Enum

    enum Texts
    {
        SkullStateText,
        SoulStateText,
        AltarHPText,
        DayText,
        CooltimeActionText,
        Cooltime1Text,
        Cooltime2Text,
    }

    enum Buttons
    {
        SettingButton,
        MasteryButton,
    }

    enum Images
    {
        AltarHPGage,
        DayImageBackground,
        SunAndMoonImage,
        ActionCooltime,
        Skill1Cooltime,
        Skill2Cooltime,
    }

    enum RectTransforms
    {
        MinimapBackground,
        CooltimeHolder,
        UnitInfo,
    }

    enum SkillTooltip
    {
        SkillTooltipBackground
    }

    #endregion

    private UI_SkillTooltip _skillTooltip;

    private int _prevSoul = 0;
    private int _currentSoul = 0;
    private bool _isMinimapLeft = false;
    private int _curAltarHp = 0;
    private float _topOfSun;
    private float _rightOfSun;

    private SkullController _currentSkull;
    private Sequence _hpBarSequence;
    private Sequence _soulTextSequence;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Bind

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        Bind<UI_SkillTooltip>(typeof(SkillTooltip));
        Bind<RectTransform>(typeof(RectTransforms));

        //===== Hover Event =====/
        GetImage((int)Images.ActionCooltime).BindEvent(action: ShowActionTooltip, type: Define.UIEvent.PointerEnter);
        GetImage((int)Images.ActionCooltime).BindEvent(action: HideSkillTooltip, type: Define.UIEvent.PointerExit);

        GetImage((int)Images.Skill1Cooltime).BindEvent(action: ShowSkill1, type: Define.UIEvent.PointerEnter);
        GetImage((int)Images.Skill1Cooltime).BindEvent(action: HideSkillTooltip, type: Define.UIEvent.PointerExit);

        GetImage((int)Images.Skill2Cooltime).BindEvent(action: ShowSkill2, type: Define.UIEvent.PointerEnter);
        GetImage((int)Images.Skill2Cooltime).BindEvent(action: HideSkillTooltip, type: Define.UIEvent.PointerExit);
        _skillTooltip = Get<UI_SkillTooltip>((int)SkillTooltip.SkillTooltipBackground);

        #endregion


        #region Subscribe

        Managers.GameManager.Player.ControlSkullStartedEvent += UpdateUIBySkullStartedControl;
        Managers.GameManager.OnAppSettingChanged += SetMinimapPosition;
        Managers.GameManager.OnChangeSkullCount += UpdateSkullCountUI;
        Managers.Soul.OnSoulChanged += UpdateSoul;
        DayManager.Instance.OnChangedDayStatus += SendSoulDataToAnalytics;
        DayManager.Instance.OnChangedDayStatus += UpdateSunMoonImage;

        #endregion

        #region AddListener

        GetButton((int)Buttons.SettingButton).BindEvent(ShowMenuPopup);
        GetButton((int)Buttons.MasteryButton).BindEvent(ShowMasteryPopup);

        #endregion

        GetButton((int)Buttons.MasteryButton).gameObject.SetActive(false);

        Rect dayBackgroundRect = GetImage((int)Images.DayImageBackground).rectTransform.rect;
        Rect sunAndMoonRect = GetImage((int)Images.SunAndMoonImage).rectTransform.rect;

        _topOfSun = dayBackgroundRect.height - sunAndMoonRect.height;
        _rightOfSun = dayBackgroundRect.width - sunAndMoonRect.width;
        _isMinimapLeft = PreferencesManager.IsMinimapLeft();
        _skillTooltip.gameObject.SetActive(false);
        UpdateUI();

        return true;
    }

    private void Start()
    {
        Init();

        UnactiveCooltimeUI();
    }


    private void Update()
    {
        if (_curAltarHp != Managers.GameManager.currentAlter)
        {
            _curAltarHp = Managers.GameManager.currentAlter;
            UpdateAltarHPUI();
        }

        UpdateDayUI();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F8))
        {
            Managers.Soul.GetSoul(100);
        }
#else
#endif
    }

    private void OnDestroy()
    {
        Managers.GameManager.Player.ControlSkullStartedEvent -= UpdateUIBySkullStartedControl;
        Managers.GameManager.OnAppSettingChanged -= SetMinimapPosition;
        Managers.GameManager.OnChangeSkullCount -= UpdateSkullCountUI;
        Managers.Soul.OnSoulChanged -= UpdateSoul;
        DayManager.Instance.OnChangedDayStatus -= UpdateSunMoonImage;
        DayManager.Instance.OnChangedDayStatus -= SendSoulDataToAnalytics;
    }

    private void SendSoulDataToAnalytics(bool isNight)
    {
        if (isNight) { return; }

        DayStateData dayData = new()
        {
            CurrentDay = DayManager.Instance.CurrentDay, EarnedSoulPerDay = Managers.Soul.EarnedSoulByDay
        };
        Managers.Soul.EarnedSoulByDay = 0;
        AnalyticsService.Instance.CustomData(
            Constants.AnalyticsData.DayEvent.EventName,
            Constants.AnalyticsData.DayEvent.ToParams(dayData));
    }

    private void SetMinimapPosition(AppSetting setting)
    {
        _isMinimapLeft = setting.IsMinimapLeft;
        UpdateUIPosition();
    }

    private void UpdateSoul(int soul, int prevSoul)
    {
        _prevSoul = prevSoul;
        _currentSoul = soul;
        UpdateSoulUI();
    }

    private void UpdateSoulUI()
    {
        if (_currentSoul >= 1000)
        {
            int soulString = _currentSoul / 1000;
            GetText((int)Texts.SoulStateText).text = $"{soulString}K";
        }
        else
        {
            GetText((int)Texts.SoulStateText).text = _currentSoul.ToString("N0");
            _soulTextSequence = DOTween.Sequence()
                .Append(GetText((int)Texts.SoulStateText).DOCounter(_prevSoul, _currentSoul, 1f))
                .SetEase(Ease.OutQuad)
                .SetUpdate(true);
        }
    }

    private void UpdateSkullCountUI()
    {
        GetText((int)Texts.SkullStateText).text =
            $"{Managers.GameManager.currentSkullCount}/{Managers.GameManager.maxSkullCount}";
    }

    private void UpdateUI()
    {
        //=====Soul UI===========//
        UpdateSoulUI();

        //===============SkullCount=============
        UpdateSkullCountUI();

        //===============Minimap UI / Skill Slot=============
        UpdateUIPosition();

        //===============SunOrMoonImage=============
        UpdateSunMoonImage(DayManager.Instance.IsNight);
    }

    private void UpdateUIPosition()
    {
        RectTransform minimap = Get<RectTransform>((int)RectTransforms.MinimapBackground);
        RectTransform unitInfo = Get<RectTransform>((int)RectTransforms.UnitInfo);
        RectTransform masteryButton = GetButton((int)Buttons.MasteryButton).GetComponent<RectTransform>();
        RectTransform skillTooltip = _skillTooltip.GetComponent<RectTransform>();
        HorizontalLayoutGroup coolTimeGroup = Get<RectTransform>((int)RectTransforms.CooltimeHolder)
            .GetComponent<HorizontalLayoutGroup>();

        if (_isMinimapLeft)
        {
            minimap.pivot = Vector2.zero;
            minimap.anchorMin = Vector2.zero;
            minimap.anchorMax = Vector2.zero;

            unitInfo.pivot = Vector2.right;
            unitInfo.anchorMin = Vector2.right;
            unitInfo.anchorMax = Vector2.right;
            unitInfo.anchoredPosition =
                new Vector2(-Constants.Padding.PaddingNormal, unitInfo.anchoredPosition.y);

            coolTimeGroup.childAlignment = TextAnchor.MiddleRight;

            skillTooltip.pivot = Vector2.right;
            skillTooltip.anchorMin = Vector2.right;
            skillTooltip.anchorMax = Vector2.right;
            skillTooltip.anchoredPosition =
                new Vector2(-Constants.Padding.PaddingNormal, skillTooltip.anchoredPosition.y);

            masteryButton.pivot = new Vector2(1, 1);
            masteryButton.anchorMin = new Vector2(1, 1);
            masteryButton.anchorMax = new Vector2(1, 1);
        }
        else
        {
            minimap.pivot = Vector2.right;
            minimap.anchorMin = Vector2.right;
            minimap.anchorMax = Vector2.right;

            unitInfo.pivot = Vector2.zero;
            unitInfo.anchorMin = Vector2.zero;
            unitInfo.anchorMax = Vector2.zero;
            unitInfo.anchoredPosition =
                new Vector2(Constants.Padding.PaddingNormal, unitInfo.anchoredPosition.y);

            coolTimeGroup.childAlignment = TextAnchor.MiddleLeft;

            skillTooltip.pivot = Vector2.zero;
            skillTooltip.anchorMin = Vector2.zero;
            skillTooltip.anchorMax = Vector2.zero;
            skillTooltip.anchoredPosition =
                new Vector2(Constants.Padding.PaddingNormal, skillTooltip.anchoredPosition.y);

            masteryButton.pivot = Vector2.up;
            masteryButton.anchorMin = Vector2.up;
            masteryButton.anchorMax = Vector2.up;
        }
    }

    public void ExpandMap(bool expand)
    {
        RectTransform minimap = Get<RectTransform>((int)RectTransforms.MinimapBackground);

        // TODO : 두트윈 적용
        if (expand)
        {
            minimap.sizeDelta = Constants.Setting.ExpandMinimapUISize * Vector2.one;
        }
        else
        {
            minimap.sizeDelta = Constants.Setting.NormalMinimapUISize * Vector2.one;
        }
    }

    public void UpdateAltarHPUI()
    {
        GetText((int)Texts.AltarHPText).text = $"{Managers.GameManager.currentAlter} / {Constants.GameSystem.MaxAlter}";
        float amount = (float)Managers.GameManager.currentAlter / Constants.GameSystem.MaxAlter;
        _hpBarSequence = DOTween.Sequence()
            .Append(GetImage((int)Images.AltarHPGage).DOFillAmount(amount, 1f));
    }

    private void UpdateSunMoonImage(bool isNight)
    {
        if (isNight)
        {
            if (DayManager.Instance.CurrentDay % Constants.Day.FullMoonCycle == 6)
            {
                GetImage((int)Images.DayImageBackground).color = Constants.Colors.FullMoonBackground;
            }
            else
            {
                GetImage((int)Images.DayImageBackground).color = Constants.Colors.NightBackground;
            }

            GetImage((int)Images.SunAndMoonImage).sprite =
                Resources.LoadAll<Sprite>(Constants.Sprites.Moon)[DayManager.Instance.MoonIndex()];
        }
        else
        {
            GetImage((int)Images.DayImageBackground).color = Constants.Colors.DayBackground;

            GetImage((int)Images.SunAndMoonImage).sprite = Resources.Load<Sprite>(Constants.Sprites.Sun);
        }
    }

    private void UpdateDayUI()
    {
        GetText((int)Texts.DayText).text = DayManager.Instance.CurrentDay.ToString();
        Image sunOrMoon = GetImage((int)Images.SunAndMoonImage);
        float ratio = DayManager.Instance.RatioOfDayNight;

        Vector2 controlPoint = new Vector2(_rightOfSun * 0.5f, _topOfSun * 2);
        Vector2 destPoint = new Vector2(_rightOfSun, 0);

        Vector2 pos1 = Vector2.Lerp(Vector2.zero, controlPoint, ratio);
        Vector2 pos2 = Vector2.Lerp(controlPoint, destPoint, ratio);
        Vector2 position = Vector2.Lerp(pos1, pos2, ratio);
        sunOrMoon.rectTransform.anchoredPosition = position;
    }

    public void ActiveMasteryButton(SkullController CurrentSkull)
    {
        GetButton((int)Buttons.MasteryButton).gameObject.SetActive(true);
        this._currentSkull = CurrentSkull;
    }

    public void UnactiveMasteryButton()
    {
        GetButton((int)Buttons.MasteryButton).gameObject.SetActive(false);
    }

    public void ActiveCooltimeUI(NewEquip equip)
    {
        Get<RectTransform>((int)RectTransforms.CooltimeHolder).gameObject.SetActive(true);

        GetImage((int)Images.ActionCooltime).sprite = equip.Action.GetIcon();
        GetImage((int)Images.ActionCooltime).fillAmount = equip.Action.GetCooltimePercentage();
        GetImage((int)Images.Skill1Cooltime).sprite = equip.Skill1.GetIcon();
        GetImage((int)Images.Skill1Cooltime).fillAmount = equip.Skill1.GetCooltimePercentage();
        GetImage((int)Images.Skill2Cooltime).sprite = equip.Skill2.GetIcon();
        GetImage((int)Images.Skill2Cooltime).fillAmount = equip.Skill2.GetCooltimePercentage();
    }

    public void UnactiveCooltimeUI()
    {
        Get<RectTransform>((int)RectTransforms.CooltimeHolder).gameObject.SetActive(false);
    }

    public void UpdateCooltimeUI(NewEquip equip)
    {
        if (equip.Action.RecentTime > 0)
        {
            GetImage((int)Images.ActionCooltime).fillAmount = equip.Action.GetCooltimePercentage();
            GetText((int)Texts.CooltimeActionText).gameObject.SetActive(true);
            GetText((int)Texts.CooltimeActionText).text = $"{Mathf.Ceil(equip.Action.RecentTime)}";
        }
        else
        {
            GetImage((int)Images.ActionCooltime).fillAmount = 1;
            GetText((int)Texts.CooltimeActionText).gameObject.SetActive(false);
        }

        if (equip.Skill1.RecentTime > 0)
        {
            GetImage((int)Images.Skill1Cooltime).fillAmount = equip.Skill1.GetCooltimePercentage();
            GetText((int)Texts.Cooltime1Text).gameObject.SetActive(true);
            GetText((int)Texts.Cooltime1Text).text = $"{Mathf.Ceil(equip.Skill1.RecentTime)}";
        }
        else
        {
            GetImage((int)Images.Skill1Cooltime).fillAmount = 1;
            GetText((int)Texts.Cooltime1Text).gameObject.SetActive(false);
        }

        if (equip.Skill2.RecentTime > 0)
        {
            GetImage((int)Images.Skill2Cooltime).fillAmount = equip.Skill2.GetCooltimePercentage();
            GetText((int)Texts.Cooltime2Text).gameObject.SetActive(true);
            GetText((int)Texts.Cooltime2Text).text = $"{Mathf.Ceil(equip.Skill2.RecentTime)}";
        }
        else
        {
            GetImage((int)Images.Skill2Cooltime).fillAmount = 1;
            GetText((int)Texts.Cooltime2Text).gameObject.SetActive(false);
        }
    }

    private void ShowMenuPopup()
    {
        if (Managers.UI.PeekPopupUI<UI_Popup>() is not UI_Settings)
        {
            Managers.Sound.PlaySound(Data.SoundType.Interaction);
            Managers.UI.ShowPopupUI<UI_Settings>();
        }
    }

    private void ShowMasteryPopup()
    {
        if (Managers.UI.PeekPopupUI<UI_Popup>() is not UI_Mastery)
        {
            _currentSkull?.MasteryController.ShowMasteryPopup();
        }
    }

    private bool ShowSkillTooltip()
    {
        if (_currentSkull == null)
        {
            return false;
        }

        if (_currentSkull.CurrentEquip == null)
        {
            return false;
        }

        _skillTooltip.gameObject.SetActive(true);


        return true;
    }

    private void ShowActionTooltip()
    {
        if (!ShowSkillTooltip()) { return; }

        _skillTooltip.UpdateUIState(_currentSkull.CurrentEquip.Action);
    }

    private void ShowSkill1()
    {
        if (!ShowSkillTooltip()) { return; }

        _skillTooltip.UpdateUIState(_currentSkull.CurrentEquip.Skill1);
    }

    private void ShowSkill2()
    {
        if (!ShowSkillTooltip()) { return; }

        _skillTooltip.UpdateUIState(_currentSkull.CurrentEquip.Skill2);
    }

    private void HideSkillTooltip()
    {
        if (_skillTooltip == null) { return; }

        _skillTooltip.gameObject.SetActive(false);
    }

    private void UpdateUIBySkullStartedControl(bool isControlled)
    {
        if (!isControlled)
        {
            HideSkillTooltip();
            Managers.GameManager.Player.CurrentSkull.OnEquipEvent -= UpdateEquipUI;
        }

        Managers.GameManager.Player.CurrentSkull.OnEquipEvent += UpdateEquipUI;
    }

    private void UpdateEquipUI(NewEquip equip)
    {
        if (equip == null)
        {
            HideSkillTooltip();
        }
    }
}