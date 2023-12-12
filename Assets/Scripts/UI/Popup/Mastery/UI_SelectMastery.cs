using System;
using UnityEngine;
using DG.Tweening;

public class UI_SelectMastery : UI_Popup
{
    private enum Buttons
    {
        StandardButton,
        RandomButton,
        BackButton
    }

    private enum GameObjects
    {
        Background
    }

    private SlaveMasteryController _controller;
    private MasteryManager.EMasteryOpenType _openType;

    public override bool Init()
    {
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        GetButton((int)Buttons.StandardButton).BindEvent(SelectStandard);
        GetButton((int)Buttons.RandomButton).BindEvent(SelectRandom);
        GetButton((int)Buttons.BackButton).BindEvent(ClosePopupUI);
        GetObject((int)GameObjects.Background).GetComponent<CanvasGroup>().alpha = 0f;
        openSequence();
        _init = true;
        return true;
    }

    public void Setup(SlaveMasteryController controller, MasteryManager.EMasteryOpenType openType)
    {
        _controller = controller;
        _openType = openType;
    }

    private void Update()
    {
        //todo remove this 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.UI.ClosePopupUI(this);
        }
    }

    private void SelectStandard()
    {
        bool isSuccess = _openType switch
        {
            MasteryManager.EMasteryOpenType.Main => _controller.AddStandardMainMastery(),
            MasteryManager.EMasteryOpenType.Sub => _controller.AddStandardSubMastery(),
            _ => throw new ArgumentOutOfRangeException()
        };

        if (isSuccess)
        {
            Managers.UI.ClosePopupUI(this);
        }
        else
        {
            //todo 
            Debug.Log("Failed to add mastery");
        }
    }

    private void SelectRandom()
    {
        bool isSuccess = _openType switch
        {
            MasteryManager.EMasteryOpenType.Main => _controller.AddRandomMainMastery(),
            MasteryManager.EMasteryOpenType.Sub => _controller.AddRandomSubMastery(),
            _ => throw new ArgumentOutOfRangeException()
        };

        if (isSuccess)
        {
            Managers.UI.ClosePopupUI(this);
        }
        else
        {
            //todo 
            Debug.Log("Failed to add mastery");
        }
    }

    private void openSequence()
    {
        GameObject background = GetObject((int)GameObjects.Background);
        Sequence sequence = DOTween.Sequence()
            .OnStart(() =>
            {
                background.GetComponent<CanvasGroup>().alpha = 0;
                background.transform.localScale = Vector3.zero;
            })
            .Append(background.GetComponent<CanvasGroup>().DOFade(1, 0.1f))
            .Join(background.transform.DOScale(1, 0.1f))
            .SetUpdate(true);
    }
}
