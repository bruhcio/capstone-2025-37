using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BBB
{
    public class RankingManager : MonoBehaviour
    {
        public static RankingManager Instance { get; private set; }
        public static int MyKillCountRank { get; private set; } = -1;

        public static int[] GetAllRanks()
        {
            return new int[]
            { 
                MyKillCountRank,
            };
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                DestroyImmediate(Instance);
                return;
            }
        }

        private void Start()
        {
            LoadMyRanking();
        }

        public void LoadMyRanking(UnityAction onLoadRanking = null)
        {
            MyKillCountRank = -1;

            NetworkingService.LoadRankingOne("kill_count", (json) =>
            {
                if (json.GetField("status").intValue == 200)
                {
                    MyKillCountRank = json.GetField("rank").intValue;
                }

                onLoadRanking?.Invoke();
            });
        }
    }
}

