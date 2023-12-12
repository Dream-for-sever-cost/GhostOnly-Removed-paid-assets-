using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Animator[] hero;
    [SerializeField] private Animator ghost;
    [SerializeField] private Animator altar;
    private Sequence _gameOverSequence;
   
    private int _move = Animator.StringToHash(Constants.AniParams.Move);
    private int _fail = Animator.StringToHash(Constants.AniParams.Fail);

    private void Start()
    {
        Managers.Sound.Play(Data.SoundType.GameOverBGM, Define.Sound.Bgm);
        GameOverStep();
    }

    private void SetGhostMove()
    {
        ghost.SetBool(_move, true);
    }
  
    private void SetHeroMove()
    {
        foreach (Animator heros in hero)
        {
            heros.SetBool(_move, true);
        }
    }

    private void SetHeroStop()
    {
        foreach (Animator heros in hero)
        {
            heros.SetBool(_move, false);
        }
    }

    private void SetAltarFail()
    {
        altar.SetBool(_fail, true);
    }

    private void GameOverStep()
    {
        _gameOverSequence = DOTween.Sequence().OnStart(() =>
        {
            
            SetAltarFail();   
            SetHeroMove();
            DOVirtual.DelayedCall(2f, SetGhostMove);
            DOVirtual.DelayedCall(4f, SetHeroStop);
        });
    }
}
