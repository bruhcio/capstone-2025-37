using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RNGNeeds;
using DominoGames.RPG;

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
        static void InitData()
        {
            data = Resources.Load<CSVDataContainer_SymbolData>("CSVData/SymbolData");
        }
    }

    [System.Serializable]
    public class CSVDataRow_SymbolData
    {
        public int ItemId;
        public int Rarity;
        public int BaseRevenue;
    }
}

public enum EQuestionType
{
    OX,
    Choice,
    ShortAnswer,
}