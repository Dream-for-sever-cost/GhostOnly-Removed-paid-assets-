using Data;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBookManager : MonoBehaviour
{
    //public bool IsInit { get; private set; }
    public string lastUnlockedSpell;
    public float effect;

    public Dictionary<string, SpellData> SpellDataDic { get; private set; } = new Dictionary<string, SpellData>();

    public void Init()
    {
        //if (IsInit)
        //    return;

        Managers.Data.InitSpellbook();
        SpellDataDic = Managers.Data.SpellDataDic;

        //IsInit = true;
    }
}
