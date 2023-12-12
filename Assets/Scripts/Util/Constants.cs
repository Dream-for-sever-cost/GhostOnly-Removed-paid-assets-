using Analytics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Util
{
    public static class Constants
    {
        public static class GameObjects
        {
            public const string UI_Game = "UI_Game";
            public const string Player = "Player(Clone)";
            public const string MinimapCamera = "MinimapCamera(Clone)";
        }

        public static class Prefabs
        {
            public const string Player = "Prefabs/Etc/Player";
            public const string DayManager = "Prefabs/Manager/DayManager";
            public const string Skull = "Prefabs/Skull/Skull";
            public const string MinimapCamera = "Prefabs/Minimap/MinimapCamera";
            public const string AppliedEffectItem = "Prefabs/UI/SubItem/AppliedEffectItem";
        }

        public static class Sprites
        {
            public const string Moon = "Sprites/Moon";
            public const string Sun = "Sprites/Sun";
            public const string Icon = "Sprites/UI/Icon";
            public const string Mastery = "Sprites/UI/Mastery";
        }

        public static class GameSystem
        {
            public const int AlterDevote = 1;
            public const int MaxAlter = 100;
            public const int AlterLow = 40;
            public const int AlterMid = 75;

            public const float HealAmount = 5f;
            public const float DecreaseHealthAmount = 0.5f;

            public const int MainOpenLevel = 10;
            public const int SubOpenLevel = 20;
        }

        public static class Pos
        {
            public static Vector2 FirstSkull = new Vector3(0, -4);
            public static Vector2 SecondSkull = new Vector3(-2.5f, -4);
            public static Vector2 CoffinSkull = new(5, -2);
            public static Vector2 CoffinEquip = new(5, -4);
        }

        public static class Probability
        {
            //===확률===// 
            public const float MasteryRare = 0.3f;
            public const float MasteryEpic = 0.125f;
            public const float MasteryLegend = 0.025f;
        }

        public static class Colors
        {
            public static Color NormalColor = Color.white;
            public static Color RareColor = Color.blue;
            public static Color EpicColor = Color.magenta;
            public static Color LegendColor = Color.yellow;
            public static Color HitColor = Color.red;
            public static Color RecoveryColor = Color.green;
            public static Color Transparent = new Color(1, 1, 1, 0);

            public static Color32 DayBackground = new Color32(200, 255, 255, 100);
            public static Color32 NightBackground = new Color32(60, 60, 60, 100);
            public static Color32 FullMoonBackground = new Color32(150, 30, 30, 100);

            public static Color AlterLow = new Color(1, 1, 1, 0);
            public static Color AlterMid = new Color(1, 1, 1, 0.5f);
        }

        public static class Speed
        {
            public const float GhostInitial = 5f;
            public const float ChainLightningSpeed = 10f;
        }

        public static class Distance
        {
            public const float ChainLightningDistance = 5.0f;
            public const float AuraRange = 3.0f;
            public const float FollowRange = 0.25f; //무기 사거리 + followRange 만큼의 거리만큼 추격함
            public const float ChaseDistance = 5f; //무기 사거리 + followRange 만큼의 거리만큼 추격함
        }

        public static class Time
        {
            public const float TimePerAuraTick = 0.5f;
            public const float AnimationTime = 0.5f;
            public const float BuffFlickeringTime = 0.25f;
            public const float SkullBaseHealTime = 0.5f;
            public const float SkullDamageTime = 1f;
            public const int TimeOutMilliseconds = 5000;
            public const float ToastLengthShort = 0.5f;
            public const float ToastLengthNormal = 1f;
            public const float ToastLengthLarge = 2f;
        }

        public static class AniParams
        {
            public const string Move = "Move";
            public const string Idle = "Idle";
            public const string Possess = "Possess";
            public const string Hit = "Hit";
            public const string Action = "Action";
            public const string Die = "Die";
            public const string Concentrate = "Concentrate";
            public const string Stop = "Stop";
            public const string Clear = "Clear";
            public const string Devote = "Devote";
            public const string Fail = "Fail";
            public const string CreditIdle = "CreditIdle";
            public const string CreditEnd = "CreditEnd";
            public const string ClearIdle = "ClearIdle";
            public const string AltarEffect = "OnEffect";
        }

        public static class Day
        {
            public const int SunRise = 0;
            public const int MoonRise = 25;
            public const int DayLength = 40;
            public const int FullMoonCycle = 7;
            public const float InverseDayScale = 0.5f; // 2의 역수
        }

        public static class EquipAction
        {
            public const string Cooltime = "cooltime";
            public const int IconAction = 42;
            public const int IconSkill1 = 38;
            public const int IconSkill2 = 39;

            public const string SwordAction = "sword_action";
            public const string SwordActionInfo = "sword_action_info";
            public const int SwordActionIconIndex = 8;
            public const string SwordSkill1 = "sword_skill1";
            public const string SwordSkill1Info = "sword_skill1_info";
            public const int SwordSkill1IconIndex = 9;
            public const string SwordSkill2 = "sword_skill2";
            public const string SwordSkill2Info = "sword_skill2_info";
            public const int SwordSkill2IconIndex = 10;

            public const string BowAction = "bow_action";
            public const string BowActionInfo = "bow_action_info";
            public const int BowActionIconIndex = 11;
            public const string BowSkill1 = "bow_skill1";
            public const string BowSkill1Info = "bow_skill1_info";
            public const int BowSkill1IconIndex = 12;
            public const string BowSkill2 = "bow_skill2";
            public const string BowSkill2Info = "bow_skill2_info";
            public const int BowSkill2IconIndex = 13;

            public const string WandAction = "wand_action";
            public const string WandActionInfo = "wand_action_info";
            public const int WandActionIconIndex = 14;
            public const string WandSkill1 = "wand_skill1";
            public const string WandSkill1Info = "wand_skill1_info";
            public const int WandSkill1IconIndex = 15;
            public const string WandSkill2 = "wand_skill2";
            public const string WandSkill2Info = "wand_skill2_info";
            public const int WandSkill2IconIndex = 16;

            public const string LampAction = "lamp_action";
            public const string LampActionInfo = "lamp_action_info";
            public const int LampActionIconIndex = 17;
            public const string LampSkill1 = "lamp_skill1";
            public const string LampSkill1Info = "lamp_skill1_info";
            public const int LampSkill1IconIndex = 18;
            public const string LampSkill2 = "lamp_skill2";
            public const string LampSkill2Info = "lamp_skill2_info";
            public const int LampSkill2IconIndex = 19;
        }

        public static class Hero
        {
            public const float SearchCycle = 0.1f;
            public const int HeroReward = 2;
            public const float DieDuration = 1f;
            public const float BossConcentrateTime = 0.5f;
            public const int BossReward = 10;
        }

        public static class Delay
        {
            public static WaitForSeconds Spawn = new WaitForSeconds(0.75f);
        }

        public static class Padding
        {
            public const float PaddingSmall = 10f;
            public const float PaddingNormal = 25f;
            public const float PaddingLarge = 40f;
        }

        public static class Starcatch
        {
            #region DOTween

            public const float F_RewardFadeDuration = 0.5f;
            public const float ReleaseAppendFadeDuration = 0.7f;
            public const float Soul2JoinMoveY = -20f;
            public const float Soul1AppendMoveY = -15f;
            public const float Soul2JoinMoveY2 = 20f;
            public const float Soul1PrependMoveY = 15f;
            public const float StarAppendMoveX = -450f;
            public const float StarPrependMoveX = 450f;
            public const float OpenJoinFadeDuration = 0.1f;
            public const float OpenAppendScaleDuration = 0.1f;
            public static Vector3 S_RewardAppendLocalMove = new Vector3(0, -120f, 0);
            public const float S_RewardJoinScaleDuration = 0.3f;
            public const float S_RewardAppendMoveDuration = 0.3f;
            public const float S_RewardJoinAppendDelay = 0.2f;
            public const float S_RewardJoinFadeDuration = 0.2f;
            public const float S_RewardPrependScaleDuration = 0.2f;
            public const float FailLampAppendFadeDuration = 0.3f;
            public const int LampShakeVivrato = 20;
            public const float LampShakeStrength = 3f;
            public const float SuccessLampPrependDelay = 0.7f;

            #endregion

            public const int ChanceEnd = 10;
            public const int Chance = 2;

            public const float EasyStarSpeed = 0.8f;
            public const int EasySuccessReward = 5;
            public const int EasyFailReward = 0;

            public const float NormalStarSpeed = 0.6f;
            public const int NormalSuccessReward = 6;
            public const int NormalFailReward = 0;

            public const float HardStarSpeed = 0.4f;
            public const int HardSuccessReward = 8;
            public const int HardFailReward = 0;

            public const float SkillStarSpeed = 0.4f;
            public const int SkillSuccessPlusReward = 20;
            public const int SkillSuccessReward = 8;
            public const int SkillFailReward = 0;

            public const float ClosePopupTime = 1f;
            public static Color NormalStarcatchColor = new Color(222 / 255f, 134 / 255f, 248 / 255f);
            public static Color HardStarcatchColor = new Color(252 / 255f, 253 / 255f, 118 / 255f);
            public static Color SkillStarcatchColor = new Color(139 / 255f, 245 / 255f, 108 / 255f);
            public const int AutoMaxReward = 4;
        }

        public static class Setting
        {
            #region DOTween

            public const float OpenAppendScaleDuration = 0.1f;
            public const float OpenJoinFadeDuration = 0.1f;

            #endregion

            #region UIText

            public const string LOBBY_START = "Lobby_GameStart";
            public const string LOBBY_SETTING = "Lobby_Setting";
            public const string LOBBY_QUIT = "Lobby_Quit";
            public const string LOBBY_TUTORIAL = "Lobby_Tutorial";
            public const string LOBBY_MAINGAME = "Lobby_MainGame";

            public const string SETTING_MASTER_VOLUME_TEXT = "Setting_MasterVolumeText";
            public const string SETTTING_EFFECT_VOLUME_TEXT = "Setting_EffectVolumeText";
            public const string SETTING_BGM_VOLUME_TEXT = "Setting_BgmVolumeText";
            public const string SETTING_LANGUAGE_TEXT = "Setting_LanguageText";
            public const string SETTING_MINIMAP_TEXT = "Setting_MinimapText";
            public const string SETTING_LOBBY_BUTTON_TEXT = "Setting_LobbyButtonText";
            public const string SETTING_EXIT_BUTTON_TEXT = "Setting_ExitButtonText";
            public const string SETTING_HELP_TEXT = "Setting_HelpText";
            public const string SETTING_LICENSE_TEXT = "Setting_LicenseText";
            public const string SETTING_HELP_MOVE = "Setting_HelpMoveText";
            public const string SETTING_HELP_POSSESS = "Setting_HelpPossessText";
            public const string SETTING_HELP_EQUIP = "Setting_HelpEquipText";
            public const string SETTING_HELP_INTERACT = "Setting_HelpInteractText";
            public const string SETTING_HELP_ACTION = "Setting_HelpActionText";
            public const string SETTING_HELP_STATUS = "Setting_HelpStatusText";
            public const string SETTING_HELP_MINIMAP = "Setting_HelpMinimapText";
            public const string SETTING_HELP_SETTING = "Setting_HelpSettingText";
            public const string SETTING_HELP_BACK = "Setting_HelpBackText";

            #endregion

            public const int NormalMinimapCamSize = 15;
            public const int ExpandMinimapCamSize = 35;
            public const int NormalMinimapUISize = 360;
            public const int ExpandMinimapUISize = 950;

            public const string GameClear = "GAME CLEAR !";
            public const string GameOver = "GAME OVER ...";
        }

        public static class Coffin
        {
            #region DOTween

            public const float ItemHoverPrependMoveY = 40f;
            public const float ItemHoverPrependMoveDuration = 0.3f;
            public const float ClickPanelAppendScaleDuration = 0.1f;
            public const float PurchaseFailAppendShakeDuration = 0.4f;
            public const float PurchaseFailAppendShakeStrength = 6f;
            public const int PurchaseFailAppendShakeVibrato = 30;

            #endregion

            #region UIText

            public const string COFFIN_CHECK_PURCHASE = "Coffin_Check_Purchase";
            public const string COFFIN_WARNING_SOUL = "Coffin_Warning_Soul";
            public const string COFFIN_WARNING_SKULL = "Coffin_Warning_Skull";
            public const string COFFIN_PURCHASE = "Coffin_Purchase";
            public const string COFFIN_CANCEL = "Coffin_Cancel";

            #endregion
        }

        public static class Spellbook
        {
            public const string Root = "ROOT";
            public const string CheckImage = "CheckImage";

            public const string IncreaseSkullEnergyRecoverySpeed = "IncreaseSkullEnergyRecoverySpeed";
            public const string IncreaseMaxSkullCount = "IncreaseMaxSkullCount";
            public const string IncreaseSkullDamage = "IncreaseSkullDamage";
            public const string IncreaseSkullMaxHp = "IncreaseSkullMaxHp";
            public const string IncreaseSkullMoveSpeed = "IncreaseSkullMoveSpeed";
            public const string IncreaseGhostMoveSpeed = "IncreaseGhostMoveSpeed";
            public const string ReduceGravestoneRespawnTime = "ReduceGravestoneRespawnTime";
        }

        public static class MasteryAnimation
        {
            #region DOTween

            public const float AlertFailShakeDuration = 0.3f;
            public const float AlertFailShakeStrength = 5f;
            public const int AlertFailShakeVivrato = 30;
            public const float OpenAppendScaleDuration = 0.1f;
            public const float OpenJoinFadeDuration = 0.1f;
            public const float MasteryShakeDuration = 1f;
            public const float MasteryShakeStrength = 5f;
            public const int MasteryVivrato = 20;
            public const float MasteryFadeDuration = 0.1f;
            public const float MasteryImageOffDelay = 1.1f;
            public const float MasteryTextFadeDuration = 0.3f;
            public const float MasteryColorDuration = 2f;

            #endregion
        }

        public static class Offset
        {
            public const int SkillIconStartOffset = 8;
            public const int SkillKeyMapOffset = 38;
            public const int StatIconOffset = 8;
            public const int StatImageMultiOffset = 3;
        }

        public static class Tutorial
        {
            public const string GuideString = "GuideObject";
            public const string InteractiveString = "InteractiveObject";

            #region Prefabs

            public const string Scarecrow = "Prefabs/Human/Scarecrow/Scarecrow";
            public const string TutorialManager = "Prefabs/Manager/TutorialManager";
            public const string TutorialImage = "Tutorial";
            public const string SkipButton = "Tutorial/SkipButton";

            public static readonly string[] GuidObjects = new string[]
            {
                "Tutorial/ArrowObject", "Tutorial/ClearImage", "Tutorial/EImage", "Tutorial/QImage",
                "Tutorial/TapImage", "Tutorial/ClickImage", "Tutorial/SpaceImage", "Tutorial/PrefabImage"
            };

            public static readonly string[] InteractiveObjects = new string[]
            {
                "Altar", "Coffin", "SpellBook", "Sword", "Lamp", "Gravestone"
            };

            #endregion

            #region DOTween

            public const float ImageMoveDuration = 0.5f;
            public const float ImageFadeDuration = 0.75f;
            public const float TextTypingDuration = 0.05f;

            #endregion

            #region TutrialText

            public static readonly string[] Storys = new string[]
            {
                "Story_0", "Story_1", "Story_2", "Story_3", "Story_4", "Story_5", "Story_6", "Story_7", "Story_8",
                "Story_9", "Story_10", "Story_11", "Story_12", "Story_13", "Story_14", "Story_15", "Story_16",
                "Story_17", "Story_18"
            };

            public static readonly string[] Explains = new string[]
            {
                "Explain_0", "Explain_1", "Explain_2", "Explain_3", "Explain_4", "Explain_5", "Explain_6",
                "Explain_7", "Explain_8", "Explain_9", "Explain_10", "Explain_11", "Explain_12"
            };

            public static readonly string[] Headline = new string[]
            {
                "Headline_0", "Headline_1", "Headline_2", "Headline_3", "Headline_4", "Headline_5", "Headline_6",
                "Headline_7", "Headline_8", "Headline_9", "Headline_10", "Headline_11", "Headline_12"
            };

            #endregion
        }

        public static class StringRes
        {
            //==Stat==//
            public const string StrengthId = "stat_str";
            public const string DexterityId = "stat_dex";
            public const string IntelligenceId = "stat_int";
            public const string MaxHpId = "stat_max_hp";
            public const string MoveSpeedId = "stat_move_speed";
            public const string CriticalId = "stat_critical_rate";
            public const string ActionDelayId = "stat_action_delay";

            //==Mastery Effect==//
            public const string EffectLifeStealId = "effect_life_steal";
            public const string EffectRageId = "effect_rage";
            public const string EffectChainLightningId = "effect_chain_lightning";
            public const string EffectSunFireAuraId = "effect_sunfire_aura";
            public const string EffectFrozenAuraId = "effect_frozen_aura";
            public const string EffectCurseAuraId = "effect_curse_aura";

            //==Alert ID ==//
            public const string LobbyMessageId = "lobby_message";
            public const string OkId = "ok";
            public const string CancelId = "cancel";

            //==Result==//
            public const string ResultDay = "result_day";
            public const string ResultPlaytime = "result_playtime";
            public const string ResultSoul = "result_soul";
            public const string ResultHuman = "result_human";
            public const string ResultBack = "result_back";

            //==Intro==//
            public const string Intro_1 = "intro_1";
            public const string Intro_2 = "intro_2";
            public const string Intro_3 = "intro_3";
            public const string Intro_4 = "intro_4";
            public const string Intro_5 = "intro_5";
            public const string IntroDialogue = "intro_dialogue";

            //==Clear==//
            public const string Clear_1 = "clear_1";
            public const string Clear_2 = "clear_2";

            //==Over==//
            public const string Over_1 = "over_1";
            public const string Over_2 = "over_2";

            //==Tutorial==//
            public const string AskText = "ask_text";
            public const string ToLobbyText = "to_lobby_text";
        }

        public static class DOTween
        {
            public const float OpenPopupDuration = 0.1f;
        }

        public static class AnalyticsData
        {
            public static class GameEndEvent
            {
                public const string EventName = "GameData";

                private const string KeyCollectedSoul = "collectedSoul";
                private const string KeyAliveDay = "day";
                private const string KeyCurSkullCount = "curSkullCount";
                private const string KeyMaxSkullCount = "maxSkullCount";
                private const string KeyPlayTime = "playTime";
                private const string KeyIsClear = "isGameClear";

                public static Dictionary<string, object> ToParams(GameStateData data)
                {
                    return new Dictionary<string, object>
                    {
                        [KeyCollectedSoul] = data.CollectedSoul,
                        [KeyAliveDay] = data.AliveDay,
                        [KeyCurSkullCount] = data.CurSkullCount,
                        [KeyMaxSkullCount] = data.MaxSkullCount,
                        [KeyPlayTime] = data.PlayTime,
                        [KeyIsClear] = data.IsGameClear,
                    };
                }
            }

            public static class DayEvent
            {
                public const string EventName = "dayData";

                private const string KeyCollectedSoulPerDay = "collectedSoulPerDay";
                private const string KeyCurrentDay = "currentDay";

                public static Dictionary<string, object> ToParams(DayStateData data)
                {
                    return new Dictionary<string, object>
                    {
                        [KeyCollectedSoulPerDay] = data.EarnedSoulPerDay, [KeyCurrentDay] = data.CurrentDay,
                    };
                }
            }
        }

        public static class Tip
        {
            public static readonly string[] Tips = new string[]
            {
                "Tip_0", "Tip_1", "Tip_2", "Tip_3", "Tip_4", "Tip_5", "Tip_6", "Tip_7", "Tip_8", "Tip_9", "Tip_10",
                "Tip_11", "Tip_12"
            };
        }
    }
}