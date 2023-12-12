using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class UI_Tutorial : UI_Popup
{
    #region enum
    enum Images
    {
        PanelImage,
        PanelExampleImage,
        PlayerImage,
        SkullImage,
        StoryBoxImage,
        CoffinImage,
        AltarImage,
        SpellBookImage,
        GraveStoneImage,
        BackGroundImage
    }
    enum Texts
    {
        PanelInfoText,
        PanelHeadLineText,
        StoryBoxText,
    }
    enum Buttons
    {
        NextStoryButton,
        PanelCloseButton,
        SkipButton
    }
    #endregion

    private TutorialManager TM;
    private Sequence TutorialSequence;
    private TextMeshProUGUI _text;

    private void OnDestroy()
    {
        DOTween.Kill(TutorialSequence);
    }

    public override bool Init()
    {

        if (base.Init() == false)
            return false;

        TM = TutorialManager.Instance;
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        for (int i = 0; i < TM.GuidObjects.Count; i++)
        {
            TM.GuidObjects[i].SetActive(false);
        }

        GetButton((int)Buttons.PanelCloseButton).BindEvent(() => PanelCloseButton());
        GetButton((int)Buttons.NextStoryButton).BindEvent(() => OnNextStoryButton());
        GetButton((int)Buttons.SkipButton).BindEvent(() => TM.OnSkipButton());

        GetButton((int)Buttons.NextStoryButton).gameObject.SetActive(false);
        GetImage((int)Images.PanelImage).gameObject.SetActive(false);

        _text = GetText((int)Texts.StoryBoxText);

        ProgressTutorial();

        return true;
    }

    #region BasicFunction
    private void ImageMove()
    {
        GetText((int)Texts.StoryBoxText).text = "";
        TutorialSequence
                .Join(GetImage((int)Images.StoryBoxImage).rectTransform.DOAnchorPos(new Vector3(0, 250, 0), Constants.Tutorial.ImageMoveDuration));
    }
    private void ImageFadeIn(int ImageNum)
    {
        TutorialSequence.Join(GetImage(ImageNum).GetComponent<CanvasGroup>().DOFade(1, Constants.Tutorial.ImageFadeDuration));
    }
    private void ImageFadeOut(int ImageNum)
    {
        TutorialSequence.Join(GetImage(ImageNum).GetComponent<CanvasGroup>().DOFade(0, Constants.Tutorial.ImageFadeDuration));
    }
    private void IsFade()
    {
        for (int i = 0; i < TM.TutorialFade.Length; i++)
        {
            if (PanelCheck(TM.TutorialFade[i].PanelNum, TM.TutorialFade[i].CountNum))
            {
                for (int j = 0; j < TM.TutorialFade[i].TutoImages.Count; j++)
                {
                    if (TM.TutorialFade[i].IsFadeIn[j])
                    {
                        if (TM.PanelNum == 4)
                        {
                            GetImage((int)Images.CoffinImage).transform.position = new Vector2(960, 540);
                        }
                        else if (TM.PanelNum == 10)
                        {
                            GetImage((int)Images.SpellBookImage).transform.position = new Vector2(960, 540);
                        }
                        ImageMove();
                        ImageFadeIn((int)TM.TutorialFade[i].TutoImages[j]);
                    }
                    else
                    {
                        ImageFadeOut((int)TM.TutorialFade[i].TutoImages[j]);
                    }
                }
            }
        }
    }
    private void ActiveObject()
    {
        for (int i = 0; i < TM.TutorialActive.Length; i++)
        {
            if (PanelCheck(TM.TutorialActive[i].PanelNum, TM.TutorialActive[i].CountNum))
            {
                foreach (int j in TM.TutorialActive[i].InteractiveObject)
                {
                    if (j > 5)
                    {
                        if (j == 6)
                        {
                            Instantiate(Resources.Load(Constants.Prefabs.Skull), TM.SpawnPoints[2]);
                        }
                        else
                        {
                            for (int q = 0; q < 2; q++)
                            {
                                GameObject EnemyObject = (GameObject)Instantiate(Resources.Load(Constants.Tutorial.Scarecrow), TM.SpawnPoints[q]);
                                EnemyObject.GetComponent<Scarecrow>().init();
                            }
                        }
                        break;
                    }
                    else if (j != -1)
                    {
                        TM.InteractiveObjects[j].SetActive(true);
                    }
                }
                foreach (int j in TM.TutorialActive[i].GuideObject)
                {
                    if (j == 1)
                    {
                        ClearTutorial();
                    }
                    else if (j != -1)
                    {
                        TM.GuidObjects[j].SetActive(true);
                    }
                }
                if ((int)TM.TutorialActive[i].GuideImage != -1)
                {
                    TM.GuidObjects[7].GetComponent<Image>().sprite = TM.PrefapImages[(int)TM.TutorialActive[i].GuideImage];
                }
            }
        }
    }
    private void ClearTutorial()
    {
        TM.GuidObjects[1].SetActive(true);

        GetImage((int)Images.StoryBoxImage).gameObject.SetActive(false);
        GetImage((int)Images.BackGroundImage).gameObject.SetActive(false);
        TutorialSequence.SetUpdate(true)
            .Append(TM.GuidObjects[1].GetComponent<CanvasGroup>().DOFade(1, Constants.Tutorial.ImageFadeDuration / 2))
            .InsertCallback(1, () => Managers.Scene.ChangeScene(Define.Scene.LobbyScene));
    }
    private bool PanelCheck(int panelNum, int count)
    {
        if (TM.PanelNum == panelNum && TM.count == count)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Button/Text
    private void PanelCloseButton()
    {
        for (int i = 0; i < TM.TutorialInfos.Length; i++)
        {
            if (TM.PanelNum == TM.TutorialInfos[i].PanelNum)
            {
                if (TM.count != TM.TutorialInfos[i].NextTutorialNum)
                {
                    TutorialSequence = DOTween.Sequence().SetUpdate(true);
                    GetImage((int)Images.PanelImage).gameObject.SetActive(false);
                    TM.ContinueTutorialNums[TM.PanelNum] = true;
                    StroyTyping();
                    return;
                }
            }
        }

        if (TM.PanelNum == 10 && TM.count == 13)
        {
            TM.ContinueTutorialNums[TM.PanelNum] = true;
            Managers.UI.ClosePopupUI(this);
            return;
        }

        ActiveObject();
        TM.ContinueTutorialNums[TM.PanelNum] = true;
        Managers.UI.ClosePopupUI(this);
    }
    private void OnNextStoryButton()
    {
        TM.count++;

        for (int i = 0; i < TM.TutorialInfos.Length; i++)
        {
            if (PanelCheck(TM.TutorialInfos[i].PanelNum, TM.TutorialInfos[i].NextTutorialNum) && TM.count != TM.TutorialInfos[i].ActivePanelNum)
            {
                ActiveObject();
                Managers.UI.ClosePopupUI(this);
                return;
            }
        }

        if (TM.PanelNum == 10)
        {
            ActiveObject();
            Managers.UI.ClosePopupUI(this);
            return;
        }

        ProgressTutorial();
        GetButton((int)Buttons.NextStoryButton).gameObject.SetActive(false);
    }
    private void PanelChange()
    {
        GetImage((int)Images.PanelImage).gameObject.SetActive(true);
        GetImage((int)Images.StoryBoxImage).GetComponent<CanvasGroup>().alpha = 0;
        GetImage((int)Images.PanelImage).gameObject.SetActive(true);
        GetText((int)Texts.PanelInfoText).text = GetString(Constants.Tutorial.Explains[TM.PanelNum]);      //설명
        GetText((int)Texts.PanelHeadLineText).text = GetString(Constants.Tutorial.Headline[TM.PanelNum]);  //헤드라인
        GetImage((int)Images.PanelExampleImage).sprite = TM.TutorialImages[TM.PanelNum];  //이미지
    }
    private void StroyTyping()
    {
        GetImage((int)Images.PanelImage).gameObject.SetActive(false);
        GetImage((int)Images.StoryBoxImage).GetComponent<CanvasGroup>().alpha = 1.0f;
        _text.text = "";

        string str = GetString(Constants.Tutorial.Storys[TM.count]);

        while (str.Contains("<") && str.Contains(">"))
        {
            var pos1 = str.IndexOf("<");
            var pos2 = str.IndexOf(">");
            str = str.Remove(pos1, pos2 - pos1 + 1);
        }

        TutorialSequence
            .Append(_text.DOText(GetString(Constants.Tutorial.Storys[TM.count]), (str.Length * Constants.Tutorial.TextTypingDuration)).SetEase(Ease.Linear)
            .OnComplete(() => GetButton((int)Buttons.NextStoryButton).gameObject.SetActive(true)));
    }
    #endregion
    private void ProgressTutorial()
    {
        TutorialSequence = DOTween.Sequence().SetUpdate(true);

        IsFade();

        for (int i = 0; i < TM.TutorialInfos.Length; i++)
        {
            if (TM.PanelNum == TM.TutorialInfos[i].PanelNum)
            {
                if (TM.count != TM.TutorialInfos[i].ActivePanelNum)
                {
                    StroyTyping();
                    return;
                }
            }
        }

        if (TM.PanelNum == 13)
        {
            if (TM.count == 17)
            {
                GetButton((int)Buttons.SkipButton).gameObject.SetActive(false);
                if (GameObject.Find("UI_SelectGame"))
                {
                    Managers.UI.ClosePopupUI(GameObject.Find("UI_SelectGame").GetComponent<UI_SelectGame>());
                }
                StroyTyping();
            }
            else if (TM.count == 18)
            {
                StroyTyping();
            }
            else
            {
                ActiveObject();
            }
            return;
        }

        PanelChange();
    }
}