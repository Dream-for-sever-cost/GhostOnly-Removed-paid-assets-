using System;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

/// <summary>
/// 특성 랜덤 생성 및 특성 적용, 특성 가능여부를 알려주는 매니저 
/// </summary>
public class MasteryManager
{
    public enum EMasteryOpenType
    {
        Main,
        Sub
    }

    private DataManager _dataManager;

    public MasteryManager(DataManager dataManager)
    {
        _dataManager = dataManager;
    }

    public int GetCost(int currentValue)
    {
        return _dataManager.StatPriceDic[currentValue].price;
    }

    public bool CanCreateMastery(int strStats, int dexStats, int intStats, EMasteryOpenType openType)
    {
        int requireStatPoint = openType switch
        {
            EMasteryOpenType.Main => Constants.GameSystem.MainOpenLevel,
            EMasteryOpenType.Sub => Constants.GameSystem.SubOpenLevel,
            _ => throw new ArgumentOutOfRangeException(nameof(openType), openType, null)
        };
        return strStats >= requireStatPoint || dexStats >= requireStatPoint || intStats >= requireStatPoint;
    }

    public Mastery CreateStandardMastery(Mastery.MasteryType type)
    {
        return GetMasteryOfTypeAndGrade(type, Mastery.MasteryGrade.Normal);
    }

    public Mastery CreateRandomMastery(int strStats, int dexStats, int intStats, EMasteryOpenType openType)
    {
        if (!CanCreateMastery(strStats, dexStats, intStats, openType))
        {
            return Mastery.None;
        }

        Mastery.MasteryType[] types = Enum.GetValues(typeof(Mastery.MasteryType)) as Mastery.MasteryType[];
        float randomGradeValue = Random.Range(0, 1f);
        int randomTypeValue = Random.Range(0, types.Length);
        Mastery.MasteryType masteryType = types[randomTypeValue];
        Mastery.MasteryGrade grade = Mastery.MasteryGrade.Normal;

        if (randomGradeValue < Managers.GameManager.LegendPercent)
        {
            grade = Mastery.MasteryGrade.Legend;
        }
        else if (randomGradeValue < Managers.GameManager.EpicPercent)
        {
            grade = Mastery.MasteryGrade.Epic;
        }
        else if (randomGradeValue < Managers.GameManager.RarePercent)
        {
            grade = Mastery.MasteryGrade.Rare;
        }

        return GetMasteryOfTypeAndGrade(masteryType, grade);
    }

    public Mastery CreateRandomMastery(int strStats, int dexStats, int intStats, Mastery mainMastery,
        EMasteryOpenType openType)
    {
        bool isContinuous;
        Mastery mastery;
        int backoff = 0;
        do
        {
            mastery = CreateRandomMastery(strStats, dexStats, intStats, openType);
            backoff++;
            isContinuous = mastery.Grade >= Mastery.MasteryGrade.Epic && mastery.Name == mainMastery.Name;
        } while (isContinuous && backoff <= 10000);
        //todo make to constants

        return backoff >= 100000
            ? GetMasteryOfTypeAndGrade(mastery.Type, Mastery.MasteryGrade.Rare)
            : mastery;
    }


    private Mastery GetMasteryOfTypeAndGrade(Mastery.MasteryType masteryType, Mastery.MasteryGrade grade)
    {
        int randomIndex = Random.Range(0, int.MaxValue);
        List<string> idList = new List<string>();
        foreach ((string id, Mastery mastery) in _dataManager.MasteryEntries)
        {
            if (mastery.Type == masteryType && mastery.Grade == grade)
            {
                idList.Add(id);
            }
        }

        if (idList.Count == 0) { throw new NotSupportedException($"가지고 있지 않은 데이터입니다 - {masteryType}:{grade}"); }

        randomIndex %= idList.Count;
        return _dataManager.MasteryEntries[idList[randomIndex]];
    }

    private Mastery GetMasteryById(string id)
    {
        return _dataManager.MasteryEntries[id];
    }
}