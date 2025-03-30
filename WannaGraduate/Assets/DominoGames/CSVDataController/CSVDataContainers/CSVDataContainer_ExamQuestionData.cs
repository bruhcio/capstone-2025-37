using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace BBB.CSVData
{
#if UNITY_EDITOR
    public class CSVImport_ExamQuestionData : Editor, ICSVImportable
    {
        [MenuItem("Domino/CSV Serializer/ExamQuestionData")]
        public static void Init()
        {
            string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRsZA9hu26wMCpckrdqk8qqJWa91Ckdy3DJ7-6aRsk0n-P7CbonjWB60GHjIRn0TmBbI5jjj1hZKsr9/pub?output=csv";
            string assetfile = "Assets/Resources/CSVData/ExamQuestionData.asset";

            CSVImportManager.StartCorountine(CSVImportManager.DownloadAndImport<CSVDataContainer_ExamQuestionData>(url, assetfile));
        }
    }
#endif

    public class CSVDataContainer_ExamQuestionData : ScriptableObject
    {
        public CSVDataRow_ExamQuestionData[] m_Items;

        public static CSVDataContainer_ExamQuestionData data;

        public static Dictionary<EQuestionType, List<CSVDataRow_ExamQuestionData>> dataByQuestionType = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void InitData()
        {
            data = Resources.Load<CSVDataContainer_ExamQuestionData>("CSVData/ExamQuestionData");


            dataByQuestionType.Clear();
            for(int i = 0; i < data.m_Items.Length; i++)
            {
                if (dataByQuestionType[data.m_Items[i].QuestionType] == null)
                {
                    dataByQuestionType[data.m_Items[i].QuestionType] = new();
                }

                dataByQuestionType[data.m_Items[i].QuestionType].Add(data.m_Items[i]);
            }
        }


        public static List<CSVDataRow_ExamQuestionData> GetRandomQuestionData(List<EQuestionType> questionTypes)
        {
            List<CSVDataRow_ExamQuestionData> result = new();

            for(int i = 0; i < questionTypes.Count; i++)
            {
                int idx = Random.Range(0, dataByQuestionType[questionTypes[i]].Count);
                result.Add(dataByQuestionType[questionTypes[i]][idx]);
            }

            return result;
        }
    }

    [System.Serializable]
    public class CSVDataRow_ExamQuestionData
    {
        public EQuestionType QuestionType;
        public int QuestionId;
        public string Answers;
    }
}

public enum EQuestionType
{
    OX,
    Choice,
    ShortAnswer,
}