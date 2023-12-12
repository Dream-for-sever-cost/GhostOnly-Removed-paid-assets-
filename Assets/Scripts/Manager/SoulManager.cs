using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulManager
{
    [field: SerializeField] public int Soul { get; private set; } = 0;
    [field: SerializeField] public int EarnedSoul { get; private set; } = 0;
    public int EarnedSoulByDay = 0;
    public event Action<int, int> OnSoulChanged;

    public void Init()
    {
        Soul = 0;
        EarnedSoul = 0;
    }

    public bool CheckSoul(int amount)
    {
        return amount <= Soul;
    }

    public void GetSoul(int amount)
    {
        int currentSoul = Soul;
        Soul += amount;
        EarnedSoul += amount;
        EarnedSoulByDay += amount;
        OnSoulChanged?.Invoke(Soul, currentSoul);
    }

    public void UseSoul(int amount)
    {
        int currentSoul = Soul;
        if (CheckSoul(amount))
        {
            Soul -= amount;
        }

        OnSoulChanged?.Invoke(Soul, currentSoul);
    }
}