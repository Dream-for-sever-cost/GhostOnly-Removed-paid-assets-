using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class ResultDotween : MonoBehaviour
{
    private Sequence _openSequence;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private TextMeshProUGUI soulText;
    [SerializeField] private TextMeshProUGUI heroKillText;

    [SerializeField] private TextMeshProUGUI dayHeaderText;
    [SerializeField] private TextMeshProUGUI playTimeHeaderText;
    [SerializeField] private TextMeshProUGUI soulHeaderText;
    [SerializeField] private TextMeshProUGUI heroKillHeaderText;
    [SerializeField] private TextMeshProUGUI backText;


    private void Start()
    {
        dayHeaderText.text = Managers.UI.GetString(Constants.StringRes.ResultDay);
        playTimeHeaderText.text = Managers.UI.GetString(Constants.StringRes.ResultPlaytime);
        soulHeaderText.text = Managers.UI.GetString(Constants.StringRes.ResultSoul);
        heroKillHeaderText.text = Managers.UI.GetString(Constants.StringRes.ResultHuman);
        backText.text = Managers.UI.GetString(Constants.StringRes.ResultBack);

        resultText.text = Managers.GameManager.isGameClear ? Constants.Setting.GameClear : Constants.Setting.GameOver;
        dayText.text = Managers.GameManager.currentDay.ToString();
        playTimeText.text = $"{(int)(Managers.GameManager.realTime / 60)} : {(int)(Managers.GameManager.realTime % 60)}";
        soulText.text = Managers.Soul.EarnedSoul.ToString();
        heroKillText.text = Managers.GameManager.heroDeathCount.ToString();
    }

    private void openSequence()
    {
        _openSequence = DOTween.Sequence()
        .Append(GetComponent<CanvasGroup>().DOFade(1, 0.5f)).SetUpdate(true);
    }

    private void OnEnable()
    {
        openSequence();
    }

    private void OnDisable()
    {
        _openSequence.Kill(true);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("LobbyScene");
    }
}
