using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RNGNeeds;
using DominoGames.RPG;
using System;

namespace BBB.CSVData
{
#if UNITY_EDITOR
    public class CSVImport_SymbolData : Editor, ICSVImportable
    {
        [MenuItem("Domino/CSV Serializer/SymbolData")]
        public static void Init()
        {
            string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRsZA9hu26wMCpckrdqk8qqJWa91Ckdy3DJ7-6aRsk0n-P7CbonjWB60GHjIRn0TmBbI5jjj1hZKsr9/pub?output=csv";
            string assetfile = "Assets/Resources/CSVData/SymbolData.asset";

            CSVImportManager.StartCorountine(CSVImportManager.DownloadAndImport<CSVDataContainer_SymbolData>(url, assetfile));
        }
    }
#endif

    public class CSVDataContainer_SymbolData : ScriptableObject
    {
        public CSVDataRow_SymbolData[] m_Items;

        public static CSVDataContainer_SymbolData data;
        private static Dictionary<int, CSVDataRow_SymbolData> itemsById = new();
        private static Dictionary<int, List<CSVDataRow_SymbolData>> itemsByRank = new();

        public static CSVDataRow_SymbolData GetSymbolData(int idx)
        {
            return itemsById[idx];
        }

        public static CSVDataRow_SymbolData GetRandomSymbolDataByRarity(int rarity)
        {
            int randIndex = UnityEngine.Random.Range(0, itemsByRank[rarity].Count);
            return itemsByRank[rarity][randIndex];
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void InitData()
        {
            if (data != null)
            {
                return;
            }

            data = Resources.Load<CSVDataContainer_SymbolData>("CSVData/SymbolData");

            itemsByRank.Clear();
            for (int i = 0; i < data.m_Items.Length; i++)
            {
                if (!itemsByRank.ContainsKey(data.m_Items[i].Rarity))
                {
                    itemsByRank.Add(data.m_Items[i].Rarity, new());
                }

                itemsByRank[data.m_Items[i].Rarity].Add(data.m_Items[i]);
                itemsById.Add(data.m_Items[i].Id, data.m_Items[i]);
            }
        }
    }

    [System.Serializable]
    public class CSVDataRow_SymbolData
    {
        public int InstanceId = -1;
        public int Id = -1;
        public string Name;
        public int Rarity;
        public int BaseRevenue;
        public int Remain = -1; // 남은 횟수 (-1 = 제한 없음)
        public string Trigger;
        public RelativeSymbolEffectArea RelativeArea;
        //public AbsoluteSymbolEffectArea AbsoluteArea;
        public int[] TargetSymbols;

        public string SpecialEffect;
        public string[] SpecialEffectParams;
        public int EffectPriority;

        public int OnDestroyRP;
        public string EvolvingTarget;
        public int EvolvingDuration;
    }


    [Flags]
    public enum SymbolEffectArea
    {
        None = 0,

        // 3x3 그리드의 각 칸을 순서대로 정의 (왼쪽 위부터 오른쪽 아래까지)
        Cell0 = 1 << 0,   // 1
        Cell1 = 1 << 1,   // 2
        Cell2 = 1 << 2,   // 4
        Cell3 = 1 << 3,   // 8
        Cell4 = 1 << 4,   // 16
        Cell5 = 1 << 5,   // 32
        Cell6 = 1 << 6,   // 64
        Cell7 = 1 << 7,   // 128
        Cell8 = 1 << 8,   // 256

        // 중앙
        Center = Cell4,

        // + 모양
        PlusShape = Cell1 | Cell3 | Cell4 | Cell5 | Cell7,

        // X 모양
        XShape = Cell0 | Cell2 | Cell4 | Cell6 | Cell8,

        // 모든 칸
        All = Cell0 | Cell1 | Cell2 | Cell3 | Cell4 | Cell5 | Cell6 | Cell7 | Cell8
    }

    [Flags]
    public enum AbsoluteSymbolEffectArea
    {
        None = 0,

        // Row 0 (y = 0)
        Cell00 = 1 << 0,   // 1
        Cell01 = 1 << 1,   // 2
        Cell02 = 1 << 2,   // 4
        Cell03 = 1 << 3,   // 8
        Cell04 = 1 << 4,   // 16

        // Row 1 (y = 1)
        Cell10 = 1 << 5,   // 32
        Cell11 = 1 << 6,   // 64
        Cell12 = 1 << 7,   // 128
        Cell13 = 1 << 8,   // 256
        Cell14 = 1 << 9,   // 512

        // Row 2 (y = 2)
        Cell20 = 1 << 10,  // 1024
        Cell21 = 1 << 11,  // 2048
        Cell22 = 1 << 12,  // 4096
        Cell23 = 1 << 13,  // 8192
        Cell24 = 1 << 14,  // 16384

        // Row 3 (y = 3)
        Cell30 = 1 << 15,  // 32768
        Cell31 = 1 << 16,  // 65536
        Cell32 = 1 << 17,  // 131072
        Cell33 = 1 << 18,  // 262144
        Cell34 = 1 << 19,  // 524288

        // 전체 영역: 모든 셀을 포함
        All = Cell00 | Cell01 | Cell02 | Cell03 | Cell04 |
              Cell10 | Cell11 | Cell12 | Cell13 | Cell14 |
              Cell20 | Cell21 | Cell22 | Cell23 | Cell24 |
              Cell30 | Cell31 | Cell32 | Cell33 | Cell34
    }


    [Flags]
    public enum RelativeSymbolEffectArea
    {
        None = 0,               // 기본값
        Self = 1 << 0,          // 1 << 0 = 1
        Top = 1 << 1,           // 1 << 1 = 2
        Bottom = 1 << 2,        // 1 << 2 = 4
        Left = 1 << 3,          // 1 << 3 = 8
        Right = 1 << 4,         // 1 << 4 = 16
        TopLeft = 1 << 5,       // 1 << 5 = 32
        TopRight = 1 << 6,      // 1 << 6 = 64
        BottomLeft = 1 << 7,    // 1 << 7 = 128
        BottomRight = 1 << 8,   // 1 << 8 = 256

        // 복합 플래그 (기존 플래그 조합)
        Surrounding3x3 = Top | Bottom | Left | Right
                       | TopLeft | TopRight | BottomLeft | BottomRight,
        // (2 | 4 | 8 | 16 | 32 | 64 | 128 | 256) = 510

        Diagonals = TopLeft | TopRight | BottomLeft | BottomRight,
        // (32 | 64 | 128 | 256) = 480

        NonDiagonals = Top | Bottom | Left | Right,
        // (2 | 4 | 8 | 16) = 30

        SameRow = 1 << 9,                        // 1 << 9 = 512
        SameColumn = 1 << 10                        // 1 << 10 = 1024
    }

}

public enum EQuestionType
{
    OX,
    Choice,
    ShortAnswer,
}