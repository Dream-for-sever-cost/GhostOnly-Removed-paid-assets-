using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.SubItem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

public class UI_UnitStatus : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] private Image hpGage;
    [SerializeField] private RectTransform buffContainer;

    private Dictionary<BuffModel, Image> _buffUIMap;
    public HealthSystem healthSystem;
    public BuffSystem buffSystem;

    private void Awake()
    {
        healthSystem = GetComponentInParent<HealthSystem>();
        _buffUIMap = new Dictionary<BuffModel, Image>();

        Rect buffRect = buffContainer.rect;
        buffContainer.localPosition += new Vector3(buffRect.width * 0.5f, buffRect.height);
        buffContainer.localScale = new Vector3(2f, 2f);
    }

    private void OnEnable()
    {
        hpBar.enabled = true;
        hpGage.enabled = true;
        hpGage.fillAmount = 0f;

        if (healthSystem != null)
        {
            hpGage.fillAmount = healthSystem.MaxHealth <= 0 ? 0 : healthSystem.CurrentHealth / healthSystem.MaxHealth;
            healthSystem.OnGettingDamageEvent += ChangeHpBar;
            healthSystem.OnDecreaseHealthEvent += ChangeHpUI;
            healthSystem.OnHealEvent += ChangeHpBar;
            healthSystem.OnDeathEvent += ChangeHpUIOnDeath;
        }

        if (buffSystem != null)
        {
            buffSystem.AddBuffEvent += AddBuffUI;
            buffSystem.RemoveBuffEvent += RemoveBuffUI;
            buffSystem.BuffTimeElapsedEvent += UpdateBuffTimeUI;
            buffSystem.ClearBuffEvent += ClearBuffTimeUI;
            foreach (BuffSystem.BuffTimer buffSystemBuffTimer in buffSystem.BuffTimers)
            {
                AddBuffUI(buffSystemBuffTimer.Buff);
            }
        }
    }

    private void OnDisable()
    {
        if (healthSystem != null)
        {
            healthSystem.OnGettingDamageEvent -= ChangeHpBar;
            healthSystem.OnDecreaseHealthEvent -= ChangeHpUI;
            healthSystem.OnHealEvent -= ChangeHpBar;
            healthSystem.OnDeathEvent -= ChangeHpUIOnDeath;
        }

        if (buffSystem != null)
        {
            buffSystem.AddBuffEvent -= AddBuffUI;
            buffSystem.RemoveBuffEvent -= RemoveBuffUI;
            buffSystem.BuffTimeElapsedEvent -= UpdateBuffTimeUI;
            buffSystem.ClearBuffEvent -= ClearBuffTimeUI;
            ClearBuffTimeUI();
        }
    }

    private void ChangeHpBar(float heal)
    {
        ChangeHpUI();
    }

    private void ChangeHpUI()
    {
        float ratio = healthSystem.CurrentHealth / healthSystem.MaxHealth;
        hpGage.DOFillAmount(ratio, Constants.Time.AnimationTime);
    }

    private void ChangeHpUIOnDeath()
    {
        hpBar.enabled = false;
        hpGage.enabled = false;
    }

    private void AddBuffUI(BuffModel buff)
    {
        //todo object pooling
        if (_buffUIMap.ContainsKey(buff))
        {
            return;
        }

        GameObject buffUI = new GameObject();
        Image buffImage = buffUI.AddComponent<Image>();
        buffImage.type = Image.Type.Filled;
        buffImage.fillMethod = Image.FillMethod.Radial360;
        buffImage.transform.SetParent(buffContainer, false);
        Sprite buffSprite = BuffToSprite(buff.Type);

        if (buffSprite != null)
        {
            buffImage.sprite = buffSprite;
        }
        else
        {
            Debug.LogAssertion($"Failed to load sprite : {buff.Type}");
        }

        _buffUIMap.Add(buff, buffImage);
    }

    private void RemoveBuffUI(BuffModel buff)
    {
        if (!_buffUIMap.ContainsKey(buff)) { return; }

        Image buffUI = _buffUIMap[buff];
        if (buffUI != null)
        {
            Destroy(buffUI.gameObject);
        }

        _buffUIMap.Remove(buff);
    }

    private Sprite BuffToSprite(BuffModel.BuffType buffType)
    {
        //TODO : [이미지를 아이콘으로 변경] 현재 버프 아이콘이 없음
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/UI/Icon");
        if (sprites.Length <= (int)buffType)
        {
            Debug.LogError("buffType mapping failed ");
            return null;
        }

        return buffType == BuffModel.BuffType.None ? null : sprites[(int)buffType];
    }

    public void SetBuffSystem(BuffSystem buff)
    {
        buffSystem = buff;
        buffSystem.AddBuffEvent += AddBuffUI;
        buffSystem.RemoveBuffEvent += RemoveBuffUI;
        buffSystem.BuffTimeElapsedEvent += UpdateBuffTimeUI;
        buffSystem.ClearBuffEvent += ClearBuffTimeUI;
    }

    private void UpdateBuffTimeUI(BuffModel buff, float elapsedTime)
    {
        if (_buffUIMap.TryGetValue(buff, out Image buffImage))
        {
            float restTime = buff.LastingTime - elapsedTime;


            if (restTime <= Constants.Time.BuffFlickeringTime )
            {
                Destroy(buffImage.gameObject);
                _buffUIMap.Remove(buff);
            }
            else
            {
                float restTimeRatio = restTime / buff.LastingTime;
                buffImage.fillAmount = restTimeRatio;
            }
        }
    }

    private void ClearBuffTimeUI()
    {
        foreach ((BuffModel _, Image ui) in _buffUIMap)
        {
            //todo optimize to use object pool
            Destroy(ui.gameObject);
        }

        _buffUIMap.Clear();
    }
}