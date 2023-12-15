using System;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TextMeshProUGUI;
using DG.Tweening;
using System.Text;
using TMPro;
using UnityEngine.UI;
using Util;

public sealed class UI_Mastery : UI_Popup
{
    private enum MasteryRectTransforms : short
    {
        AppliedStatsContainer,
        Background
    }

    private enum MasteryButtons : short
    {
        CloseButton,
        MainOpenButton,
        SubOpenButton,
        MainLockButton,
        SubLockButton,
    }

    private enum StatsButtons : short
    {
        AddStrButton,
        AddDexButton,
        AddIntButton
    }

    private enum MasteryImages
    {
        MainMasteryIconImage,
        SubMasteryIconImage,
        MainMasteryBorderImage,
        SubMasteryBorderImage,
    }

    private enum MasteryTexts
    {
        //TitleText,
        //Status,
        //Mastery,
        MainMasteryNameText,
        SubMasteryNameText,
        StrengthText,
        DexterityText,
        IntelligenceText,
    }

    public enum StatusType
    {
        Atk,
        MaxHp,
        CriticalRate,
        MovementSpd,
        ActionDelay
    }

    public struct StatusInfo
    {
        public string Header;
        public string ValueString;
        public string AddedValueString;
    }

    //====View
    private RectTransform _window;
    private UI_MasteryTooltip _currentShownTooltip;
    private Dictionary<StatType, AddStatsButton> _addStatsButtons;


    //====Controller
    private SlaveMasteryController _slaveMasteryController;


    //====State 
    private Mastery _mainMastery;
    private Mastery _subMastery;
    private bool _isMainOpened;
    private bool _isSubOpened;
    private int _currentStrengthStats;
    private int _currentDexStats;
    private int _currentIntStats;
    private int CurrentStrengthPrice => _costs[StatType.Strength];
    private int CurrentDexPrice => _costs[StatType.Dexterity];
    private int CurrentIntPrice => _costs[StatType.Intelligence];

    private bool _isStateChanged;
    private bool _canBuy;
    private IReadOnlyDictionary<StatType, int> _costs;
    private IReadOnlyList<MasteryEffect> _effects = new List<MasteryEffect>();
    private IReadOnlyList<StatusInfo> _infos = new List<StatusInfo>();
    private bool _isSubscribing;

    //Tween Animation
    private Sequence _openSequence;
    private Sequence _closeSequence;
    private Sequence _mainOpenSequence;
    private Sequence _subOpenSequence;
    private Sequence _mainTextSequence;
    private Sequence _subTextSequence;
    private Sequence _mainColorSequence;
    private Sequence _subColorSequence;
    private Sequence _mainDelaySequence;
    private Sequence _subDelaySequence;
    private Sequence _mainLockOpenSequence;
    private Sequence _subLockOpenSequence;
    private Sequence _mainOpenButtonSequence;
    private Sequence _subOpenButtonSequence;

    public override bool Init()
    {
        Bind<RectTransform>(typeof(MasteryRectTransforms));
        _window = Get<RectTransform>((int)MasteryRectTransforms.Background);
        Bind<AddStatsButton>(typeof(StatsButtons));
        BindImage(typeof(MasteryImages));
        BindText(typeof(MasteryTexts));
        BindButton(typeof(MasteryButtons));
        GetButton((int)MasteryButtons.CloseButton).onClick.AddListener(Close);
        GetButton((int)MasteryButtons.MainOpenButton).onClick.AddListener(SelectMainMastery);
        GetButton((int)MasteryButtons.SubOpenButton).onClick.AddListener(SelectSubMastery);
        GetButton((int)MasteryButtons.MainLockButton).onClick.AddListener(FailOpenMastery);
        GetButton((int)MasteryButtons.SubLockButton).onClick.AddListener(FailOpenMastery);

        GetImage((int)MasteryImages.MainMasteryIconImage).BindEvent(ShowMainMastery, Define.UIEvent.PointerEnter);
        GetImage((int)MasteryImages.MainMasteryIconImage).BindEvent(HideMainMastery, Define.UIEvent.PointerExit);
        GetImage((int)MasteryImages.SubMasteryIconImage).BindEvent(ShowSubMastery, Define.UIEvent.PointerEnter);
        GetImage((int)MasteryImages.SubMasteryIconImage).BindEvent(HideSubMastery, Define.UIEvent.PointerExit);

        _window.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        _addStatsButtons = new Dictionary<StatType, AddStatsButton>();
        foreach ((StatType type, int _) in _costs)
        {
            switch (type)
            {
                case StatType.Strength:
                    _addStatsButtons[type] = Get<AddStatsButton>(type - StatType.Strength);
                    break;
                case StatType.Dexterity:
                    _addStatsButtons[type] = Get<AddStatsButton>(type - StatType.Strength);
                    break;
                case StatType.Intelligence:
                    _addStatsButtons[type] = Get<AddStatsButton>(type - StatType.Strength);
                    break;
            }

            _addStatsButtons[type].AddStatsToUndead = _slaveMasteryController.BuyStats;
            _addStatsButtons[type].addType = type;
        }

        _init = true;
        UpdateUI();
        SetOpenSequence();
        return true;
    }


    private void UpdateUI()
    {
        //Title, Subtitles, Etc -> Localization
        //GetText((int)MasteryTexts.TitleText).text = GetString(Constants.StringRes.ResIdStatus);
        //GetText((int)MasteryTexts.Status).text = GetString(Constants.StringRes.ResIdAbility);
        //GetText((int)MasteryTexts.Mastery).text = GetString(Constants.StringRes.ResIdMastery);
        DOTween.KillAll();
        GetButton((int)MasteryButtons.MainOpenButton).gameObject.SetActive(!_isMainOpened);
        GetButton((int)MasteryButtons.SubOpenButton).gameObject.SetActive(!_isSubOpened);
        GetImage((int)MasteryImages.MainMasteryBorderImage).gameObject.SetActive(_isMainOpened);
        GetImage((int)MasteryImages.SubMasteryBorderImage).gameObject.SetActive(_isSubOpened);

        //======Stats=====//
        GetText((int)MasteryTexts.StrengthText).text = _currentStrengthStats.ToString("N0");
        GetText((int)MasteryTexts.DexterityText).text = _currentDexStats.ToString("N0");
        GetText((int)MasteryTexts.IntelligenceText).text = _currentIntStats.ToString("N0");

        //======Stats Price=====/
        Get<AddStatsButton>((int)StatsButtons.AddStrButton).UpdatePriceText(CurrentStrengthPrice);
        Get<AddStatsButton>((int)StatsButtons.AddDexButton).UpdatePriceText(CurrentDexPrice);
        Get<AddStatsButton>((int)StatsButtons.AddIntButton).UpdatePriceText(CurrentIntPrice);

        if (_isMainOpened)
        {
            //===== Main Mastery =====//
            GetText((int)MasteryTexts.MainMasteryNameText).text = GetString(_mainMastery.Name);
            SetMainMasterySequence();

            Sprite spriteAsset = Resources.LoadAll<Sprite>(Constants.Sprites.Mastery)[(int)_mainMastery.IconName];
            if (spriteAsset != null)
            {
                GetImage((int)MasteryImages.MainMasteryIconImage).sprite = spriteAsset;
            }
        }

        if (_isSubOpened)
        {
            //===== Sub Mastery =====//
            GetText((int)MasteryTexts.SubMasteryNameText).text = GetString(_subMastery.Name);
            SetSubMasterySequence();
            Sprite spriteAsset = Resources.LoadAll<Sprite>(Constants.Sprites.Mastery)[(int)_subMastery.IconName];
            if (spriteAsset != null)
            {
                GetImage((int)MasteryImages.SubMasteryIconImage).sprite = spriteAsset;
            }
        }

        //===== Can Open =====//
        bool canMainOpen = _slaveMasteryController.CanOpenMainMastery();
        bool canSubOpen = _slaveMasteryController.CanOpenSubMastery();
        GetButton((int)MasteryButtons.MainOpenButton).enabled = canMainOpen;
        SetMainMasteryLock(canMainOpen);
        GetButton((int)MasteryButtons.SubOpenButton).enabled = canSubOpen;
        SetSubMasteryLock(canSubOpen);

        //===== Effect Lines =====//      
        ShowStatusInfos();
    }

    private void ShowStatusInfos()
    {
        //todo optimization
        RectTransform parent = Get<RectTransform>((int)MasteryRectTransforms.AppliedStatsContainer);
        foreach (Text child in parent.GetComponentsInChildren<Text>())
        {
            Destroy(child.gameObject);
        }

        //todo add status data 
        foreach (StatusInfo statusInfo in _infos)
        {
            Text textAsset = Resources.Load<Text>(Constants.Prefabs.AppliedEffectItem);
            Text text = Instantiate(textAsset, parent, false);
            text.text = ToStatusString(statusInfo);
        }
    }

    public void UpdateUIState(
        Mastery mainMastery,
        Mastery subMastery,
        bool isMainOpened,
        bool isSubOpened,
        int strStats,
        int dexStats,
        int intStats,
        Sprite weaponSprite,
        IReadOnlyDictionary<StatType, int> costs,
        SlaveMasteryController masteryController,
        IReadOnlyList<MasteryEffect> effects = null)
    {
        _mainMastery = mainMastery;
        _subMastery = subMastery;
        _isMainOpened = isMainOpened;
        _isSubOpened = isSubOpened;
        _currentStrengthStats = strStats;
        _currentDexStats = dexStats;
        _currentIntStats = intStats;
        _slaveMasteryController = masteryController;

        if (!_isSubscribing)
        {
            _slaveMasteryController.OnBuySuccess += UpdateStatUI;
            _slaveMasteryController.OnFailedToBuyEvent += AlertFailed;
            _slaveMasteryController.OnMainMasteryOpened += UpdateMainMasteryState;
            _slaveMasteryController.OnSubMasteryOpened += UpdateSubMasteryState;
            _isSubscribing = true;
        }

        _costs = costs;
        if (effects != null)
        {
            _effects = effects;
        }

        if (_init)
        {
            UpdateUI();
        }
    }

    public void UpdateUIState(
        Mastery mainMastery,
        Mastery subMastery,
        bool isMainOpened,
        bool isSubOpened,
        int strStats,
        int dexStats,
        int intStats,
        Sprite weaponSprite,
        IReadOnlyDictionary<StatType, int> costs,
        SlaveMasteryController masteryController,
        IReadOnlyList<StatusInfo> infos,
        IReadOnlyList<MasteryEffect> effects = null)
    {
        _infos = infos;
        UpdateUIState(mainMastery, subMastery, isMainOpened, isSubOpened, strStats, dexStats, intStats, weaponSprite,
            costs, masteryController, effects);
    }


    private void UpdateMainMasteryState(Mastery mastery)
    {
        _mainMastery = mastery;
        _isMainOpened = _mainMastery != Mastery.None;
        _effects = _slaveMasteryController.Effects;
        _infos = _slaveMasteryController.GetStatusInfos();
        UpdateUI();
    }

    private void UpdateSubMasteryState(Mastery mastery)
    {
        _subMastery = mastery;
        _isSubOpened = _subMastery != Mastery.None;
        _effects = _slaveMasteryController.Effects;
        _infos = _slaveMasteryController.GetStatusInfos();
        UpdateUI();
    }

    private void FailOpenMastery()
    {
        AlertFailed();
        Managers.Sound.PlaySound(Data.SoundType.PurchaseFail);
    }

    private void SelectMainMastery()
    {
        ShowSelectWindow(MasteryManager.EMasteryOpenType.Main);
        Managers.Sound.PlaySound(Data.SoundType.Interaction);
    }

    private void SelectSubMastery()
    {
        ShowSelectWindow(MasteryManager.EMasteryOpenType.Sub);
        Managers.Sound.PlaySound(Data.SoundType.Interaction);
    }

    private void ShowSelectWindow(MasteryManager.EMasteryOpenType openType)
    {
        UI_SelectMastery window = Managers.UI.ShowPopupUI<UI_SelectMastery>();
        window.Setup(_slaveMasteryController, openType);

        // todo add click sfx 
    }

    private void UpdateStatUI(StatType statType, bool isMainOpened, bool isSubOpened, int stats, int price)
    {
        switch (statType)
        {
            case StatType.Strength:
                _currentStrengthStats = stats;
                break;
            case StatType.Dexterity:
                _currentDexStats = stats;
                break;
            case StatType.Intelligence:
                _currentIntStats = stats;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
        }

        _infos = _slaveMasteryController.GetStatusInfos();
        UpdateUI();
    }

    private void Close()
    {
        SequenceKill();
        Managers.Sound.PlaySound(Data.SoundType.Click);
        ClosePopupUI();
    }

    private void OnDestroy()
    {
        if (_currentShownTooltip != null)
        {
            _currentShownTooltip.HideTooltip();
        }

        _slaveMasteryController.OnMainMasteryOpened -= UpdateMainMasteryState;
        _slaveMasteryController.OnSubMasteryOpened -= UpdateSubMasteryState;
        _slaveMasteryController.OnFailedToBuyEvent -= AlertFailed;
        _slaveMasteryController.OnBuySuccess -= UpdateStatUI;
    }

    private void AlertFailed()
    {
        Transform windowTrans = _window.transform;
        windowTrans.DOShakePosition(
            Constants.MasteryAnimation.AlertFailShakeDuration,
            Constants.MasteryAnimation.AlertFailShakeStrength,
            Constants.MasteryAnimation.AlertFailShakeVivrato
        ).SetUpdate(true);
    }

    private void SetOpenSequence()
    {
        GameObject background = Get<RectTransform>((int)MasteryRectTransforms.Background).gameObject;
        _openSequence = DOTween.Sequence().OnStart(() =>
            {
                background.transform.localScale = Vector3.zero;
            })
            .Append(background.transform.DOScale(1, Constants.MasteryAnimation.OpenAppendScaleDuration)).SetEase(Ease.Linear)
            .Join(background.GetComponent<CanvasGroup>().DOFade(1, Constants.MasteryAnimation.OpenJoinFadeDuration))
            .SetUpdate(true);
    }

    private void MainLockOpenSequence()
    {
        GameObject lockData = GetButton((int)MasteryButtons.MainLockButton).gameObject;
        _mainLockOpenSequence = DOTween.Sequence()
            .Prepend(lockData.transform.DOShakePosition(Constants.MasteryAnimation.MasteryShakeDuration,
                Constants.MasteryAnimation.MasteryShakeStrength
                , Constants.MasteryAnimation.MasteryVivrato))
            .Append(lockData.transform.GetComponent<CanvasGroup>().DOFade(0f, Constants.MasteryAnimation.MasteryFadeDuration))
            .SetUpdate(true);

        GetText((int)MasteryTexts.MainMasteryNameText).text = "OPEN !";
    }

    private void SubLockOpenSequence()
    {
        GameObject lockData = GetButton((int)MasteryButtons.SubLockButton).gameObject;
        _subLockOpenSequence = DOTween.Sequence()
            .Prepend(lockData.transform.DOShakePosition(Constants.MasteryAnimation.MasteryShakeDuration,
                Constants.MasteryAnimation.MasteryShakeStrength
                , Constants.MasteryAnimation.MasteryVivrato))
            .Append(lockData.transform.GetComponent<CanvasGroup>().DOFade(0f, Constants.MasteryAnimation.MasteryFadeDuration))
            .SetUpdate(true);

        GetText((int)MasteryTexts.SubMasteryNameText).text = "OPEN !";
    }

    private void SetMainMasteryLock(bool canMainOpen)
    {
        if (!_slaveMasteryController.MainMasteryLockOpen)
        {
            if (!canMainOpen && !_slaveMasteryController.MainMasteryLockOpen)
            {
                GetButton((int)MasteryButtons.MainLockButton).gameObject.SetActive(true);
            }
            else
            {
                Managers.Sound.PlaySound(Data.SoundType.Hit);
                _slaveMasteryController.MainMasteryLockOpen = true;
                MainLockOpenSequence();
                MainOpenButtonSequence();
                _mainDelaySequence = DOTween
                    .Sequence()
                    .Prepend(DOVirtual.DelayedCall(Constants.MasteryAnimation.MasteryImageOffDelay, MainLockImageOff))
                    .SetUpdate(true);
            }
        }
        else
        {
            if (!_isMainOpened) 
            {
                MainOpenButtonSequence();
                GetText((int)MasteryTexts.MainMasteryNameText).text = "OPEN !";
            }
            MainLockImageOff();
        }
    }

    private void MainLockImageOff()
    {
        GetButton((int)MasteryButtons.MainLockButton).gameObject.SetActive(false);
    }

    private void SetSubMasteryLock(bool canSubOpen)
    {
        if (!_slaveMasteryController.SubMasteryLockOpen)
        {
            if (!canSubOpen && !_slaveMasteryController.SubMasteryLockOpen)
            {
                GetButton((int)MasteryButtons.SubLockButton).gameObject.SetActive(true);
            }
            else
            {
                Managers.Sound.PlaySound(Data.SoundType.Hit);
                _slaveMasteryController.SubMasteryLockOpen = true;
                SubLockOpenSequence();
                SubOpenButtonSequence();
                _subDelaySequence = DOTween.Sequence()
                    .Prepend(DOVirtual.DelayedCall(Constants.MasteryAnimation.MasteryImageOffDelay, SubLockImageOff))
                    .SetUpdate(true);
            }
        }
        else
        {
            if (!_isSubOpened)
            {
                SubOpenButtonSequence();
                GetText((int)MasteryTexts.SubMasteryNameText).text = "OPEN !";
            }
            SubLockImageOff();
        }
    }

    private void SubLockImageOff()
    {
        GetButton((int)MasteryButtons.SubLockButton).gameObject.SetActive(false);
    }

    private void SetMainMasterySequence()
    {
        if (!_slaveMasteryController.MainMasteryOpen)
        {
            Managers.Sound.PlaySound(Data.SoundType.GetMastery);
            _slaveMasteryController.MainMasteryOpen = true;
            GetText((int)MasteryTexts.MainMasteryNameText).alpha = 0;
            Image image = GetImage((int)MasteryImages.MainMasteryBorderImage);

            _mainColorSequence = DOTween.Sequence()
                .Append(image.DOColor(_mainMastery.Grade.Color(), Constants.MasteryAnimation.MasteryColorDuration))
                .SetUpdate(true);
            _mainTextSequence = DOTween.Sequence();

            TextMeshProUGUI name = GetText((int)MasteryTexts.MainMasteryNameText);
            DOTweenTMPAnimator nameAnimator = new DOTweenTMPAnimator(name);

            for (int i = 0; i < name.text.Length; i++)
            {
                _mainTextSequence
                    .Append(nameAnimator.DOFadeChar(i, 1f, Constants.MasteryAnimation.MasteryTextFadeDuration))
                    .SetUpdate(true);
            }
        }
        else
        {
            GetImage((int)MasteryImages.MainMasteryBorderImage).color = _mainMastery.Grade.Color();
        }
    }

    private void SetSubMasterySequence()
    {
        if (!_slaveMasteryController.SubMasteryOpen)
        {
            Managers.Sound.PlaySound(Data.SoundType.GetMastery);
            _slaveMasteryController.SubMasteryOpen = true;

            GetText((int)MasteryTexts.SubMasteryNameText).alpha = 0;
            Image image = GetImage((int)MasteryImages.SubMasteryBorderImage);
            _subColorSequence = DOTween.Sequence()
                .Append(image.DOColor(_subMastery.Grade.Color(), Constants.MasteryAnimation.MasteryColorDuration))
                .SetUpdate(true);
            _subTextSequence = DOTween.Sequence().SetUpdate(true);
            TextMeshProUGUI name = GetText((int)MasteryTexts.SubMasteryNameText);
            DOTweenTMPAnimator nameAnimator = new DOTweenTMPAnimator(name);

            for (int i = 0; i < name.text.Length; i++)
            {
                _subTextSequence.Append(nameAnimator.DOFadeChar(i, 1f, Constants.MasteryAnimation.MasteryTextFadeDuration));
            }
        }
        else
        {
            GetImage((int)MasteryImages.SubMasteryBorderImage).color = _subMastery.Grade.Color();
        }
    }

    private void MainOpenButtonSequence()
    {
        GameObject button = GetButton((int)MasteryButtons.MainOpenButton).gameObject;
        _mainOpenButtonSequence = DOTween.Sequence()
            .Append(button.transform.DOShakePosition(0.2f, 5, 30)).PrependInterval(1.1f)
            .SetEase(Ease.Linear)
            .SetLoops(-1)
            .SetUpdate(true);
    }

    private void SubOpenButtonSequence()
    {
        GameObject button = GetButton((int)MasteryButtons.SubOpenButton).gameObject;
        _subOpenButtonSequence = DOTween.Sequence()
            .Append(button.transform.DOShakePosition(0.2f, 5, 30)).PrependInterval(1.1f)
            .SetEase(Ease.Linear)
            .SetLoops(-1)
            .SetUpdate(true);
    }

    private void SequenceKill()
    {
        _mainTextSequence.Kill(true);
        _subTextSequence.Kill(true);
        _mainOpenSequence.Kill(true);
        _subOpenSequence.Kill(true);
        _openSequence.Kill(true);
        _closeSequence.Kill(true);
        _subColorSequence.Kill(true);
        _mainColorSequence.Kill(true);
        _mainDelaySequence.Kill(true);
        _subDelaySequence.Kill(true);
        _mainLockOpenSequence.Kill(true);
        _subLockOpenSequence.Kill(true);
        _mainOpenButtonSequence.Kill(true);
        _subOpenButtonSequence.Kill(true);
    }

    private string ToStatusString(StatusInfo statusInfo)
    {
        //todo 디자인 변경시 수정해야됨
        //StringBuilder sb = new StringBuilder();
        //sb.Append("= ");
        //sb.Append(GetString(statusInfo.Header));
        //sb.Append(" : ");
        //sb.Append(statusInfo.ValueString);
        //sb.Append($"{statusInfo.AddedValueString}");
        //return sb.ToString();

        return $"= {GetString(statusInfo.Header)} : {statusInfo.ValueString}{statusInfo.AddedValueString}";
    }

    private void ShowMainMastery()
    {
        Debug.Log("show main mastery");
        Rect iconRect = GetImage((int)MasteryImages.MainMasteryBorderImage).rectTransform.rect;
        float x = iconRect.xMax;
        float y = iconRect.yMax + Constants.Padding.PaddingNormal;
        SetMasteryTooltipVisibility(_mainMastery, new Vector2(x, y), true);
    }

    private void HideMainMastery()
    {
        Debug.Log("hide main mastery");
        SetMasteryTooltipVisibility(null, Vector2.zero, false);
    }

    private void ShowSubMastery()
    {
        Debug.Log("show sub mastery");
        Rect iconRect = GetImage((int)MasteryImages.SubMasteryBorderImage).rectTransform.rect;
        float x = iconRect.xMax;
        float y = iconRect.yMax + Constants.Padding.PaddingLarge;
        SetMasteryTooltipVisibility(_subMastery, new Vector2(x, y), true);
    }

    private void HideSubMastery()
    {
        Debug.Log("hide sub mastery");
        SetMasteryTooltipVisibility(null, Vector2.zero, false);
    }

    private void SetMasteryTooltipVisibility(Mastery mastery, Vector2 position, bool isVisible)
    {
        //todo object pooling 
        if (isVisible)
        {
            if (_currentShownTooltip != null)
            {
                _currentShownTooltip.HideTooltip();
            }

            _currentShownTooltip = Managers.UI.MakeSubItem<UI_MasteryTooltip>();
            _currentShownTooltip.SetMastery(mastery);
            _currentShownTooltip.SetPosition(position);
        }
        else
        {
            _currentShownTooltip.HideTooltip();
            _currentShownTooltip = null;
        }
    }
}