#if UNITY_EDITOR
using Manager;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class DataTransformerEditor : EditorWindow
{
    [MenuItem("Tools/DeletePlayerPrefs ")]
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Complete DeletePlayerPrefs");
    }

    [MenuItem("Tools/ParseExcel %#K")]
    public static async void ParseExcel()
    {
        await DownloadJsonApi.GetLatestVersion();
        await DownloadJsonApi.GetRequestsForDownload();
        await DownloadJsonApi.DownloadDataSet();
        Debug.Log("Complete DataTransformer");
    }
}
#endif