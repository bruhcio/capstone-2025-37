using UnityEngine;
using UnityEditor;

namespace BBB.CSVData
{
#if UNITY_EDITOR
    public class CSVImport_ItemData : Editor, ICSVImportable
    {
        [MenuItem("Domino/CSV Serializer/ItemData")]
        public static void Init()
        {
            string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRsZA9hu26wMCpckrdqk8qqJWa91Ckdy3DJ7-6aRsk0n-P7CbonjWB60GHjIRn0TmBbI5jjj1hZKsr9/pub?gid=1974360237&single=true&output=csv";
            string assetfile = "Assets/Resources/CSVData/ItemData.asset";

            CSVImportManager.StartCorountine(CSVImportManager.DownloadAndImport<CSVDataContainer_ItemData>(url, assetfile));
        }
    }
#endif

    public class CSVDataContainer_ItemData : ScriptableObject
    {
        public CSVDataRow_ItemData[] m_Items;

        public static CSVDataContainer_ItemData data;

        public static CSVDataRow_ItemData GetItemData(int idx)
        {
            return data.m_Items[idx];
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void InitData()
        {
            data = Resources.Load<CSVDataContainer_ItemData>("CSVData/ItemData");
        }
    }

    // 아이템 발동 시점
    public enum ItemType
    {
        AlwaysOn,
        TurnEnd
    }

    [System.Serializable]
    public class CSVDataRow_ItemData
    {
        public int Id;
        public string Name;
        public int Rarity;
        public int Appearance;
        public ItemType Type;
    }
}
