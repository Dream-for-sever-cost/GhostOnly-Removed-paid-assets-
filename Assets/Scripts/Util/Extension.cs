using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;
using Component = UnityEngine.Component;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Utils.GetOrAddComponent<T>(go);
    }

    public static bool TryGetComponentInChildren<T>(this GameObject go, out T ret) // where T : UnityEngine.Component
    {
        ret = go.GetComponentInChildren<T>(go);
        return ret != null;
    }

    public static void BindEvent(this GameObject go, Action action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    public static void BindEvent(this UnityEngine.Component component, Action action,
        Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(component.gameObject, action, type);
    }

    public static int ToInt(this float value) => (int)value;

    public static Color Color(this Mastery.MasteryGrade grade) => grade switch
    {
        Mastery.MasteryGrade.Normal => Constants.Colors.NormalColor,
        Mastery.MasteryGrade.Rare => Constants.Colors.RareColor,
        Mastery.MasteryGrade.Epic => Constants.Colors.EpicColor,
        Mastery.MasteryGrade.Legend => Constants.Colors.LegendColor,
        _ => throw new ArgumentOutOfRangeException(nameof(grade), grade, null)
    };

    public static string ToStatHeaderId(this StatType statType)
    {
        return statType switch
        {
            StatType.MaxHP => Constants.StringRes.MaxHpId,
            StatType.MoveSpeed => Constants.StringRes.MoveSpeedId,
            StatType.Strength => Constants.StringRes.StrengthId,
            StatType.Dexterity => Constants.StringRes.DexterityId,
            StatType.Intelligence => Constants.StringRes.IntelligenceId,
            StatType.CriticalRate => Constants.StringRes.CriticalId,
            StatType.ActionDelay => Constants.StringRes.ActionDelayId,
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
        };
    }

    public static int LastIndex(this ICollection collection)
    {
        return collection.Count - 1;
    }
}