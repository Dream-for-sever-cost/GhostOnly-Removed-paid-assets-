using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Manager
{
    public class VersionManager : MonoBehaviour
    {
        private static VersionManager _instance;
        public static VersionManager Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = new GameObject().AddComponent<VersionManager>();
                return _instance;
            }
        }

        private string _latestVersion;
        public bool IsDone { get; private set; }

        private Action<bool> _getVersionCallback;
        private Action<int> _updateProgressChangedCallback;

        public event Action VersionCheckCompleteEvent;
        public event Action DownloadCompleted;

        public async void LoadLatestVersion(
            Action<bool> onNeedUpdate = null,
            Action<int> onUpdateProgressChanged = null)
        {
            _getVersionCallback = onNeedUpdate;
            _updateProgressChangedCallback = onUpdateProgressChanged;

            IsDone = false;
            _latestVersion = await DownloadJsonApi.GetLatestVersion();
            if (_latestVersion == null)
            {
                Debug.LogWarning("Failed To load data");
                return;
            }

            bool needUpdate = CheckPastVersion();
            _getVersionCallback?.Invoke(needUpdate);
            Debug.Log(_latestVersion);

            if (needUpdate)
            {
                Debug.Log($"current version is past version");
                await UpdateVersion(_updateProgressChangedCallback);
            }

            DownloadCompleted?.Invoke();
            
            //intentionally delay to show tip
            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSecondsRealtime(1.5f);
            VersionCheckCompleteEvent?.Invoke();
            IsDone = true;
        }

        private bool CheckPastVersion()
        {
            //todo getString using Preferences manager
            if (PlayerPrefs.GetString("version", "v.0").Equals(_latestVersion))
            {
                Debug.Log("This version is latestVersion");
                return false;
            }

            return true;
        }

        private async Task UpdateVersion(Action<int> onProgressChanged = null)
        {
            await DownloadJsonApi.GetRequestsForDownload();
            await DownloadJsonApi.DownloadDataSet(onProgressChanged);
            PreferencesManager.SetVersion( _latestVersion);
        }
    }
}