using Manager;
using UnityEditor;
using UnityEngine;
using Util;

public class UI_Loading : UI_Scene
{
    #region Enums

    enum Texts
    {
        LoadingText,
        TipText,
    }
    
    enum Images
    {
        LoadingBar,
        LoadingGauge,
    }
    #endregion


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetImage((int)Images.LoadingBar).gameObject.SetActive(false);

        DownloadJsonApi.FailEvent += ShowFailMessage;
        DownloadJsonApi.ErrorEvent += ShowErrorMessage;

        VersionManager.Instance.DownloadCompleted += ShowTipMessage;
        VersionManager.Instance.VersionCheckCompleteEvent += LoadNextScene;
        VersionManager.Instance.LoadLatestVersion(
            onNeedUpdate: UpdateVersionUI,
            onUpdateProgressChanged: UpdateProgressUI
        );

        return true;
    }

    private void OnDestroy()
    {
        DownloadJsonApi.FailEvent -= ShowFailMessage;
        DownloadJsonApi.ErrorEvent -= ShowErrorMessage;
    }

    //todo show ui toast 
    private void ShowErrorMessage(string message)
    {
        Debug.Log($"error:{message}");
        UI_ToastMessage toast = Managers.UI.MakeSubItem<UI_ToastMessage>();
        toast.SetMessage(message, Color.black);
    }

    private void ShowFailMessage()
    {
        GetText((int)Texts.LoadingText).text = "Networking Error";
    }

    private void ShowTipMessage()
    {
        Managers.Data.Init();
        GetText((int)Texts.TipText).text = GetString(Constants.Tip.Tips[Random.Range(0, Constants.Tip.Tips.Length)]);
    }


    private void LoadNextScene()
    {
        Managers.Scene.ChangeScene(Define.Scene.IntroScene);
    }

    private void UpdateProgressUI(int progress)
    {
        Debug.Log($"progressChanged : {progress}");
        GetImage((int)Images.LoadingGauge).fillAmount = (float)(progress / 100f);
    }

    private void UpdateVersionUI(bool needUpdate)
    {
        if (needUpdate)
        {
            GetText((int)Texts.LoadingText).text = "Loading...";
            GetImage((int)Images.LoadingBar).gameObject.SetActive(true);
        }
        else
        {
            GetText((int)Texts.LoadingText).text = "Loading Complete";
        }
    }
}