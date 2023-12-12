using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditSoulDOtween : MonoBehaviour
{
    private Sequence _soulMoveSequence;
    private Sequence _soulIdleSequence;

    private void Start()
    {
        SoulIdleSequence();
        DOVirtual.DelayedCall(21f, SoulMoveSequence);
    }

    private void SoulMoveSequence()
    {
        _soulMoveSequence = DOTween.Sequence()
            .Append(transform.DOScale(0f, 0.4f)).SetEase(Ease.InExpo);
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