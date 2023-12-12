using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Manager
{
    public class SlaveManager : MonoBehaviour
    {
        private HashSet<SkullStateMachine> _skulls;
        public IReadOnlyCollection<SkullStateMachine> Skulls => _skulls;
        public int SlaveCount => _skulls.Count;

        public event Action<SkullStateMachine> skullCreated;

        private void Awake()
        {
            _skulls = new HashSet<SkullStateMachine>();
        }

        public void CreateSlave(Vector3 position = new Vector3())
        {
            GameObject slave = Resources.Load<GameObject>(Constants.Prefabs.Skull);
            SkullStateMachine sm = Instantiate(slave, position, Quaternion.identity)
                .GetComponentInChildren<SkullStateMachine>();
            if (sm != null)
            {
                sm.Init(this);
            }

            sm.StatController.Stats[StatType.Strength].AddValue(Managers.GameManager.attckPowerCoefficient);
            sm.StatController.Stats[StatType.Dexterity].AddValue(Managers.GameManager.attckPowerCoefficient);
            sm.StatController.Stats[StatType.Intelligence].AddValue(Managers.GameManager.attckPowerCoefficient);
            sm.StatController.Stats[StatType.MaxHP].AddValue(Managers.GameManager.skullMaxHpCoefficient);
            sm.StatController.Stats[StatType.MoveSpeed].AddValue(Managers.GameManager.skullMoveSpeedCoefficient);
            skullCreated?.Invoke(sm);
            _skulls.Add(sm);
        }

        public void NotifyStatChanged(StatType stat)
        {
            switch (stat)
            {
                case StatType.MaxHP:
                    float health = Managers.GameManager.skullMaxHpCoefficient;
                    foreach (SkullStateMachine skull in _skulls)
                    {
                        skull.StatController.Stats[stat].AddValue(health);
                        skull.HealthSystem.UpdateMaxHp();
                    }

                    break;
                case StatType.MoveSpeed:
                    float moveSpd = Managers.GameManager.skullMoveSpeedCoefficient;
                    foreach (SkullStateMachine skull in _skulls)
                    {
                        skull.StatController.Stats[stat].AddValue(moveSpd);
                    }

                    break;
                case StatType.Strength:
                    float str = Managers.GameManager.attckPowerCoefficient;
                    foreach (SkullStateMachine skull in _skulls)
                    {
                        skull.StatController.Stats[stat].AddValue(str);
                    }

                    break;
                case StatType.Dexterity:
                    float dex = Managers.GameManager.attckPowerCoefficient;
                    foreach (SkullStateMachine skull in _skulls)
                    {
                        skull.StatController.Stats[stat].AddValue(dex);
                    }

                    break;
                case StatType.Intelligence:
                    float _int = Managers.GameManager.attckPowerCoefficient;
                    foreach (SkullStateMachine skull in _skulls)
                    {
                        skull.StatController.Stats[stat].AddValue(_int);
                    }

                    break;
                case StatType.CriticalRate:
                    break;
                case StatType.ActionDelay:
                    break;
            }
        }
    }
}