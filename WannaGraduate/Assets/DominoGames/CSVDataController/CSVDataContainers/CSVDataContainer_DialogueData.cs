using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace BBB.CSVData
{
#if UNITY_EDITOR
    public class CSVImport_DialogueData : Editor, ICSVImportable
    {
        [MenuItem("Domino/CSV Serializer/DialogueData")]
        public static void Init()
        {
            string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRsZA9hu26wMCpckrdqk8qqJWa91Ckdy3DJ7-6aRsk0n-P7CbonjWB60GHjIRn0TmBbI5jjj1hZKsr9/pub?gid=1833105453&single=true&output=csv";
            string assetfile = "Assets/Resources/CSVData/DialogueData.asset";

            CSVImportManager.StartCorountine(CSVImportManager.DownloadAndImport<CSVDataContainer_DialogueData>(url, assetfile));
        }
    }
#endif

    public class CSVDataContainer_DialogueData : ScriptableObject
    {
        public CSVDataRow_DialogueData[] m_Items;

        public static Dictionary<string, List<CSVDataRow_DialogueData>> dialogueDataById = new();
        public static CSVDataContainer_DialogueData data;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void InitData()
        {
            data = Resources.Load<CSVDataContainer_DialogueData>("CSVData/DialogueData");

            // init dialogueDataById
            dialogueDataById.Clear();
            for(int i = 0; i < data.m_Items.Length; i++)
            {
                if (!dialogueDataById.ContainsKey(data.m_Items[i].DialogueId))
                {
                    dialogueDataById.Add(data.m_Items[i].DialogueId, new());
                }
                dialogueDataById[data.m_Items[i].DialogueId].Add(data.m_Items[i]);
            }
        }


    }

    [System.Serializable]
    public class CSVDataRow_DialogueData
    {
        public string DialogueId, CharacterId;
        public int Index;
        public string Korean, English;
    }
}
