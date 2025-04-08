using BBB.CSVData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarView : UI_Base
{
    [UIAutoAttachField, SerializeField] List<CalendarDailyView> dayObjects;

    // 캘린더 초기화
    public void ClearCalendar()
    {
        for (int i = 0; i < dayObjects.Count; i++)
        {
            dayObjects[i].ClearSymbol();
        }
    }

    // 일정 계획 스핀
    public void RollCalendar()
    {
        ClearCalendar();
        StartCoroutine(RollCalendarDirection());
    }

    // 기본 수익 획득
    public void EarnBaseRevenue()
    {

    }

    // 특수 효과 재생
    public void PlaySpecialEffect()
    {

    }














    // 일정 스핀 연출
    IEnumerator RollCalendarDirection()
    {
        // Update day view
        for (int i = 0; i < dayObjects.Count; i++)
        {
            dayObjects[i].RollSymbol(PlayerSaveDataModel.data.calendar[i], OnCalendarDailyDirectionEnd);
            yield return new WaitForSeconds(0.02f);
        }
    }

    int dailyDirectionCount = 0;
    private void OnCalendarDailyDirectionEnd()
    {
        dailyDirectionCount++;

        if(dailyDirectionCount >= 20)
        {
            
        }
    }

    // 기본 수익 정산 연출
    IEnumerator CalcBaseRevenuesDirection()
    {
        for(int i = 0; i < dayObjects.Count; i++)
        {

        }
    }
}
