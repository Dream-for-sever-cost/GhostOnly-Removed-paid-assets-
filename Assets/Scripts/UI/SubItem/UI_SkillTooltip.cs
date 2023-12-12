using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Text = TMPro.TextMeshProUGUI;

public class UI_SkillTooltip : UI_Base
{
    // todo 툴팁 UI_Game 위에 올리기
    private enum Texts
    {
        SkillNameText,
        SkillDescriptions,
        SkillCoolTimeValueText,
        SkillCoolTimeText
    }

    private enum Images
    {
        SkillTooltipBackground,
        SkillIconImage,
        SkillKeyImage,
    }

    //Views
    private Text _nameText;
    private Text _descriptionsText;
    private Text _coolTimeValueText;
    private Text _coolTimeText;

    private Image _window;
    private Image _iconImage;
    private Image _keyImage;
    private bool _isChanged;

    private EquipAction _currentAction = null;

    private Vector3 _windowPosition = Vector3.zero;

    public override bool Init()
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        _window = GetImage((int)Images.SkillTooltipBackground);
        _iconImage = GetImage((int)Images.SkillIconImage);
        _keyImage = GetImage((int)Images.SkillKeyImage);

        _nameText = GetText((int)Texts.SkillNameText);
        _descriptionsText = GetText((int)Texts.SkillDescriptions);
        _coolTimeValueText = GetText((int)Texts.SkillCoolTimeValueText);
        _coolTimeText = GetText((int)Texts.SkillCoolTimeText);

        _init = true;
        return true;
    }

    private void Update()
    {
        if (!_isChanged) { return; }

        _isChanged = false;
        UpdateUI();
    }

    public void SetWindowPosition(Vector3 position)
    {
        _windowPosition = position;
    }

    public void UpdateUIState(EquipAction _action)
    {
        _currentAction = _action;
        _isChanged = true;
    }

    private void UpdateUI()
    {
        if (_currentAction == null) { return; }

        _nameText.text = GetActionName(_currentAction.UIInfo);
        _descriptionsText.text = GetActionInfo(_currentAction.UIInfo);
        _coolTimeText.text = GetString(Constants.EquipAction.Cooltime);
        _coolTimeValueText.text = _currentAction.Cooltime.ToString("N1");
        _iconImage.sprite = _currentAction.GetIcon();
        _keyImage.sprite = GetKeyIcon(_currentAction.UIInfo);
    }

    private string GetActionName(ActionType type)
    {
        return type switch
        {
            ActionType.SwordAction => GetString(Constants.EquipAction.SwordAction),
            ActionType.SwordSkill1 => GetString(Constants.EquipAction.SwordSkill1),
            ActionType.SwordSkill2 => GetString(Constants.EquipAction.SwordSkill2),
            ActionType.BowAction => GetString(Constants.EquipAction.BowAction),
            ActionType.BowSkill1 => GetString(Constants.EquipAction.BowSkill1),
            ActionType.BowSkill2 => GetString(Constants.EquipAction.BowSkill2),
            ActionType.WandAction => GetString(Constants.EquipAction.WandAction),
            ActionType.WandSkill1 => GetString(Constants.EquipAction.WandSkill1),
            ActionType.WandSkill2 => GetString(Constants.EquipAction.WandSkill2),
            ActionType.LampAction => GetString(Constants.EquipAction.LampAction),
            ActionType.LampSkill1 => GetString(Constants.EquipAction.LampSkill1),
            ActionType.LampSkill2 => GetString(Constants.EquipAction.LampSkill2),
            _ => string.Empty,
        };
    }

    private string GetActionInfo(ActionType type)
    {
        return type switch
        {
            ActionType.SwordAction => GetString(Constants.EquipAction.SwordActionInfo),
            ActionType.SwordSkill1 => GetString(Constants.EquipAction.SwordSkill1Info),
            ActionType.SwordSkill2 => GetString(Constants.EquipAction.SwordSkill2Info),
            ActionType.BowAction => GetString(Constants.EquipAction.BowActionInfo),
            ActionType.BowSkill1 => GetString(Constants.EquipAction.BowSkill1Info),
            ActionType.BowSkill2 => GetString(Constants.EquipAction.BowSkill2Info),
            ActionType.WandAction => GetString(Constants.EquipAction.WandActionInfo),
            ActionType.WandSkill1 => GetString(Constants.EquipAction.WandSkill1Info),
            ActionType.WandSkill2 => GetString(Constants.EquipAction.WandSkill2Info),
            ActionType.LampAction => GetString(Constants.EquipAction.LampActionInfo),
            ActionType.LampSkill1 => GetString(Constants.EquipAction.LampSkill1Info),
            ActionType.LampSkill2 => GetString(Constants.EquipAction.LampSkill2Info),
            _ => string.Empty,
        };
    }

    private Sprite GetKeyIcon(ActionType type)
    {
        return type switch
        {
            ActionType.SwordAction => Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[Constants.EquipAction.IconAction],
            ActionType.SwordSkill1 => Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[Constants.EquipAction.IconSkill1],
            ActionType.SwordSkill2 => Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[Constants.EquipAction.IconSkill2],
            ActionType.BowAction => Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[Constants.EquipAction.IconAction],
            ActionType.BowSkill1 => Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[Constants.EquipAction.IconSkill1],
            ActionType.BowSkill2 => Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[Constants.EquipAction.IconSkill2],
            ActionType.WandAction => Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[Constants.EquipAction.IconAction],
            ActionType.WandSkill1 => Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[Constants.EquipAction.IconSkill1],
            ActionType.WandSkill2 => Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[Constants.EquipAction.IconSkill2],
            ActionType.LampAction => Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[Constants.EquipAction.IconAction],
            ActionType.LampSkill1 => Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[Constants.EquipAction.IconSkill1],
            ActionType.LampSkill2 => Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[Constants.EquipAction.IconSkill2],
            _ => null,
        };
    }

    //private Sprite SkillToIcon(Skill.ESkillType skillType)
    //{
    //    return Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[(int)skillType + Constants.Offset.SkillIconStartOffset];
    //}

    //private Sprite GetSkillKeyMap(int skillIndex)
    //{
    //    //todo 키 변경 가능하다면 ? 
    //    return Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[skillIndex + Constants.Offset.SkillKeyMapOffset];
    //}
}