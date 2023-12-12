using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearSoulDOTween : MonoBehaviour
{
    private Sequence _soulMoveSequence;
    private Sequence _soulIdleSequence;

    private void Start()
    {
        SoulIdleSequence();
        DOVirtual.DelayedCall(10f, SoulMoveSequence);
    }
   
    private void SoulMoveSequence()
    {
        _soulMoveSequence = DOTween.Sequence()
            .Append(transform.DOScale(0f, 1f)).SetEase(Ease.Linear);             
    }

    private void SoulIdleSequence()
    {
        _soulIdleSequence = DOTween.Sequence()
            .Prepend(transform.DOLocalMoveY(0.1f, 1f))       
            .Append(transform.DOLocalMoveY(-0.1f, 1f))
            .SetEase(Ease.InOutSine)
            .SetRelative()
            .SetLoops(-1);      
    }
}
