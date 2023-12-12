using Data;
using Data.Remote.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Util;

public static class DownloadJsonApi
{
    #region RequestBody

    private const string VersionPath = "1eg8DbKRZ_aGCLwL1fncxT9o59w0sM2XRNofWYcbWYEA";
    private const string MessageOnFailed = "Networking Error :: Please Restart";

    private static VersionResponseBody _version;

    private static SheetToJsonRequest[] _requests;

    public static readonly string DownloadPath =
        $"{Application.persistentDataPath}{Path.DirectorySeparatorChar}";

    public static event Action<string> ErrorEvent;
    public static event Action FailEvent;

    #endregion

    private static async Task<string> GetData(SheetToJsonRequest request)
    {
#if DEBUG
        Uri uri = request.ToUri(true);
#else
        Uri uri = request.ToUri(false);
#endif
        UnityWebRequest req = await SendWebRequest(uri, 100, "Reloading...");

        return req.result == UnityWebRequest.Result.Success ? req.downloadHandler.text : string.Empty;
    }

    private static T ConvertValue<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return default(T);

        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        return (T)converter.ConvertFromString(value);
    }

    private static List<T> ConvertList<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return new List<T>();

        return value.Split('&').Select(x => ConvertValue<T>(x)).ToList();
    }

    private static List<T> ParseTsvDataSet<T>(string data)
    {
        string replaced = data.Replace("\r", "");
        string[] lines = replaced.Split("\n");
        return lines.Select(ParseTsvData<T>).ToList();
    }

    private static T ParseTsvData<T>(string line)
    {
        Type type = typeof(T);
        string[] cols = line.Split("\t");
        ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
        if (constructorInfo == null)
        {
            Debug.LogAssertion("Result type must have default constructor.");
            throw new ArgumentException("Result type must have default constructor.");
        }

        T result = (T)constructorInfo.Invoke(Array.Empty<object>());
        FieldInfo[] fieldInfoArr = type.GetFields();
        for (int i = 0; i < fieldInfoArr.Length; i++)
        {
            FieldInfo fieldInfo = fieldInfoArr[i];
            object value = ConvertValueByType(fieldInfo.FieldType, cols[i]);
            fieldInfo.SetValue(result, value);
        }

        return result;
    }

    private static object ConvertValueByType(Type type, string column)
    {
        TypeConverter converter = TypeDescriptor.GetConverter(type);
        return converter.ConvertFromString(column);
    }

    private static T StringToEnum<T>(string e)
    {
        return (T)Enum.Parse(typeof(T), e);
    }

    public static async Task<string> GetLatestVersion(int milliSeconds = 1000)
    {
        Uri uri = new SheetToJsonRequest
        {
            Path = VersionPath,
            Range = "A2:D2",
            TestSheetId = "0",
            File = EFileNames.None,
            TestSheetRange = "A17:D17"
        }.ToUri(Debug.isDebugBuild);
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        UnityWebRequestAsyncOperation operation = www.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        Debug.Log($"code : {www.responseCode}");
        if (www.result != UnityWebRequest.Result.Success)
        {
            if (milliSeconds >= Constants.Time.TimeOutMilliseconds)
            {
                return null;
            }

            Debug.LogWarning($"Failed to connect- {www.error}");
            ErrorEvent?.Invoke("Reloading...");
            Debug.Log("wait...");
            await Task.Delay(milliSeconds);
            return await GetLatestVersion(milliSeconds * 2);
        }

        string data = www.downloadHandler.text;
        _version = ParseTsvData<VersionResponseBody>(data);

        if (string.CompareOrdinal(Application.version, _version.BuildVersion) >= 0)
        {
            return _version.Version;
        }

        Debug.LogWarning("Got version is not compatible with current build version.");
        ErrorEvent?.Invoke("Application needs update");

        return _version.Version;
    }

    public static async Task GetRequestsForDownload()
    {
        if (_version == null)
        {
            Debug.LogError("Cannot read request from version");
            return;
        }

        Uri uri = new SheetToJsonRequest
            {
                Path = VersionPath, Range = _version.VersionDataRange, File = EFileNames.None,
            }
            .ToUri(isTest: Debug.isDebugBuild);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        UnityWebRequestAsyncOperation operation = www.SendWebRequest();
        while (!operation.isDone) { await Task.Yield(); }

        if (www.result != UnityWebRequest.Result.Success)
        {
            //todo Capsule logging to error using delegate 
            Debug.LogError("Failed to load  data");
            ErrorEvent?.Invoke("Failed to load - Reloading...");
        }

        string result = www.downloadHandler.text;
        Debug.Log($"result {result}");
        List<VersionDataResponseBody> requests = ParseTsvDataSet<VersionDataResponseBody>(result);
        _requests = requests.Select(body => new SheetToJsonRequest()
        {
            File = body.Data,
            Path = body.Path,
            Range = body.Range,
            TestSheetId = body.TestSheetId,
            TestSheetRange = body.TestRange
        }).ToArray();
    }

    public static async Task DownloadDataSet(Action<int> onProgressChanged = null)
    {
        onProgressChanged?.Invoke(0);
        int completed = 0;
        int totalTask = _requests.Length * 2;

        Task<string>[] tasks = _requests.Select(GetData).ToArray();
        Debug.Log("request to task");
        Debug.Log($"{tasks.Length}");
        for (int i = 0; i < tasks.Length; i++)
        {
            string tsvData = await tasks[i];
            Debug.Log($"data :{tsvData}");
            completed++;
            onProgressChanged?.Invoke((completed * 100) / totalTask);
            EFileNames eFileNames = _requests[i].File;

            switch (eFileNames)
            {
                case EFileNames.I18nData:
                    DownloadJson<string, I18nData>(tsvData, eFileNames, data => data.id);
                    break;
                case EFileNames.UnitStatData:
                    DownloadJson<string, UnitStatData>(tsvData, eFileNames, data => data.name);
                    break;
                case EFileNames.MasteryData:
                    DownloadJson<string, MasteryDataEntry>(tsvData, eFileNames, data => data.Id);
                    break;
                case EFileNames.SoundData:
                    DownloadJson<string, SoundData>(tsvData, eFileNames, data => data.soundName);
                    break;
                case EFileNames.WaveData:
                    DownloadJson<int, WaveResponseBody>(
                        tsvData,
                        eFileNames,
                        data => int.Parse(data.wave.Split(" ")[1]));
                    break;
                case EFileNames.SpellBookData:
                    DownloadJson<string, SpellResponseBody>(tsvData, eFileNames, data => data.id);
                    break;
                case EFileNames.CoffinData:
                    DownloadJson<string, CoffinData>(tsvData, eFileNames, data => data.name);
                    break;
                case EFileNames.OnHitData:
                    DownloadJson<string, OnHit>(tsvData, eFileNames, data => data.Type.ToString());
                    break;
                case EFileNames.StatPriceData:
                    DownloadJson<int, StatPriceResponseBody>(tsvData, eFileNames, data => data.level);
                    break;
                case EFileNames.AuraData:
                    DownloadJson<string, AuraResponseBody>(tsvData, eFileNames, data => data.Id.ToString());
                    break;
                case EFileNames.SettingData:
                    DownloadJson<string, SettingResponseBody>(tsvData, eFileNames, data => data.Property);
                    break;
                case EFileNames.None:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            completed++;
            onProgressChanged?.Invoke(completed * 100 / totalTask);
        }
    }

    private static void DownloadJson<TK, TV>(string data, EFileNames file, Func<TV, TK> keyFunction)
    {
        List<TV> list = ParseTsvDataSet<TV>(data);
        JsonDataList<TK, TV> result = new()
        {
            data = list
                .Select(value => new DataComponent<TK, TV> { key = keyFunction(value), value = value })
                .ToList()
        };
        string downloadPath = $"{DownloadPath}{file}.json";
        string jsonString = JsonUtility.ToJson(result, true);
        Debug.Log($"json is :{jsonString}");
        File.WriteAllText(downloadPath, jsonString, Encoding.UTF8);
        Debug.Log("download complete");
    }

    private static async Task<UnityWebRequest> SendWebRequest(Uri uri, int reloadDelayTime, string reloadMessage)
    {
        UnityWebRequest req = UnityWebRequest.Get(uri);
        UnityWebRequestAsyncOperation response = req.SendWebRequest();
        while (!response.isDone) { await Task.Yield(); }

        if (req.result != UnityWebRequest.Result.Success)
        {
            if (reloadDelayTime >= Constants.Time.TimeOutMilliseconds)
            {
                FailEvent?.Invoke();
            }

            ErrorEvent?.Invoke(reloadMessage);

            await Task.Delay(reloadDelayTime);
            return await SendWebRequest(uri, reloadDelayTime * 2, reloadMessage);
        }

        return req;
    }
}