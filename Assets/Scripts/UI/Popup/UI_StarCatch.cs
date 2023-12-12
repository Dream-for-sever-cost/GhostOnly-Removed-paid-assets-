using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class UI_StarCatch : UI_Popup
{
    private int _successReward = 0;
    private int _failReward = 0;
    private int _difficulty = 0;
    private float _starSpeed = 0;

    private Sequence _successSequence;
    private Sequence _lampSequence;
    private Sequence _openSequence;
    private Sequence _starSequence;
    private Sequence _soulSequence;
    private Sequence _releaseSoulSequence;
    private Sequence _lampFailSequence;

    #region enum
    enum Images
    {
        GraveStoneImage,
        //StarCatchBarImage,
        StarImage,
        StarCatchDifficultyBGImage,
        Soul1Image,
        Soul2Image,
    }

    enum Texts
    {
        SoulText,
    }

    enum Buttons
    {
        StarCatchButton
    }

    enum HitPoints
    {
        HitPointImage,
    }

    enum GameObjects
    {
        Reward,
        StarCatchBG,
        SoulBG,
    }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Time.timeScale = 1.0f;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        Bind<HitPoint>(typeof(HitPoints));
        GetButton((int)Buttons.StarCatchButton).onClick.AddListener(CatchStar);
        GetObject((int)GameObjects.Reward).SetActive(false);
        switch (_difficulty)
        {
            case 0: SetEasy(); break;
            case 1: SetNormal(); break;
            case 2: SetHard(); break;
            case 3: SetSkill(); break;
            default: break;
        }
        _init = true;
        SetOpenSequence();
        StarSequence();
        SoulSequence();
        return true;
    }

    private void CatchStar()
    {
        HitPoint hit = Get<HitPoint>((int)HitPoints.HitPointImage);
        Animator starAnimation = GetImage((int)Images.StarImage).gameObject.GetComponent<Animator>();
        Button clickButton = GetButton((int)Buttons.StarCatchButton);
    
        ReleaseSoulSequence();
        _starSequence.Pause();
        if (hit.starInputChk)
        {           
            SuccessRewardSequence();
            SuccessLampSequence();
            Managers.Soul.GetSoul(_successReward);   
            clickButton.interactable = false;
            GetObject((int)GameObjects.Reward).SetActive(true);
            RewardSet(_successReward);
            DOVirtual.DelayedCall(Constants.Starcatch.ClosePopupTime, Close).SetUpdate(true);       
            Managers.Sound.PlaySound(Data.SoundType.StarcatchSuccess);
        }
        else
        {
            FailRewardSequence();
            FailLampSequence();
            Managers.Soul.GetSoul(_failReward);
            clickButton.interactable = false;
            GetObject((int)GameObjects.Reward).SetActive(true);
            RewardSet(_failReward);
            DOVirtual.DelayedCall(Constants.Starcatch.ClosePopupTime, Close).SetUpdate(true);
            Managers.Sound.PlaySound(Data.SoundType.StarcatchFail);
        }
    }

    private void Close()
    {
        DOTween.KillAll();
        Managers.UI.ClosePopupUI();
    }

    private void SetEasy()
    {

    }

    private void SetNormal()
    {

        GetImage((int)Images.StarCatchDifficultyBGImage).color = Constants.Starcatch.NormalStarcatchColor;
    }

    private void SetHard()
    {

        GetImage((int)Images.StarCatchDifficultyBGImage).color = Constants.Starcatch.HardStarcatchColor;
    }

    private void SetSkill()
    {
        GetImage((int)Images.StarCatchDifficultyBGImage).color = Constants.Starcatch.SkillStarcatchColor;
    }

    private void RewardSet(int soul)
    {
        GetObject((int)GameObjects.Reward).SetActive(true);
        GetText((int)Texts.SoulText).text = $"+{soul}";
    }

    public void Initialized(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                this._difficulty = difficulty;
                _starSpeed = Constants.Starcatch.EasyStarSpeed;
                _successReward = Constants.Starcatch.EasySuccessReward;
                _failReward = Constants.Starcatch.EasyFailReward;
                break;
            case 1:
                this._difficulty = difficulty;
                _starSpeed = Constants.Starcatch.NormalStarSpeed;
                _successReward = Constants.Starcatch.NormalSuccessReward;
                _failReward = Constants.Starcatch.NormalFailReward;
                break;
            case 2:
                this._difficulty = difficulty;
                _starSpeed = Constants.Starcatch.HardStarSpeed;
                _successReward = Constants.Starcatch.HardSuccessReward;
                _failReward = Constants.Starcatch.HardFailReward;
                break;
            case 3:
                this._difficulty = difficulty;
                _starSpeed = Constants.Starcatch.SkillStarSpeed;
                int chance = UnityEngine.Random.Range(0, Constants.Starcatch.ChanceEnd);
                if (chance < Constants.Starcatch.Chance)
                {
                    _successReward = Constants.Starcatch.SkillSuccessPlusReward;
                }
                else
                {
                    _successReward = Constants.Starcatch.SkillSuccessReward;
                }
                _failReward = Constants.Starcatch.SkillFailReward;
                break;

            default: break;
        }
    }

    private void SuccessLampSequence()
    {
        GameObject lamp = GetButton((int)Buttons.StarCatchButton).gameObject;

        _lampSequence = DOTween.Sequence()
        .Append(lamp.transform.DOShakePosition(0.7f, Constants.Starcatch.LampShakeStrength, Constants.Starcatch.LampShakeVivrato))
        .PrependInterval(Constants.Starcatch.SuccessLampPrependDelay)
        .SetUpdate(true);
    }

    private void FailLampSequence()
    {
        GameObject lamp = GetButton((int)Buttons.StarCatchButton).gameObject;
        _lampFailSequence = DOTween.Sequence()       

        .Append(lamp.GetComponent<CanvasGroup>().DOFade(0.6f, Constants.Starcatch.FailLampAppendFadeDuration))
        .SetUpdate(true);
    }

    private void SuccessRewardSequence()
    {
        GameObject reward = GetObject((int)GameObjects.Reward);
        _successSequence = DOTween.Sequence()
           .OnStart(() =>
           {
               reward.transform.localScale = Vector3.zero;
               reward.GetComponent<CanvasGroup>().alpha = 0;
           })
           .Prepend(reward.transform.DOScale(1, Constants.Starcatch.S_RewardPrependScaleDuration)).SetEase(Ease.Linear)
           .Join(reward.GetComponent<CanvasGroup>().DOFade(1, Constants.Starcatch.S_RewardJoinFadeDuration)).AppendInterval(Constants.Starcatch.S_RewardJoinAppendDelay)
           .Append(reward.transform.DOLocalMove(Constants.Starcatch.S_RewardAppendLocalMove, Constants.Starcatch.S_RewardAppendMoveDuration))
           .Join(reward.transform.DOScale(0, Constants.Starcatch.S_RewardJoinScaleDuration))
           .SetUpdate(true);
    }

    public void SetOpenSequence()
    {
        GameObject window = GetObject((int)GameObjects.StarCatchBG);
        _openSequence = DOTween.Sequence()   
        .OnStart(() =>
        {
            window.SetActive(true);
            window.transform.localScale = Vector3.zero;
            window.GetComponent<CanvasGroup>().alpha = 0;
        })
       .Append(window.transform.DOScale(1, Constants.Starcatch.OpenAppendScaleDuration)).SetEase(Ease.Linear)
       .Join(window.GetComponent<CanvasGroup>().DOFade(1, Constants.Starcatch.OpenJoinFadeDuration))
       .SetUpdate(true);
    }

    public void StarSequence()
    {
        GameObject star = GetImage((int)Images.StarImage).gameObject;
        _starSequence = DOTween.Sequence()
       .Prepend(star.transform.DOLocalMoveX(Constants.Starcatch.StarPrependMoveX, _starSpeed).SetEase(Ease.InOutSine)).SetRelative()
       .Append(star.transform.DOLocalMoveX(Constants.Starcatch.StarAppendMoveX, _starSpeed).SetEase(Ease.InOutSine)).SetRelative()   
       .SetLoops(-1)
       .SetUpdate(true);
    }

    public void SoulSequence()
    {
        GameObject soul1 = GetImage((int)Images.Soul1Image).gameObject;
        GameObject soul2 = GetImage((int)Images.Soul2Image).gameObject;

        _soulSequence = DOTween.Sequence()
            .Prepend(soul1.transform.DOLocalMoveY(Constants.Starcatch.Soul1PrependMoveY, 1f))
            .Join(soul2.transform.DOLocalMoveY(Constants.Starcatch.Soul2JoinMoveY, 1f))
            .SetEase(Ease.InOutSine)
            .Append(soul1.transform.DOLocalMoveY(Constants.Starcatch.Soul1AppendMoveY, 1f))
            .Join(soul2.transform.DOLocalMoveY(Constants.Starcatch.Soul2JoinMoveY2, 1f))
            .SetEase(Ease.InOutSine)
            .SetLoops(-1)
            .SetRelative()
            .SetUpdate(true);
    }

    public void ReleaseSoulSequence()
    {
        GameObject soulBG = GetObject((int)GameObjects.SoulBG);

        _releaseSoulSequence = DOTween.Sequence()
            .Append(soulBG.GetComponent<CanvasGroup>().DOFade(0, Constants.Starcatch.ReleaseAppendFadeDuration))
            .SetUpdate(true);   
    }

    public void FailRewardSequence()
    {
        GameObject reward = GetObject((int)GameObjects.Reward);
        reward.GetComponent<CanvasGroup>().DOFade(1, Constants.Starcatch.F_RewardFadeDuration)
            .SetUpdate(true);
    }

    private void Sequencekill()
    {
        _successSequence.Kill(true);
        _lampSequence.Kill(true);
        _openSequence.Kill(true);
        _starSequence.Kill(true);
        _soulSequence.Kill(true);
        _releaseSoulSequence.Kill(true);
        _lampFailSequence.Kill(true);
    }
}
