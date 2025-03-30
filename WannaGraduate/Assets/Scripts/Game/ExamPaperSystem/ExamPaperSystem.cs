using BBB.CSVData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamPaperSystem : MonoBehaviour
{
    List<ExamPaperData> paperData = new();

    public void CreatePapers(int count)
    {
        // 문제 데이터 베이스에서 랜덤하게 문제 가져오기
        List<EQuestionType> questionTypes = CSVDataContainer_WeekQuestionData.GetQuestionTypesForExamPaper(PlayerSaveDataModel.data.week);
        List<CSVDataRow_ExamQuestionData> questionData = CSVDataContainer_ExamQuestionData.GetRandomQuestionData(questionTypes);

        
    }



    


    private void CreatePaper()
    {

    }








}




public class ExamPaperData
{
    public int departmentIdx = 0;
    public int studentId = 0;
    public int studentNameIdx = 0;

    public List<ExamQuestionData> questionData = new();
}

public class ExamQuestionData
{
    public CSVDataRow_ExamQuestionData questionData;

    public int isCorrect; // 0:정답, 1:오답, 2:부분정답, 3:실격
    public int playerMarkedAs = -1; // 플레이어 정답 체크 여부

    public List<int> disturbanceIds = new(); // 방해 기믹 index
}