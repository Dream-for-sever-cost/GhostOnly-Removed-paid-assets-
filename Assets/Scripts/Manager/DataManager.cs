using Data;
using System.Collections.Generic;
using UnityEngine;
using AES;
using Data.Remote.Response;
using System;
using System.IO;
using System.Threading;

public enum EFileNames
{
    None,
    I18nData,
    UnitStatData,
    MasteryData,
    SoundData,
    WaveData,
    SpellBookData,
    CoffinData,
    OnHitData,
    StatPriceData,
    AuraData,
    SettingData,
};

public class DataManager
{
    public bool IsLoaded { get; private set; } = false;

    public Dictionary<string, I18nData> I18nDic { get; private set; } = new Dictionary<string, I18nData>();
    public Dictionary<string, UnitStatData> UnitDic { get; private set; } = new Dictionary<string, UnitStatData>();

    public Dictionary<string, Mastery> MasteryEntries { get; } = new Dictionary<string, Mastery>();
    public Dictionary<string, OnHit> OnHitEntries { get; private set; } = new Dictionary<string, OnHit>();

    public Dictionary<string, SoundData> SoundDic { get; private set; } = new Dictionary<string, SoundData>();
    public Dictionary<int, JsonWaveData> WaveDataDic { get; private set; } = new Dictionary<int, JsonWaveData>();
    public Dictionary<string, SpellData> SpellDataDic { get; private set; } = new Dictionary<string, SpellData>();

    public Dictionary<string, CoffinData> CoffinDataDic { get; private set; } = new Dictionary<string, CoffinData>();

    public Dictionary<int, StatPriceResponseBody> StatPriceDic { get; private set; } =
        new Dictionary<int, StatPriceResponseBody>();

    public Dictionary<string, SettingResponseBody> SystemValueDic { get; private set; }

    public event Action LoadAllDataSetEvent;

    public void Init()
    {
        if (IsLoaded) return;

        I18nDic = LoadJson<string, I18nData>($"{EFileNames.I18nData.ToString()}");
        LoadStringResources();
        UnitDic = LoadJson<string, UnitStatData>($"{EFileNames.UnitStatData.ToString()}");

        Dictionary<string, MasteryDataEntry> masteryEntries =
            LoadJson<string, MasteryDataEntry>($"{EFileNames.MasteryData.ToString()}");
        foreach ((string id, MasteryDataEntry entry) in masteryEntries)
        {
            MasteryEntries.Add(id, entry.ToMasteryDto());
        }

        Dictionary<int, WaveResponseBody> waves = LoadJson<int, WaveResponseBody>($"{EFileNames.WaveData.ToString()}");
        foreach ((int key, WaveResponseBody value) in waves)
        {
            WaveDataDic.Add(key, value.ToDto());
        }

        InitSpellbook();

        StatPriceDic = LoadJson<int, StatPriceResponseBody>(EFileNames.StatPriceData.ToString());
        OnHitEntries = LoadJson<string, OnHit>(EFileNames.OnHitData.ToString());
        SoundDic = LoadJson<string, SoundData>($"{EFileNames.SoundData.ToString()}");
        CoffinDataDic = LoadJson<string, CoffinData>($"{EFileNames.CoffinData.ToString()}");
        SystemValueDic = LoadJson<string, SettingResponseBody>(EFileNames.SettingData.ToString());
        IsLoaded = true;
        LoadAllDataSetEvent?.Invoke();
    }

    private void LoadStringResources()
    {
        //todo refactoring this method 
        TextAsset stringAsset = Resources.Load<TextAsset>("Data/JsonData/string");
        JsonDataList<string, I18nData>
            jsonData = JsonUtility.FromJson<JsonDataList<string, I18nData>>(stringAsset.text);
        foreach (DataComponent<string, I18nData> dataComponent in jsonData.data)
        {
            I18nDic.Add(dataComponent.key, dataComponent.value);
        }
    }

    public void InitSpellbook()
    {
        SpellDataDic.Clear();
        Dictionary<string, SpellResponseBody> spellDataSet =
            LoadJson<string, SpellResponseBody>($"{EFileNames.SpellBookData.ToString()}");
        foreach ((string key, SpellResponseBody value) in spellDataSet)
        {
            SpellDataDic.Add(key, value.ToDto());
        }
    }

    //todo open or create directory
    private Dictionary<TKey, TValue> LoadJson<TKey, TValue>(string path)
    {
        try
        {
            Debug.Log("startLoadJson");
            string folderPath = DownloadJsonApi.DownloadPath;
            string filePath = $"{folderPath}{path}.json";

            Debug.Log($"filePath :{filePath}");
            FileStream fs = File.Open(filePath, FileMode.Open);
            StreamReader reader = new StreamReader(fs);
            using (fs)
            {
                string jsonString = reader.ReadToEnd();
                Debug.Log($"loadJson : {jsonString}");
                JsonDataList<TKey, TValue> result = JsonUtility.FromJson<JsonDataList<TKey, TValue>>(jsonString);
                Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
                foreach (DataComponent<TKey, TValue> data in result.data)
                {
                    dictionary.Add(data.key, data.value);
                }

                return dictionary;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured.");
            Debug.LogException(e);
        }


        return new Dictionary<TKey, TValue>();
    }
}