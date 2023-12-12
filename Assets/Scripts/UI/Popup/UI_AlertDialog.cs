using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

namespace UI.Popup
{
    public class UI_AlertDialog : UI_Popup
    {
        private enum GameObjects
        {
            Background,
        }

        private enum Texts
        {
            TitleText,
            MessageText,
            PositiveText,
            NegativeText,
        }

        private enum Buttons
        {
            PositiveButton,
            NegativeButton,
        }

        private Text _titleText;
        private Text _messageText;
        private Text _positiveText;
        private Text _negativeText;

        private Button _positiveButton;
        private Button _negativeButton;

        private Action _onPositive;
        private Action _onNegative;

        private Sequence _openSequence;

        public override bool Init()
        {
            if (_init) { return true; }

            _init = true;
            BindText(typeof(Texts));
            BindButton(typeof(Buttons));
            BindObject(typeof(GameObjects));

            _titleText = GetText((int)Texts.TitleText);
            _messageText = GetText((int)Texts.MessageText);
            _positiveText = GetText((int)Texts.PositiveText);
            _negativeText = GetText((int)Texts.NegativeText);

            _positiveButton = GetButton((int)Buttons.PositiveButton);
            _negativeButton = GetButton((int)Buttons.NegativeButton);

            _onNegative = ClosePopup;
            _positiveButton.BindEvent(() => _onPositive?.Invoke());
            _negativeButton.BindEvent(() => _onNegative?.Invoke());

            OpenSequence();

            return true;
        }

        //todo builder 패턴 적용하면 좋음
        public void Alert(
            string titleId = null,
            string messageId = null,
            string positiveTextId = null,
            string negativeTextId = null,
            Action onPositive = null,
            Action onNegative = null)
        {
            Init();

            _titleText.gameObject.SetActive(titleId != null);
            _messageText.gameObject.SetActive(messageId != null);
            _positiveButton.gameObject.SetActive(positiveTextId != null);
            _negativeButton.gameObject.SetActive(negativeTextId != null);

            if (titleId != null)
            {
                _titleText.text = GetString(titleId);
            }

            if (messageId != null)
            {
                _messageText.text = GetString(messageId);
            }

            if (positiveTextId != null)
            {
                _positiveText.text = GetString(positiveTextId);
            }

            if (negativeTextId != null)
            {
                _negativeText.text = GetString(negativeTextId);
            }

            _onPositive = onPositive;
            if (onNegative != null)
            {
                _onNegative = onNegative;
            }
        }

        private void ClosePopup()
        {
            Managers.Sound.PlaySound(Data.SoundType.Click);
            ClosePopupUI();
        }

        private void OpenSequence()
        {
            GameObject bg = GetObject((int)GameObjects.Background);

            _openSequence = DOTween.Sequence()
            .OnStart(() =>
            {
                bg.transform.localScale = Vector3.zero;
            })
            .Append(bg.transform.DOScale(1f, Util.Constants.DOTween.OpenPopupDuration))
            .SetEase(Ease.Linear)
            .SetUpdate(true);
        }
    }
}