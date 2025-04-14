using BBB.DataStructure;
using DominoGames.Core.EventSystem;
using Mono.CSharp.Linq;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DominoGames.RPG
{
    // Latest updated on 2025.04.08.
    // Author JYS & CHS
    // Version 1.1.0

    public class StatSystem : SerializedMonoBehaviour
    {
        public bool InitOnAwake = true;
        public bool isGlobal = false;

        [ShowIf("isGlobal")]
        public string globalKey;

        public Dictionary<EStatTypes, float> originStatValues = new();
        public static Dictionary<string, StatSystem> globalStats = new();

        private Dictionary<EStatTypes, List<System.Action>> onStatChangedEvents = new();

        [Button]
        public void InitOriginStatValues()
        {
            Dictionary<EStatTypes, float> newDictionary = new();

            var stats = Enum.GetValues(typeof(EStatTypes));
            for (int i = 0; i < stats.Length; i++)
            {
                newDictionary.Add((EStatTypes)stats.GetValue(i), 1f);
            }


            var keyList = new List<EStatTypes>(newDictionary.Keys);
            foreach (var key in keyList)
            {
                if (originStatValues.ContainsKey(key))
                {
                    newDictionary[key] = originStatValues[key];
                }
            }

            originStatValues = newDictionary;
        }


        private void Awake()
        {
            if (InitOnAwake)
            {
                Init();
            }
        }

        public void Init()
        {
            ClearOnStatChangedEvents();
            ClearBuffsAll();
            ApplyGlobalStat();
        }

        private void ApplyGlobalStat()
        {
            if (isGlobal)
            {
                globalStats[globalKey] = this;
            }
        }
        public void ClearOnStatChangedEvents()
        {
            this.onStatChangedEvents.Clear();
        }
        public void ClearOnStatChangedEvents(EStatTypes statType)
        {
            if (this.onStatChangedEvents.ContainsKey(statType))
            {
                this.onStatChangedEvents[statType].Clear();
            }
        }
        public void AddOnStatChangedEvent(System.Action action)
        {
            foreach (EStatTypes statType in Enum.GetValues(typeof(EStatTypes)))
            {
                AddOnStatChangedEvent(statType, action);
            }
        }
        public void AddOnStatChangedEvent(EStatTypes statType, System.Action action)
        {
            if (!this.onStatChangedEvents.ContainsKey(statType))
            {
                this.onStatChangedEvents[statType] = new();
            }

            this.onStatChangedEvents[statType].Add(action);
        }


        private List<DoubleDictionary<EStatTypes, float>> statBuffs = new()
        {
            new(),
            new(),
            new()
        };

        public int AddBuff(EStatTypes statType, EStatCalcTypes calcType, float value)
        {
            int statId = statBuffs[(int)calcType].AddItem(statType, value);

            // Id Ranges
            // Percent = -10억 ~
            // Multiply = 0~
            // Constant = +10억 ~
            statId += (int)calcType * 1000000000 - 1000000000;

            if (onStatChangedEvents.ContainsKey(statType))
            {
                onStatChangedEvents[statType].ForEach(x => x?.Invoke());
            }

            return statId;
        }

        public void RemoveBuff(EStatTypes statType, int buffId)
        {
            for (int i = 0; i < statBuffs.Count; i++)
            {
                statBuffs[i].RemoveItem(statType, buffId);
            }
        }

        public float GetBuffValue(EStatTypes statType)
        {
            float result = originStatValues[statType];

            // constant 연산
            var list = statBuffs[0].GetValues(statType);

            for (int i = 0; i < list.Count; i++)
            {
                result += list[i];
            }

            // percent(%p) 연산
            list = statBuffs[1].GetValues(statType);
            float percent = 1f;
            for (int i = 0; i < list.Count; i++)
            {
                percent += list[i] * 0.01f;
            }
            result *= percent;

            // multiply(%) 연산
            list = statBuffs[2].GetValues(statType);
            for (int i = 0; i < list.Count; i++)
            {
                result *= 1 + list[i] * 0.01f;
            }
            return result;
        }
        public void ClearBuffsAll()
        {
            for (int i = 0; i < statBuffs.Count; i++)
            {
                statBuffs[i].RemoveAllItems();
            }
        }
    }

    public enum EStatTypes
    {
        EarnRP,                     // 연구 포인트 획득량 증가
        HighRarityAppearance,       // (일반 제외) 고급, 희귀, 전설 급 심볼 출현 확률 증가
    }

    public enum EStatCalcTypes
    {
        Constant,
        Percent,
        Multiply,
    }
}