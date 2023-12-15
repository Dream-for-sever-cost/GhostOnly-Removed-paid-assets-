using Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [Serializable]
    public class JsonDataList<TK, TV>
    {
        public List<DataComponent<TK, TV>> data;
    }

    [Serializable]
    public class DataComponent<TK, TV>
    {
        public TK key;
        public TV value;
    }

    #region I18n

    [Serializable]
    public class I18nData
    {
        public string id;
        public string kr;
        public string en;
    }

    [Serializable]
    public class I18nDataLoader
    {
        public Dictionary<string, I18nData> dataDic = new Dictionary<string, I18nData>();
    }

    public enum Language
    {
        KOREAN = 0,
        ENGLISH = 1,
        NONE,
    }

    #endregion

    #region UnitStat

    [System.Serializable]
    public class UnitStatData
    {
        public string name;
        public float maxHp;
        public float moveSpeed;
        public float strength;
        public float dexterity;
        public float intelligence;
        public float criticalRate;
    }

    [System.Serializable]
    public class UnitStatDataLoader
    {
        public Dictionary<string, UnitStatData> dataDic = new Dictionary<string, UnitStatData>();
    }

    public enum UnitType
    {
        Skull,
        SwordMan,
        BowMan,
        WandMan,
        SwordHero,
        BowHero,
        WandHero,
        Guardian,
        Assassin,
        Ranger,
        Lancer,
        Wizard,
        Mage,
        Jack,
        Queen,
        King,
    }

    public static class MaxStat
    {
        public const float MAX_HP = 1000f;
        public const float DEFENSE = 20f;
        public const float MOVESPEED = 20f;
        public const float STRENGTH = 50f;
        public const float DEXTERITY = 50f;
        public const float INTELLIGENCE = 50f;
        public const float CRITICAL = 100f;
        public const float ACTION_DELAY = 3f;
    }

    public static class MinStat
    {
        public const float MIN_HP = 1f;
        public const float DEFENSE = 0f;
        public const float MOVESPEED = 0.125f;
        public const float STRENGTH = 1f;
        public const float DEXTERITY = 1f;
        public const float INTELLIGENCE = 1f;
        public const float CRITICAL = 0f;
        public const float ACTION_DELAY = 0.125f;
    }

    #endregion

    #region Mastery
    
    [Serializable]
    public class MasteryDataEntry
    {
        //이름	아이콘	등급	타입	효과1	수치 1	효과2	수치2
        public string Name;
        public string Id;
        public Mastery.MasteryGrade Grade;
        public Mastery.MasteryType Type;
        public Mastery.MasteryEffectType Effect1;
        public string EffectValue1;
        public Mastery.MasteryEffectType Effect2;
        public string EffectValue2;

        public Mastery ToMasteryDto()
        {
            MasteryEffect effect1 = Effect1 switch
            {
                Mastery.MasteryEffectType.None => new MasteryEffect(Mastery.MasteryEffectType.None),
                Mastery.MasteryEffectType.AddStatEffect => ParseAddStatEffect(EffectValue1),
                Mastery.MasteryEffectType.OnHitEffect => ParseOnHitEffect(EffectValue1),
                Mastery.MasteryEffectType.AuraEffect => ParseAuraEffect(EffectValue1),
                _ => throw new ArgumentOutOfRangeException()
            };
            MasteryEffect effect2 = Effect2 switch
            {
                Mastery.MasteryEffectType.None => new MasteryEffect(Mastery.MasteryEffectType.None),
                Mastery.MasteryEffectType.AddStatEffect => ParseAddStatEffect(EffectValue2),
                Mastery.MasteryEffectType.OnHitEffect => ParseOnHitEffect(EffectValue2),
                Mastery.MasteryEffectType.AuraEffect => ParseAuraEffect(EffectValue2),
                _ => throw new ArgumentOutOfRangeException()
            };

            return new Mastery(
                grade: Grade,
                type: Type,
                iconName: Enum.Parse<Mastery.MasteryId>(Id),
                name: Name,
                effects: new List<MasteryEffect>() { effect1, effect2 }
            );
        }

        private MasteryEffect ParseAddStatEffect(string dataString)
        {
            string[] effectValues = dataString.Split(":");
            StatType statType1 = Enum.Parse<StatType>(effectValues[0]);
            float value1 = float.Parse(effectValues[1]);
            return new MasteryAddStatEffect(
                Mastery.MasteryEffectType.AddStatEffect,
                statType1,
                value1,
                0f);
        }

        private MasteryEffect ParseOnHitEffect(string dataString)
        {
            OnHit.OnHitType onHitType = Enum.Parse<OnHit.OnHitType>(dataString);
            return new MasteryOnHitEffect(onHitType: onHitType, type: Mastery.MasteryEffectType.OnHitEffect);
        }

        private MasteryEffect ParseAuraEffect(string dataString)
        {
            Aura.AuraType auraType = Enum.Parse<Aura.AuraType>(dataString);
            return new MasteryAuraEffect(type: Mastery.MasteryEffectType.AuraEffect, auraType: auraType);
        }
    }

    #endregion

    #region Sound

    [System.Serializable]
    public class SoundData
    {
        public string soundName;
        public string soundPath;
    }
    
    public enum SoundType
    {
        SwordAction,
        BowAction,
        LampAction,
        WandAction,
        SwordSkill1,
        SwordSkill2,
        BowSkill1,
        BowSkill2,
        LampSkill1,
        LampSkill2,
        WandSkill1,
        WandSkill2,
        Click,
        Possess,
        Equip,
        Hit,
        Death,
        AltarHit,
        AltarInput,
        BossSkill1,
        Purchase,
        GravestoneHit,
        HeroHit,
        Interaction,
        SkullHit,
        StarcatchFail,
        StarcatchSuccess,
        PurchaseFail,
        ItemClick,
        GetMastery,
        DayBGM,
        NightBGM,
        LobbyBGM,
        GameOverBGM,
        GameClearBGM,
        SpellBookOpen,
        SpellBookClose,
        CoffinOpen,
        CoffinClose,
        MasteryOpen,
        MasteryClose,
        AltarLack,
        IntroBGM,
        Morning,
        CreditBGM,
        GetSpell,
    }

    #endregion

    #region Wave

    [Serializable]
    public class WaveResponseBody
    {
        public string wave;
        public string road1;
        public string road2;
        public string road3;

        public JsonWaveData ToDto()
        {
            JsonWaveData data = new JsonWaveData { wave = wave };
            string[] roadStrings = new string[] { road1, road2, road3 };
            foreach (string roadString in roadStrings)
            {
                Road road = new Road();
                foreach (string units in roadString.Split(","))
                {
                    if (string.IsNullOrEmpty(units))
                        continue;
                    string[] unitIdCount = units.Split('x');

                    if (int.TryParse(unitIdCount[0], out int unitId) && int.TryParse(unitIdCount[1], out int count))
                    {
                        road.roadUnitInfoDic.Add(new RoadUnitData() { type = (UnitType)unitId + 1, count = count });
                    }
                }

                data.roads.Add(road);
            }


            return data;
        }
    }

    [Serializable]
    public class JsonWaveData
    {
        //data transfer object 
        public string wave;
        public List<Road> roads = new List<Road>();
    }
    
    [Serializable]
    public class Road
    {
        public List<RoadUnitData> roadUnitInfoDic = new List<RoadUnitData>();
    }

    [Serializable]
    public class RoadUnitData
    {
        public UnitType type;
        public int count;
    }

    #endregion

    #region Spellbook

    [Serializable]
    public class SpellResponseBody
    {
        public string id;
        public string parentId;
        public string children;
        public string name;
        public string explanation;
        public float effect;
        public int price;
        public SpellType spellType;

        public SpellData ToDto()
        {
            SpellData data = new SpellData()
            {
                id = id,
                parentId = parentId,
                name = name,
                explanation = explanation,
                effect = effect,
                price = price,
                spellType = spellType
            };
            data.isLocked = !parentId.Equals("ROOT");

            if (string.IsNullOrEmpty(children))
            {
                return data;
            }

            foreach (string childId in children.Split(","))
            {
                if (string.IsNullOrEmpty(childId))
                    continue;

                data.childrens.Add(childId);
            }

            return data;
        }
    }

    [System.Serializable]
    public class SpellData
    {
        public string id;
        public string parentId;
        public List<string> childrens = new List<string>();
        public string name;
        public string explanation;
        public float effect;
        public int price;
        public SpellType spellType;
        public bool isLocked = false;
        public bool isActivated = false;
    }
    
    public enum SpellType
    {
        IncreaseSkullEnergyRecoverySpeed,
        IncreaseMaxSkullCount,
        IncreaseSkullDamage,
        IncreaseSkullMaxHp,
        IncreaseSkullMoveSpeed,
        IncreaseGhostMoveSpeed,
        ReduceGravestoneRespawnTime,
        IncreaseSoulYield,
    }

    #endregion

    #region Coffin

    [System.Serializable]
    public class CoffinData
    {
        public string coffinId;
        public CoffinType type;
        public string name;
        public int price;
        public string explanation;
    }
    
    public enum CoffinType
    {
        Skull,
        Equipment,
        Random,
    }

    #endregion

    #region Version

    class VersionResponseBody
    {
        public string Version;
        public string CreatedDate;
        public string BuildVersion;
        public string VersionDataRange;
    }

    class VersionDataResponseBody
    {
        public EFileNames Data;
        public string Path;
        public string Range;
        public string TestSheetId;
        public string TestRange;
    }

    #endregion
}