using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Util;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    [SerializeField] private Light2D _generalLight2D;
    [SerializeField] private Gradient _lightColor;

    [Header("For TEST")]
    [SerializeField] private int testTimeScale = 1;

    public float RealTime { get; private set; }

    public float GameTime { get; private set; } // 현재 경과 시간
    public int CurrentDay { get; private set; } // 현재 날짜 (보름달이 뜨는 날 지나면 초기화)

    public bool IsNight { get; private set; } = false;

    public event Action<bool> OnChangedDayStatus;

    private bool _isDayMusic;
    private bool _isNightMusic;

    public float RatioOfDayNight
    {
        get
        {
            if (IsNight)
            {
                if (Constants.Day.MoonRise < GameTime)
                {
                    return (GameTime - Constants.Day.MoonRise) / (Constants.Day.SunRise + Constants.Day.DayLength - Constants.Day.MoonRise);
                }

                return (GameTime + (Constants.Day.DayLength - Constants.Day.MoonRise)) / (Constants.Day.SunRise + Constants.Day.DayLength - Constants.Day.MoonRise);
            }

            return (GameTime - Constants.Day.SunRise) / (Constants.Day.MoonRise - Constants.Day.SunRise);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameTime = 0;
        CurrentDay = 0;
        IsNight = false;

        RealTime = 0;

        GameTime = Constants.Day.MoonRise;
    }

    void Update()
    {
        if (GameTime >= Constants.Day.DayLength)
        {
            GameTime = 0;
            CurrentDay += 1;
        }
        else
        {
            GameTime += Time.deltaTime * Constants.Day.InverseDayScale * testTimeScale;
        }

        RealTime += Time.deltaTime;

        _generalLight2D.color = _lightColor.Evaluate(GameTime / Constants.Day.DayLength);

        bool nextIsNight = GameTime <= Constants.Day.SunRise || Constants.Day.MoonRise <= GameTime;

        if (nextIsNight != IsNight)
        {
            IsNight = nextIsNight;
            OnChangedDayStatus?.Invoke(IsNight);
        }

        if(!IsNight && !_isDayMusic)
        {
            _isDayMusic = true;
            _isNightMusic = false;
            Managers.Sound.Play(Data.SoundType.DayBGM, Define.Sound.Bgm);
            Managers.Sound.PlaySound(Data.SoundType.Morning);
        }

        if(IsNight && !_isNightMusic)
        {
            _isDayMusic = false;
            _isNightMusic = true;
            Managers.Sound.Play(Data.SoundType.NightBGM, Define.Sound.Bgm);
        }
    }

    public int MoonIndex() => CurrentDay % Constants.Day.FullMoonCycle;

    public void SetTutorialDay()
    {
        CurrentDay = 0;
        GameTime = 0;
        testTimeScale = 0;
    }
}