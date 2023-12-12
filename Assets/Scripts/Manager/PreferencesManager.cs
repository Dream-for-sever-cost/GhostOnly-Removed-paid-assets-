using System;
using UnityEngine;
using Data;

namespace Manager
{ 
    public static class PreferencesManager
    {
        //todo optimization to use only 1 bit for bool value 
        //1비트만 써도 불값은 저장이 가능하다 따라서 최적화를 위해서 플래그들로 만들고
        //1 << n 으로 지정 한 후 각 비트에 맞는 연산으로 불값을 정수 하나로 32개가 가능해짐  
        //근데 메모리 많으니 그냥 플렉스 해버림 ㅋ

        private static Action _onAppSettingChanged;

        public static void SetOnAppSettingChangeListener(Action action)
        {
            _onAppSettingChanged = action;
        }

        private static class Keys
        {
            public const string MasterVolumeKey = "master_volume";
            public const string MasterVolumeSwitchKey = "master_volume_switch";
            public const string EffectVolumeKey = "effect_volume";
            public const string EffectVolumeSwitchKey = "effect_volume_switch";
            public const string BgmVolumeKey = "bgm_volume";
            public const string BgmVolumeSwitchKey = "bgm_volume_switch";
            public const string MinimapIsLeftKey = "minimap_position";
            public const string LanguageKey = "language";
            public const string VersionKey = "version";
        }

        public static void SetMasterVolume(float volume)
        {
            PlayerPrefs.SetFloat(Keys.MasterVolumeKey, volume);
            Save();
        }

        public static float GetMasterVolume()
        {
            return PlayerPrefs.GetFloat(Keys.MasterVolumeKey, 1f);
        }

        public static void SetMasterVolumeSwitch(bool isOn)
        {
            int saved = isOn ? 1 : 0;
            PlayerPrefs.SetInt(Keys.MasterVolumeSwitchKey, saved);
            Save();
        }

        public static bool IsMasterVolumeOn()
        {
            int saved = PlayerPrefs.GetInt(Keys.MasterVolumeSwitchKey, 1);
            return saved == 1;
        }

        public static float GetEffectVolume()
        {
            return PlayerPrefs.GetFloat(Keys.EffectVolumeKey, 1f);
        }

        public static void SetEffectVolume(float effectVolume)
        {
            PlayerPrefs.SetFloat(Keys.EffectVolumeKey, effectVolume);
            Save();
        }

        public static bool IsEffectVolumeOn()
        {
            int saved = PlayerPrefs.GetInt(Keys.EffectVolumeSwitchKey, 1);
            return saved == 1;
        }

        public static void SetEffectVolumeSwitch(bool isOn)
        {
            int saved = isOn ? 1 : 0;
            PlayerPrefs.SetInt(Keys.EffectVolumeSwitchKey, saved);
            Save();
        }

        public static float GetBgmVolume()
        {
            return PlayerPrefs.GetFloat(Keys.BgmVolumeKey, 1f);
        }

        public static void SetBgmVolume(float bgmVolume)
        {
            PlayerPrefs.SetFloat(Keys.BgmVolumeKey, bgmVolume);
            Save();
        }

        public static bool IsBgmVolumeOn()
        {
            return PlayerPrefs.GetInt(Keys.BgmVolumeSwitchKey, 1) == 1;
        }

        public static void SetBgmVolumeSwitch(bool isOn)
        {
            int saved = isOn ? 1 : 0;
            PlayerPrefs.SetInt(Keys.BgmVolumeSwitchKey, saved);
            Save();
        }

        public static bool IsMinimapLeft()
        {
            return PlayerPrefs.GetInt(Keys.MinimapIsLeftKey, 0) == 1;
        }

        public static void SetMinimapLeft(bool isMinimapLeft)
        {
            int isMinimapLeftInt = isMinimapLeft ? 1 : 0;
            PlayerPrefs.SetInt(Keys.MinimapIsLeftKey, isMinimapLeftInt);
            Save();
        }

        public static Language GetLanguage()
        {
            return (Language)PlayerPrefs.GetInt(Keys.LanguageKey, 0);
        }

        public static void SetLanguage(Language language)
        {
            PlayerPrefs.SetInt(Keys.LanguageKey, (int)language);
            Save();
        }

        private static void Save()
        {
            PlayerPrefs.Save();
            _onAppSettingChanged?.Invoke();
        }

        public static void SetVersion(string latestVersion)
        {
            PlayerPrefs.SetString(Keys.VersionKey,latestVersion);
            PlayerPrefs.Save();
        }
    }
}