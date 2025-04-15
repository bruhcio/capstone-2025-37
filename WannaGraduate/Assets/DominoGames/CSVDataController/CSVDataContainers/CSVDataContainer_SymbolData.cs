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

        public static CSVDataRow_SymbolData GetSymbolData(int idx)
        {
            return data.m_Items[idx];
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void InitData()
        {
            if(data != null)
            {
                return;
            }

            data = Resources.Load<CSVDataContainer_SymbolData>("CSVData/SymbolData");
        }
    }

    [System.Serializable]
    public class CSVDataRow_SymbolData
    {
        public int Id = -1;
        public string Name;
        public int Rarity;
        public int BaseRevenue;
        public SymbolEffectArea EffectArea = SymbolEffectArea.Center;
        public int Remain = -1; // 남은 횟수 (-1 = 제한 없음)
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
}

public enum EQuestionType
{
    OX,
    Choice,
    ShortAnswer,
}