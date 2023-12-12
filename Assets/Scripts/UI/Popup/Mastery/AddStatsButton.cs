using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class AddStatsButton : UI_Base
{
    private enum BindingText
    {
        PriceText
    }

    [HideInInspector] public StatType addType;
    private Button _button;
    private Text _priceText;
    public AddStatsDelegate AddStatsToUndead;

    public delegate void AddStatsDelegate(StatType type);


    public override bool Init()
    {
        BindText(typeof(BindingText));
        _priceText = GetText((int)BindingText.PriceText);
        _button = GetComponent<Button>();
        _button.onClick.AddListener(AddStats);
        _init = true;
        return true;
    }

    private void AddStats()
    {
        AddStatsToUndead?.Invoke(addType);
    }

    public void UpdatePriceText(int price)
    {     
        _priceText.text = price.ToString("N0");
    }
}