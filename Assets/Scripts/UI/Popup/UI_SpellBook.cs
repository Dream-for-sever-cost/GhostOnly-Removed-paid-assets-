using Data;
using DG.Tweening;
using Manager;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class UI_SpellBook : UI_Popup
{
    SpellBookController spellBookController;
    private string _spellId;
    private Sequence _openCoffinSequence;
    private Sequence _openSpellSequence;

    #region Enum
    enum GameObjects
    {
        DetailPanel,
        Background,
    }

    enum Texts
    {
        //SoulTxt,
        SpellNameText,
        SpellEffectText,
    }

    enum Buttons
    {
        CloseButton,
        DetailCloseButton,
        UnlockButton,

        // TODO 테스트 후 제거
        //GetSoulButton,
    }

    enum Images
    {
        A0, A1,
        B0, B1, B2, B3, B4, B5,
        C0, C1,C2,
        D0, D1,
        E0, E1,
        F0,
        G0, G1,
        H0,
        SpellImage,
    }
    #endregion

    private void Start()
    {
        spellBookController = Managers.Resource.Load<GameObject>(Constants.Prefabs.SpellBook).GetComponent<SpellBookController>();
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickedCloseButton);
        GetButton((int)Buttons.DetailCloseButton).gameObject.BindEvent(OnClickedDetailCloseButton);
        GetObject((int)GameObjects.DetailPanel).SetActive(false);
        GetObject((int)GameObjects.Background).transform.localScale = Vector3.zero;
        OpenSequence();
        foreach (string spellId in Managers.SpellBook.SpellDataDic.Keys)
        {
            if (Managers.SpellBook.SpellDataDic[spellId].parentId.Equals(Constants.Spellbook.Root))
            {
                InitSpellTree(spellId);
            }
        }

        return true;
    }

    private void OnClickedCloseButton()
    {
        SequenceKill();
        Managers.Sound.PlaySound(Data.SoundType.Click);
        ClosePopupUI();
    }

    private void OnClickedDetailCloseButton()
    {
        GetObject((int)GameObjects.DetailPanel).SetActive(false);

        Managers.Sound.PlaySound(SoundType.Click);
    }

    private void OnClickedUnlockButton()
    {
        string parentId = Managers.SpellBook.SpellDataDic[_spellId].parentId;
        if (parentId.Equals(Constants.Spellbook.Root) == false)
        {
            if (Managers.SpellBook.SpellDataDic[Managers.SpellBook.SpellDataDic[_spellId].parentId].isActivated == false)
                return;
        }

        if (Managers.Soul.CheckSoul(Managers.SpellBook.SpellDataDic[_spellId].price))
        {
            spellBookController.UpdateEffect(_spellId);
            spellBookController.ActivateSpell(_spellId);
            spellBookController.UnlockSpell(_spellId);

            Managers.Soul.UseSoul(Managers.SpellBook.SpellDataDic[_spellId].price);
            ShowDetailPanel();
            UpdateChildSpell(Color.white);
            UpdateActivateSpell();

            Managers.Sound.PlaySound(SoundType.GetSpell);
        }
        else
        {
            Managers.Sound.PlaySound(SoundType.PurchaseFail);
        }
    }

    public void ShowDetailPanel()
    {
        Managers.Sound.PlaySound(SoundType.Interaction);

        GetImage((int)Images.SpellImage).sprite = GetImage((int)Enum.Parse(typeof(Images), _spellId)).sprite;

        switch (PreferencesManager.GetLanguage())
        {
            case Language.KOREAN:
                GetText((int)Texts.SpellNameText).text = GetString(Managers.SpellBook.SpellDataDic[_spellId].name);
                GetText((int)Texts.SpellEffectText).text = GetString(Managers.SpellBook.SpellDataDic[_spellId].explanation);
                break;
            case Language.ENGLISH:
                GetText((int)Texts.SpellNameText).text = GetString(Managers.SpellBook.SpellDataDic[_spellId].name);
                GetText((int)Texts.SpellEffectText).text = GetString(Managers.SpellBook.SpellDataDic[_spellId].explanation);
                break;
        }

        GetButton((int)Buttons.UnlockButton).gameObject.SetActive(true);
        GetButton((int)Buttons.UnlockButton).GetComponentInChildren<TextMeshProUGUI>().text = $"x {Managers.SpellBook.SpellDataDic[_spellId].price}";

        if (Managers.SpellBook.SpellDataDic[_spellId].isActivated)
        {
            GetButton((int)Buttons.UnlockButton).gameObject.SetActive(false);
        }
        else
        {
            if (Managers.SpellBook.SpellDataDic[_spellId].parentId.Equals(Constants.Spellbook.Root))
            {
                GetButton((int)Buttons.UnlockButton).gameObject.BindEvent(OnClickedUnlockButton);
                GetButton((int)Buttons.UnlockButton).interactable = true;
            }
            else if (Managers.SpellBook.SpellDataDic[Managers.SpellBook.SpellDataDic[_spellId].parentId].isActivated == false
                    || Managers.SpellBook.SpellDataDic[Managers.SpellBook.SpellDataDic[_spellId].parentId].isLocked)
            {
                GetButton((int)Buttons.UnlockButton).interactable = false;
            }
            else if (!Managers.Soul.CheckSoul(Managers.SpellBook.SpellDataDic[_spellId].price))
            {
                GetButton((int)Buttons.UnlockButton).interactable = false;
            }
            else
            {
                GetButton((int)Buttons.UnlockButton).interactable = true;
                GetButton((int)Buttons.UnlockButton).gameObject.BindEvent(OnClickedUnlockButton);
                GetButton((int)Buttons.UnlockButton).GetComponentInChildren<TextMeshProUGUI>().text = $"x {Managers.SpellBook.SpellDataDic[_spellId].price}";
            }
        }

        GetObject((int)GameObjects.DetailPanel).SetActive(true);
    }

    private void InitSpellTree(string spellId)
    {
        SetSpellId(spellId);
        Image spellImage = GetImage((int)Enum.Parse(typeof(Images), spellId));
        spellImage.gameObject.BindEvent(() => SetSpellId(spellId));
        spellImage.gameObject.BindEvent(ShowDetailPanel);
        spellImage.transform.GetChild(0).gameObject.SetActive(false);
        bool parentActivate = false;
        string parentId = Managers.SpellBook.SpellDataDic[_spellId].parentId;


        if (parentId.Equals(Constants.Spellbook.Root))
        {
            parentId = _spellId;
        }


        if (Managers.SpellBook.SpellDataDic[parentId].isLocked || Managers.SpellBook.SpellDataDic[parentId].isActivated == false)
        {
            UpdateChildSpell(Color.gray);
            if (parentId.Equals(_spellId))
            {
                GetImage((int)Enum.Parse(typeof(Images), _spellId)).color = Color.white;
            }
        }
        else
        {
            UpdateChildSpell(Color.white);
        }

        // DOTween Check
        if (Managers.SpellBook.SpellDataDic[parentId].isActivated)
        {
            parentActivate = true;
        }

        UpdateActivateSpell(parentActivate);


        foreach (string childId in Managers.SpellBook.SpellDataDic[_spellId].childrens)
        {
            InitSpellTree(childId);
        }
    }

    private void UpdateChildSpell(Color color)
    {
        Image spellImage = GetImage((int)Enum.Parse(typeof(Images), _spellId));

        foreach (Image arrowImage in spellImage.transform.GetComponentsInChildren<Image>())
        {
            if (arrowImage.name == Constants.Spellbook.CheckImage) continue;
            arrowImage.color = color;
        }
        foreach (string childId in Managers.SpellBook.SpellDataDic[_spellId].childrens)
        {
            GetImage((int)Enum.Parse(typeof(Images), childId)).color = color;
        }
    }

    private void UpdateActivateSpell(bool chk = false)
    {
        if (!chk && !Managers.SpellBook.SpellDataDic[_spellId].isLocked)
        {
            OpenSpellSequence();
        }

        if (Managers.SpellBook.SpellDataDic[_spellId].isActivated)
        {
            GetImage((int)Enum.Parse(typeof(Images), _spellId)).transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void SetSpellId(string spellId)
    {
        _spellId = spellId;
    }


    private void OpenSequence()
    {
        GameObject panel = GetObject((int)GameObjects.Background);
        _openCoffinSequence = DOTween.Sequence()
            .Append(panel.transform.DOScale(1, Constants.Coffin.ClickPanelAppendScaleDuration)).SetEase(Ease.Linear).SetUpdate(true);
    }

    private void OpenSpellSequence()
    {
        GameObject spellChk = GetImage((int)Enum.Parse(typeof(Images), _spellId)).transform.GetChild(0).gameObject;
        _openSpellSequence = DOTween.Sequence()
            .OnStart(() =>
            {
                spellChk.GetComponent<CanvasGroup>().alpha = 0f;
            })
            .Append(spellChk.GetComponent<CanvasGroup>().DOFade(1f, 0.7f))
            .SetUpdate(true);
    }

    private void SequenceKill()
    {
        _openCoffinSequence.Kill();
        _openSpellSequence.Kill();
    }
}