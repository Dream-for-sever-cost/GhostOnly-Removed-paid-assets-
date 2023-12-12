using System;
using UnityEngine;
using Text = TMPro.TextMeshProUGUI;

namespace UI.SubItem
{
    public class UI_DamageText : PoolAble
    {
        //todo pooling
        private float _timeToVisible = 1f;
        private Text _damageText;
        private bool _isReady = false;
        private float _timeSinceStarted = 0f;
        private float _damage;
        private Color _color;
        public Transform damageTransform;

        private void Awake()
        {
            _damageText = GetComponentInChildren<Text>();
        }

        public void SetDamage(int damage, Color color)
        {
            _isReady = true;
            _damage = damage;
            _color = color;
            _timeSinceStarted = 0f;
        }

        private void Update()
        {
            if (!_isReady) { return; }

            _damageText.text = _damage.ToString("N0");
            _damageText.color = _color;

            _timeSinceStarted += Time.deltaTime;
            if (_timeSinceStarted >= _timeToVisible)
            {
                _isReady = false;
                ReleaseObject();
            }
        }

        public static UI_DamageText ShowDamageText(float damage, Color color, Vector3 pos)
        {
            UI_DamageText damageText =
                ObjectPoolManager.Instance.GetGo(PoolType.DamageText).GetComponent<UI_DamageText>();
            float randomPos = UnityEngine.Random.Range(-0.2f, 0.2f);
            damageText.transform.position = new Vector3(pos.x + randomPos, pos.y + randomPos, pos.z);
            damageText.SetDamage(damage.ToInt(), color);
            return damageText;
        }
    }
}