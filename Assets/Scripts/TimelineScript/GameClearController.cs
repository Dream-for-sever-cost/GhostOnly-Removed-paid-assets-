using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Util;

public class GameClearManager : MonoBehaviour
{
    [SerializeField] private GameObject[] Soul;
    [SerializeField] private Animator[] _heros;
    [SerializeField] private Animator _ghost;
    [SerializeField] private Animator _altar;

    private Sequence _textSequence;
    private int _stop = Animator.StringToHash(Constants.AniParams.Stop);
    private int _move = Animator.StringToHash(Constants.AniParams.Move);
    private int _clearIdle = Animator.StringToHash(Constants.AniParams.ClearIdle);
    private int _die = Animator.StringToHash(Constants.AniParams.Die);

    private void Start()
    {
        Managers.Sound.Play(Data.SoundType.GameClearBGM, Define.Sound.Bgm);
        GameClearStep();
    }

    private void GameClearStep()
    {
        SetAltarIdle();
        SetGhostMove();
        DOVirtual.DelayedCall(9f, SetGhostStop);
        DOVirtual.DelayedCall(11f, SetHeroDie);
    }

    private void SetGhostMove()
    {
        _ghost.SetBool(_move, true);
    }

    private void SetGhostStop()
    {
        _ghost.SetBool(_move, false);
    }

    private void SetHeroDie()
    {
        foreach (var hero in _heros)
        {
            hero.SetBool(_die, true);
        }
    }

    private void SetAltarIdle()
    {
        _altar.SetBool(_clearIdle, true);
    }  
}
