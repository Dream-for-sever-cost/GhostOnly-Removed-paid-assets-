using UnityEngine;
using UnityEngine.UIElements;
using Util;
using Text = TMPro.TextMeshProUGUI;

public class UI_MasteryTooltip : UI_Base
{
    private enum Texts
    {
        MasteryNameText,
        MasteryGradeText,
        MasteryEffect1Text,
        MasteryEffect2Text
    }

    private enum Images
    {
        MasteryIconImage,
        MasteryStatImage,
        Background,
    }

    private const int MAX_MASTERY_EFFECT_COUNT = 2;
    private Mastery _mastery;
    private readonly Text[] _masteryEffectTexts = new Text[MAX_MASTERY_EFFECT_COUNT];
    private Vector2 _position = Vector2.zero;

    public override bool Init()
    {
        _init = true;
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        _masteryEffectTexts[0] = GetText((int)Texts.MasteryEffect1Text);
        _masteryEffectTexts[1] = GetText((int)Texts.MasteryEffect2Text);
        UpdateUI();
        return true;
    }

    public void SetMastery(Mastery mastery)
    {
        _mastery = mastery;
    }

    public void HideTooltip()
    {
        //todo optimization
        Destroy(gameObject);
    }

    public void SetPosition(Vector2 position)
    {
        _position = position;
    }

    private void UpdateUI()
    {
        //todo REFACTOR this path
        GetImage((int)Images.Background).rectTransform.anchoredPosition = _position;
        GetImage((int)Images.MasteryIconImage).sprite =
            Resources.LoadAll<Sprite>(Constants.Sprites.Mastery)[(int)_mastery.IconName];

        GetText((int)Texts.MasteryNameText).text = GetString(_mastery.Name);
        GetText((int)Texts.MasteryGradeText).text = GetString(_mastery.Grade.ToString());
        int iconIndex = ((int)_mastery.Type * Constants.Offset.StatImageMultiOffset) + Constants.Offset.StatIconOffset;
        GetImage((int)Images.MasteryStatImage).sprite = Resources.LoadAll<Sprite>(Constants.Sprites.Icon)[iconIndex];

        for (int i = 0; i < _mastery.Effects.Count; i++)
        {
            string formatString = GetString(_mastery.Effects[i].StringRes());
            string desc = _mastery.Effects[i].ToFormatString(formatString);
            _masteryEffectTexts[i].text = desc;
        }
    }
}