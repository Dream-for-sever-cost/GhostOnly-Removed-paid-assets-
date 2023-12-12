using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class Gravestone : MonoBehaviour
{
    [field: SerializeField] public int InitialCount { get; private set; } = 3;
    private int nowCount;

    [field: SerializeField] public float SpawnDelay { get; private set; } = 5f;
    private WaitForSeconds waitSpawnDelay;
    private UI_StarCatch starCatchUI;
    private void Start()
    {
        Initialize();
        waitSpawnDelay = new WaitForSeconds(SpawnDelay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Lamping b))
        {        
            if (b.IsPlayer)
            {
                starCatchUI = Managers.UI.ShowPopupUI<UI_StarCatch>();
                
                starCatchUI.Initialized(b.IsSkill? 3 : Random.Range(0, 3));                     
            }
            else
            {
                Managers.Sound.PlaySound(Data.SoundType.GravestoneHit, transform.position, true);
                Managers.Soul.GetSoul(Random.Range(1, Constants.Starcatch.AutoMaxReward));
            }

            ReduceCount();
        }
    }

    private void Initialize()
    {
        Managers.Target.AddEnemy(transform);
        nowCount = InitialCount;
        gameObject.SetActive(true);
    }

    private void ReduceCount()
    {
        nowCount -= 1;

        if (nowCount <= 0)
        {
            Managers.Target.RemoveEnemy(transform);
            gameObject.SetActive(false);
            Invoke(nameof(Initialize), SpawnDelay - Managers.GameManager.gravestoneRespawnTimeCoefficient);
        }
    }
}