using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class CreditController : MonoBehaviour
{
    [SerializeField] private Animator[] _heros;
    [SerializeField] private Animator[] _skuls;
    [SerializeField] private Animator _ghost;
    [SerializeField] private Animator _altar;
    [SerializeField] private GameObject _altarEffect;

    private int _move = Animator.StringToHash(Constants.AniParams.Move);
    private int _AltarIdle = Animator.StringToHash(Constants.AniParams.CreditIdle);
    private int _AltarEnd = Animator.StringToHash(Constants.AniParams.CreditEnd);
    private int _die = Animator.StringToHash(Constants.AniParams.Die);
    private int _OnEffect = Animator.StringToHash(Constants.AniParams.AltarEffect);

    private Sequence _creditSequence;
    private void Start()
    {
        Managers.Sound.Play(Data.SoundType.CreditBGM, Define.Sound.Bgm);
        CreditStep();
    }

    private void OnDisable()
    {
        DOTween.KillAll(this);
    }

    private void CreditStep()
    {
        SetAltarIdle();
        SetHeroMove();
        SetSkulMove();
        SetGhostMove();
        CreditSequence();
    }

    private void CreditSequence()
    {
        _creditSequence = DOTween.Sequence()
            .OnStart(() =>
            {
                DOVirtual.DelayedCall(22f, SetAltarEnd);
                DOVirtual.DelayedCall(22f, SetAltarEffect);
            });
    }

    private void SetHeroMove()
    {
        foreach (var hero in _heros)
        {
            hero.SetBool(_move, true);
        }
    }

    private void SetSkulMove()
    {
        foreach(var skul in _skuls)
        {
            skul.SetBool(_move, true);
        }
    }


    private void SetGhostMove()
    {
        _ghost.SetBool(_move, true);
    }

    private void SetAltarIdle()
    {
        _altar.SetBool(_AltarIdle, true);
    }

    private void SetAltarEnd()
    {
        _altar.SetBool(_AltarEnd, true);
    }

    private void SetAltarEffect()
    {
        _altarEffect.SetActive(true);
        _altarEffect.GetComponent<Animator>().SetBool(_OnEffect, true);
    }
}
