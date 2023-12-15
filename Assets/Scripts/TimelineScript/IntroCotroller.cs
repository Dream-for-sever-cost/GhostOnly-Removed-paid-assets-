using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class IntroCotroller : MonoBehaviour
{
    [SerializeField] private Animator[] animator;
    [SerializeField] private Animator skulAnimator;
    private int _moveHash = Animator.StringToHash(Constants.AniParams.Move);
    private int _possess = Animator.StringToHash(Constants.AniParams.Possess);
    private Sequence _sequence;
    private void Start()
    {
        Managers.Sound.Play(Data.SoundType.IntroBGM, Define.Sound.Bgm);
        IntroPhase();
    }

    private void OnDisable()
    {
        DOTween.KillAll(this);
    }

    private void OnAnimatorMove()
    {
        foreach (Animator hero in animator)
        {
            hero.SetBool(_moveHash, true);
        }
    }

    private void OnAnimatorSkulPossess()
    {
        skulAnimator.SetTrigger(_possess);           
    }

    private void OnAnimatorSkulMove()
    {
        skulAnimator.SetBool(_moveHash, true);
    }

    private void IntroPhase()
    {
        _sequence = DOTween.Sequence().OnStart(() =>
        {
            DOVirtual.DelayedCall(15f, OnAnimatorMove, false);
            DOVirtual.DelayedCall(23f, OnAnimatorSkulPossess, false);
            DOVirtual.DelayedCall(24f, OnAnimatorSkulMove, false);
        });
    } 
}
