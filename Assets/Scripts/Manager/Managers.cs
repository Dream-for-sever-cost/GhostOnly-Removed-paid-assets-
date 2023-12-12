using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    public static bool AnalyticsCollected = false;
    public static Managers s_instance = null;

    public static Managers Instance
    {
        get
        {
            Init();
            return s_instance;
        }
    }

    private static GameManager s_gameManager = new GameManager();
    private static ResourceManager s_resourceManager = new ResourceManager();
    private static UIManager s_uiManager;
    private static SceneManagerEx s_sceneManager = new SceneManagerEx();
    private static SoundManager s_soundManager = new GameObject(nameof(SoundManager)).AddComponent<SoundManager>();
    private static DataManager s_dataManager = new DataManager();
    private static MasteryManager s_masteryManager = new MasteryManager(s_dataManager);
    private static SoulManager s_soulManager = new SoulManager();
    private static DayManager s_dayManager = new DayManager();

    private static SlaveManager s_slaveManager =
        new GameObject(nameof(Manager.SlaveManager)).AddComponent<SlaveManager>();

    private static TargetManager s_targetManager = new TargetManager();
    private static SpellBookManager s_spellBookManager = new SpellBookManager();

    private static VersionManager s_versionManager =
        new GameObject(nameof(Manager.VersionManager)).AddComponent<VersionManager>();

    public static GameManager GameManager { get { return s_gameManager; } }

    public static ResourceManager Resource
    {
        get
        {
            Init();
            return s_resourceManager;
        }
    }

    public static MasteryManager Mastery { get { return s_masteryManager; } }

    public static UIManager UI
    {
        get
        {
            Init();
            return s_uiManager;
        }
    }

    public static SceneManagerEx Scene
    {
        get
        {
            Init();
            return s_sceneManager;
        }
    }

    public static SoundManager Sound
    {
        get
        {
            Init();
            return s_soundManager;
        }
    }

    public static DataManager Data
    {
        get
        {
            Init();
            return s_dataManager;
        }
    }

    public static SoulManager Soul { get { return s_soulManager; } }
    public static SlaveManager SlaveManager => s_slaveManager;
    public static TargetManager Target { get { return s_targetManager; } }
    public static SpellBookManager SpellBook { get { return s_spellBookManager; } }
    public static VersionManager VersionManager => VersionManager.Instance;
    public static DayManager DayManager { get { return s_dayManager; } }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AskForConsent();
        ConsentGiven();
        AnalyticsCollected = true;
        Init();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
#if DEBUG
            Debug.unityLogger.logEnabled = true;
#else
 Debug.unityLogger.logEnabled = false;
#endif
            SceneManager.sceneUnloaded += ChangeSceneData;
            s_uiManager = new UIManager();
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
                go = new GameObject { name = "@Managers" };
            DontDestroyOnLoad(go);
            s_instance = Utils.GetOrAddComponent<Managers>(go);
            s_resourceManager.Init();
            s_soundManager.Init();
            ObjectPoolManager instance = ObjectPoolManager.Instance;
            instance.Init();
        }
    }

    private static void ChangeSceneData(Scene scene)
    {
        Define.Scene sceneName = Enum.Parse<Define.Scene>(scene.name);
        switch (sceneName)
        {
            case Define.Scene.LobbyScene:
            case Define.Scene.TutorialScene:
            case Define.Scene.LoadingScene:
            case Define.Scene.GameScene:
                GameManager.ReadyForNextGame();
                break;
            case Define.Scene.LabTestScene:
                break;
            case Define.Scene.Unknown:
                break;
            case Define.Scene.IntroScene:
                break;
            case Define.Scene.GameClearScene:
                break;
            case Define.Scene.GameOverScene:
                break;
            case Define.Scene.CreditScene:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void AskForConsent()
    {
        // TODO ... show the player a UI element that asks for consent.
    }

    void ConsentGiven()
    {
        AnalyticsService.Instance.StartDataCollection();
    }
}