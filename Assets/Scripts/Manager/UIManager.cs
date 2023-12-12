using Data;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager
{
    //int _order = 20;
    private DataManager _dataManager;
    private Language _currentLanguage;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    Stack<string> _popupNameStack = new Stack<string>();

    public UI_Scene SceneUI { get; private set; }

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };

            return root;
        }
    }

    public UIManager()
    {
        Managers.GameManager.OnAppSettingChanged += ChangeLanguage;
        SceneManager.sceneLoaded += (_, _) => { CloseAllPopupUI(); };
        _currentLanguage = PreferencesManager.GetLanguage();
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.sortingOrder = go.GetComponent<Canvas>().sortingOrder;       
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        /*if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }*/
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        go.transform.localScale = Vector3.one;

        return Utils.GetOrAddComponent<T>(go);
    }

    public T ShowSceneUI<T>() where T : UI_Scene
    {
        string name = typeof(T).Name;
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Utils.GetOrAddComponent<T>(go);
        SceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null, Transform parent = null) where T : UI_Popup
    {
        Time.timeScale = 0f;
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        foreach (UI_Popup p in _popupStack)
        {
            if (p.name == name)
            {
                return p as T;
            }
        }

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Utils.GetOrAddComponent<T>(go);

        _popupStack.Push(popup);

        if (parent != null)
            go.transform.SetParent(parent);
        //else if (SceneUI != null)
        //    go.transform.SetParent(SceneUI.transform);
        else
            go.transform.SetParent(Root.transform);

        go.transform.localScale = Vector3.one;

        return popup;
    }

    public T FindPopup<T>() where T : UI_Popup
    {
        return _popupStack.Where(x => x.GetType() == typeof(T)).FirstOrDefault() as T;
    }

    public T PeekPopupUI<T>() where T : UI_Popup
    {
        if (_popupStack.Count == 0)
            return null;

        return _popupStack.Peek() as T;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
        {
            return;
        }

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
        {
            return;
        }

        UI_Popup popup = _popupStack.Pop();
        if (popup != null)
        {
            Managers.Resource.Destroy(popup.gameObject);
        }

        //_order--;

        if (_popupStack.Count > 0)
            return;
        Time.timeScale = 1.0f;
    }

    public void HidePopupUI(UI_Popup popup)
    {
        popup.gameObject.SetActive(false);
    }

    public bool IsPopupUI()
    {
        return _popupStack.Count > 0;
    }

    public bool CloseAllPopupUI()
    {
        bool ret = IsPopupUI();

        while (_popupStack.Count > 0)
        {
            ClosePopupUI();
        }

        return ret;
    }

    public void Clear()
    {
        CloseAllPopupUI();
        SceneUI = null;
    }

    public string GetString(string resId)
    {
        if (_dataManager == null)
        {
            _dataManager = Managers.Data;
        }

        if (!_dataManager.I18nDic.ContainsKey(resId))
        {
            return string.Empty;
        }

        I18nData stringRes = _dataManager.I18nDic[resId];
        return (_currentLanguage) switch
        {
            Language.KOREAN => stringRes.kr,
            Language.ENGLISH => stringRes.en,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void ChangeLanguage(AppSetting setting)
    {
        _currentLanguage = setting.Language;
    }
}