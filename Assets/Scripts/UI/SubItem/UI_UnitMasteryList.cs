using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SubItem
{
    public class UI_UnitMastery : UI_Base
    {
        private List<Mastery> _masteries;

        private GridLayoutGroup _container;
        private List<Image> _images;

        private enum Grids
        {
            MasteryContainer,
        }

        public override bool Init()
        {
            _masteries = new List<Mastery>();
            _images = new List<Image>();
            Bind<GridLayoutGroup>(typeof(Grids));
            _container = Get<GridLayoutGroup>((int)Grids.MasteryContainer);
            _init = true;
            return true;
        }

        public void SetMasteryArray(params Mastery[] masteryArray)
        {
            _masteries.Clear();
            _masteries.AddRange(masteryArray);
            UpdateUI();
        }

        private void UpdateUI()
        {
            foreach (Image image in _images)
            {
                Destroy(image.gameObject);
            }

            _images.Clear();

            foreach (Mastery mastery in _masteries)
            {
                if (mastery == Mastery.None) { continue; }

                GameObject go = new GameObject();
                Image image = go.AddComponent<Image>();
                _images.Add(image);
                image.type = Image.Type.Filled;
                image.fillMethod = Image.FillMethod.Radial360;
                image.transform.SetParent(_container.transform, false);

                Sprite sprite = Resources.LoadAll<Sprite>("Sprites/UI/Mastery")[(int)mastery.IconName];
                image.sprite = sprite;
            }
        }
    }
}