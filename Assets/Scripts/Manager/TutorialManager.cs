using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class TutorialManager : MonoBehaviour
{
    #region enum
    public enum TutoImages
    {
        None = -1,
        PanelImage = 0,
        PanelExampleImage = 1,
        PlayerImage = 2,
        SkullImage = 3,
        StoryBoxImage = 4,
        CoffinImage = 5,
        AltarImage = 6,
        SpellBookImage = 7,
        GraveStoneImage = 8,
        BackGroundImage = 9
    }
    public enum InteractiveObject
    {
        None = -1,
        Altar = 0,
        Coffin = 1,
        SpellBook = 2,
        Sword = 3,
        Lamp = 4,
        Gravestone = 5,
        Skull = 6,
        Scarecrow = 7
    }
    public enum GuideObject
    {
        None = -1,
        ArrowObject = 0,
        ClearImage = 1,
        EImage = 2, 
        QImage = 3,
        TapImage = 4,
        ClickImage = 5,
        SpaceImage = 6,
        PrefabImage = 7
    }
    public enum GuideImage
    {
        None = -1,
        Skull = 0,
        Scarecrow = 1,
        Coffin = 2,
        SpellBook = 3,
        Sword = 4,
        Lamp = 5,
    }
    #endregion

    #region class
    [System.Serializable]
    public class TutorialInfo
    {
        public int PanelNum;
        public int ActivePanelNum;
        public int NextTutorialNum;
    }
    [System.Serializable]
    public class TutoFade
    {
        public int PanelNum;
        public int CountNum;
        public List<TutoImages> TutoImages;
        public List<bool> IsFadeIn;
    }

    [System.Serializable]
    public class TutoActive
    {
        public int PanelNum;
        public int CountNum;
        public List<InteractiveObject> InteractiveObject;
        public List<GuideObject> GuideObject;
        public GuideImage GuideImage;
    }
    #endregion

    public static TutorialManager Instance;

    [field: SerializeField] public TutorialInfo[] TutorialInfos { get; private set; }
    [field: SerializeField] public TutoFade[] TutorialFade { get; private set; }
    [field: SerializeField] public TutoActive[] TutorialActive { get; private set; }
    [field: SerializeField] public List<Sprite> PrefapImages { get; private set; }
    [HideInInspector] public Rigidbody2D Player;
    [HideInInspector] public List<GameObject> InteractiveObjects;
    [HideInInspector] public List<GameObject> GuidObjects;
    [HideInInspector] public List<Transform> SpawnPoints;
    [HideInInspector] public Sprite[] TutorialImages;
    [HideInInspector] public Button SkipButton;
    [HideInInspector] public int PanelNum = 0;
    [HideInInspector] public int count = 0;
    [HideInInspector] public bool[] ContinueTutorialNums = new bool[14];

    private bool _isContinue = false;
    private float _timer = 0;

    private void Awake()
    {
        Instance = this;

        TutorialSetting();
    }

    void Update()
    {
        if (!GameObject.Find("UI_Tutorial"))
        {
            if (IsContinueTutorial(12) && !InteractiveObjects[4].transform.GetChild(0).gameObject.activeSelf)
            {
                GuidObjects[2].SetActive(false);
                GuidObjects[5].SetActive(true);
                GuidObjects[7].GetComponent<Image>().sprite = PrefapImages[6];
            }
            else if (IsContinueTutorial(13) && !InteractiveObjects[3].transform.GetChild(0).gameObject.activeSelf)
            {
                GuidObjects[2].SetActive(false);
                GuidObjects[5].SetActive(true);
                GuidObjects[7].GetComponent<Image>().sprite = PrefapImages[1];
            }

            #region Tutorial
            if (!ContinueTutorialNums[0])
            {
                ContinueTutorial(0);
            }
            else if (IsContinueTutorial(1, !_isContinue))
            {
                if (_timer < 2 && Player.velocity != Vector2.zero)
                {
                    _timer += Time.deltaTime;
                }
                if (_timer >= 2)
                {
                    _isContinue = true;
                }
            }
            else if (IsContinueTutorial(1, _isContinue))
            {
                ContinueTutorial(1);
            }
            else if (IsContinueTutorial(2))
            {
                ContinueTutorial(2);
            }
            else if (IsContinueTutorial(3))
            {
                ContinueTutorial(3);
            }
            else if (IsContinueTutorial(4))
            {
                ContinueTutorial(4);
            }
            else if (IsContinueTutorial(5, GameObject.Find("UI_Coffin")))
            {
                ContinueTutorial(5);
            }
            else if (IsContinueTutorial(6, !GameObject.Find("UI_Coffin")))
            {
                ContinueTutorial(6);
            }
            else if (IsContinueTutorial(7, !GameObject.Find("Skull(Clone)")))
            {
                ContinueTutorial(7);
            }
            else if (IsContinueTutorial(8, !InteractiveObjects[3].transform.GetChild(0).gameObject.activeSelf))
            {
                ContinueTutorial(8);
            }
            else if (IsContinueTutorial(9, GameObject.Find("UI_Mastery")))
            {
                ContinueTutorial(9);
            }
            else if (IsContinueTutorial(10, !GameObject.Find("UI_Mastery") && !_isContinue))
            {
                _isContinue = true;
                PanelNum = 10;
                Managers.UI.ShowPopupUI<UI_Tutorial>();
            }
            else if (IsContinueTutorial(10, GameObject.Find("UI_SpellBook")))
            {
                ContinueTutorial(10);
            }
            else if (IsContinueTutorial(11, !GameObject.Find("UI_SpellBook")))
            {
                ContinueTutorial(11);
            }
            else if (IsContinueTutorial(12, GameObject.Find("UI_StarCatch")))
            {
                _isContinue = true;
            }
            else if (IsContinueTutorial(12, !GameObject.Find("UI_StarCatch") && _isContinue))
            {
                ContinueTutorial(12);
            }
            else if (IsContinueTutorial(13, !GameObject.Find("Scarecrow(Clone)")))
            {
                ContinueTutorial(13);
            }
            #endregion
        }
    }
    private void OnDestroy()
    {
        Managers.GameManager.ReadyForNextGame();
    }

    private void TutorialSetting()
    {
        Transform UI_Game = GameObject.Find("UI_Game").transform;

        GameObject SkipObject = (GameObject)Instantiate(Resources.Load(Constants.Tutorial.SkipButton), UI_Game);

        SkipButton = SkipObject.GetComponent<Button>();

        SkipButton.onClick.AddListener(() => OnSkipButton());

        Player = GameObject.Find("Player(Clone)").GetComponent<Rigidbody2D>();
        for (int i = 0; i < Constants.Tutorial.GuidObjects.Length; i++)
        {
            GuidObjects.Add((GameObject)Instantiate(Resources.Load(Constants.Tutorial.GuidObjects[i]), UI_Game));
            GuidObjects[i].SetActive(false);
        }
        for(int i = 0; i < Constants.Tutorial.InteractiveObjects.Length; i++)
        {
            InteractiveObjects.Add(GameObject.Find(Constants.Tutorial.InteractiveObjects[i]));
            InteractiveObjects[i].SetActive(false);
            if (i >= 1)
            {
                PrefapImages.Add(InteractiveObjects[i].GetComponent<SpriteRenderer>().sprite);
            }
        }
        for (int i = 1; i < 4; i++)
        {
            SpawnPoints.Add(GameObject.Find($"SpawnPoint{i}").transform);
        }

        TutorialImages = Resources.LoadAll<Sprite>(Constants.Tutorial.TutorialImage);

        DayManager.Instance.SetTutorialDay();
    }

    private bool IsContinueTutorial(int TutorialNum, bool receivedBool = true)
    {
        if (!ContinueTutorialNums[TutorialNum] && ContinueTutorialNums[TutorialNum - 1] && receivedBool)
        {
            return true;
        }
        return false;
    }

    private void ContinueTutorial(int TutorialNum)
    {
        _isContinue = false;
        PanelNum = TutorialNum;
        Managers.UI.ShowPopupUI<UI_Tutorial>();
    }

    public void OnSkipButton()
    {
        Managers.UI.ShowPopupUI<UI_SelectSkip>();
    }
}
