using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace BBB.CSVData
{
#if UNITY_EDITOR
    public class CSVImport_WeekQuestionData : Editor, ICSVImportable
    {
        [MenuItem("Domino/CSV Serializer/WeekQuestionData")]
        public static void Init()
        {
            string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRsZA9hu26wMCpckrdqk8qqJWa91Ckdy3DJ7-6aRsk0n-P7CbonjWB60GHjIRn0TmBbI5jjj1hZKsr9/pub?output=csv";
            string assetfile = "Assets/Resources/CSVData/WeekQuestionData.asset";

            CSVImportManager.StartCorountine(CSVImportManager.DownloadAndImport<CSVDataContainer_WeekQuestionData>(url, assetfile));
        }
    }
#endif

    public class CSVDataContainer_WeekQuestionData : ScriptableObject
    {
        public CSVDataRow_WeekQuestionData[] m_Items;

        public static CSVDataContainer_WeekQuestionData data;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void InitData()
        {
            data = Resources.Load<CSVDataContainer_WeekQuestionData>("CSVData/WeekQuestionData");
        }


        public static List<EQuestionType> GetQuestionTypesForExamPaper(int week)
        {
            return data.m_Items[week].QuestionTypes.Split(", ").Select(s => System.Enum.Parse<EQuestionType>(s)).ToList();
        }
    }

    [System.Serializable]
    public class CSVDataRow_WeekQuestionData
    {
        public int Week;
        public string QuestionTypes;
    }
}