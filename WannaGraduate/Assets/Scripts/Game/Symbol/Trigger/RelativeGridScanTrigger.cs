using BBB.CSVData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RelativeGridScanTrigger : ITrigger
{
    public bool Evaluate(TriggerParameter context, ref List<int> foundCalendarIds)
    {
        if (context == null)
            return false;

        int currentIndex = context.CalendarIndex; // 현재 심볼의 그리드 위치
        List<int> offsets = GetOffsetsForRelativeArea(context.RelativeArea); // 설정된 상대 영역에 해당하는 오프셋

        if (PlayerSaveDataModel.data.calendarSIds == null || PlayerSaveDataModel.data.calendarSIds.Count == 0)
        {
            Debug.LogWarning("Emtpy list: calenderSymbols");
            return false;
        }

        // (1) 기본 오프셋 기반 검사 : 현재 셀을 기준으로 주변(상,하,좌,우,대각선)
        foreach (int offset in offsets)
        {
            int checkCalendarIndex = currentIndex + offset;

            if(checkCalendarIndex >= 20 || checkCalendarIndex < 0)
            {
                continue;
            }

            if (context.TargetSymbols.Contains(PlayerSaveDataModel.data.GetSymbolIdFromCalendar(checkCalendarIndex)))
            {
                foundCalendarIds.Add(checkCalendarIndex);
            }
        }

        // (2) SameRow 검사: ExtraData에 SameRow 플래그가 있으면, 현재 행 전체를 검사
        if (context.RelativeArea.HasFlag(RelativeSymbolEffectArea.SameRow))
        {
            int checkCalendarIndex = context.CalendarIndex / 5 * 5;

            for(int i = 0; i < 5; i++)
            {
                if (context.TargetSymbols.Contains(PlayerSaveDataModel.data.GetSymbolIdFromCalendar(checkCalendarIndex + i)))
                {
                    foundCalendarIds.Add(checkCalendarIndex + i);
                }
            }
        }

        // (3) SameColumn 검사: ExtraData에 SameColumn 플래그가 있으면, 현재 열 전체를 검사
        if (context.RelativeArea.HasFlag(RelativeSymbolEffectArea.SameColumn))
        {
            int checkCalendarIndex = context.CalendarIndex % 5;
            for (int i = 0; i < 4; i++)
            {
                if (context.TargetSymbols.Contains(PlayerSaveDataModel.data.GetSymbolIdFromCalendar(checkCalendarIndex + i * 5)))
                {
                    foundCalendarIds.Add(checkCalendarIndex + i * 5);
                }
            }
        }

        foundCalendarIds = foundCalendarIds.Distinct().ToList();

        if (foundCalendarIds.Count > 0)
            return true;
        else
            return false;
    }

    private List<int> GetOffsetsForRelativeArea(RelativeSymbolEffectArea area)
    {
        List<int> offsetIndexes = new();

        if (area.HasFlag(RelativeSymbolEffectArea.Self))
            offsetIndexes.Add(0);
        if (area.HasFlag(RelativeSymbolEffectArea.Top))
            offsetIndexes.Add(-5);
        if (area.HasFlag(RelativeSymbolEffectArea.Bottom))
            offsetIndexes.Add(+5);
        if (area.HasFlag(RelativeSymbolEffectArea.Left))
            offsetIndexes.Add(-1);
        if (area.HasFlag(RelativeSymbolEffectArea.Right))
            offsetIndexes.Add(+1);
        if (area.HasFlag(RelativeSymbolEffectArea.TopLeft))
            offsetIndexes.Add(-6);
        if (area.HasFlag(RelativeSymbolEffectArea.TopRight))
            offsetIndexes.Add(-4);
        if (area.HasFlag(RelativeSymbolEffectArea.BottomLeft))
            offsetIndexes.Add(+4);
        if (area.HasFlag(RelativeSymbolEffectArea.BottomRight))
            offsetIndexes.Add(+6);

        // SameRow와 SameColumn은 별도 로직(예, 전체 행 또는 열에 대해 검사)으로 처리 가능하므로 생략
        return offsetIndexes;
    }
}