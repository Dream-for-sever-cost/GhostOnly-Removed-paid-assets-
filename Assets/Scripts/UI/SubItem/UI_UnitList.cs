using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class UI_UnitList : UI_Base
{
    private GridLayoutGroup _itemContainer;
    private List<SkullStateMachine> _skulls;
    private List<UI_UnitItem> _items;

    public override bool Init()
    {
        if (_init) { return true; }

        _items = new List<UI_UnitItem>();
        _skulls = new List<SkullStateMachine>();
        _itemContainer = GetComponent<GridLayoutGroup>();
        _items.AddRange(_itemContainer.GetComponentsInChildren<UI_UnitItem>());
        _skulls.AddRange(Managers.SlaveManager.Skulls);
        Managers.SlaveManager.skullCreated += ShowSkullUI;
        UpdateUI();
        _init = true;
        return true;
    }

    private void ShowSkullUI(SkullStateMachine sm)
    {
        _skulls.Add(sm);
        int lastIndex = _skulls.LastIndex();
        _items[lastIndex].SetSkull(sm);
    }

    private void UpdateUI()
    {
        for (int index = 0; index < _items.Count; index++)
        {
            UI_UnitItem unitItem = _items[index];
            SkullStateMachine skull = index > _skulls.LastIndex() ? null : _skulls[index];
            unitItem.SetSkull(skull);
        }
    }

    private void OnDestroy()
    {
        Managers.SlaveManager.skullCreated -= ShowSkullUI;
    }
}