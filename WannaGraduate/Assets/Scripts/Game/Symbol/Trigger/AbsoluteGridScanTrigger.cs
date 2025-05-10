using BBB.CSVData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PlayerSaveDataModel;

public class AbsoluteGridScanTrigger : ITrigger
{
    // 특정 심볼이 존재하는지 여부를 검사하는 트리거입니다
    public bool Evaluate(TriggerParameter context, ref List<int> foundCalendarIds)
    {
        if (context == null)
            return false;

        for(int i=0; i < PlayerSaveDataModel.data.calendarSIds.Count; i++)
        {
            if (context.TargetSymbols.Contains(PlayerSaveDataModel.data.GetSymbolIdFromCalendar(i)))
            {
                foundCalendarIds.Add(i);
            }
        }

        //// 그리드 데이터
        //var calenderSymbols = PlayerSaveDataModel.data.calenderSymbols;
        //if (calenderSymbols == null || calenderSymbols.Count != 20)
        //{
        //    Debug.LogWarning("캘린더 심볼 데이터가 비어있습니다.");
        //    return false;
        //}

        //// 사전(Dictionary)를 이용해 각 GridIndex로 CalendarSymbol을 빠르게 조회할 수 있도록 함
        //// >> Dictionary의 Hash 연산이 O(1)이지만 상수 값이 커서, List를 이용해 저장하고 조회하는 것이 더 빠릅니다. (JYS 2025.05.09)
        //Dictionary<Vector2Int, CalendarSymbol> gridDictionary = new Dictionary<Vector2Int, CalendarSymbol>();
        //foreach (var symbol in calenderSymbols)
        //{
        //    gridDictionary[symbol.GridIndex] = symbol;
        //}

        //// 0부터 19까지 각 셀을 검사합니다.
        //for (int i = 0; i < 20; i++)
        //{
        //    // i번째 셀에 대응하는 플래그 생성 (예: i=0이면 1<<0, i=1이면 1<<1, ...)
        //    AbsoluteSymbolEffectArea cellFlag = (AbsoluteSymbolEffectArea)(1 << i);
        //    if (context.AbsoluteArea.HasFlag(cellFlag))
        //    {
        //        // 셀 인덱스 i를 기반으로 행과 열 계산 (4행 x 5열)
        //        int row = i / 5;
        //        int col = i % 5;
        //        Vector2Int targetCoordinate = new Vector2Int(col, row);

        //        // Dictionary에서 targetCoordinate에 해당하는 CalendarSymbol을 조회합니다.
        //        if (gridDictionary.TryGetValue(targetCoordinate, out CalendarSymbol symbol))
        //        {
        //            // 해당 심볼의 ID가 대상 심볼 목록에 포함되어 있다면 조건 충족
        //            if (context.TargetSymbols.Contains(symbol.Data.Id))
        //            {
        //                Debug.Log($"타겟 심볼 ID {symbol.Data.Id} 를 위치 {targetCoordinate} 에서 발견하였습니다.");
        //                foundPositions.Add(symbol.GridIndex);
        //            }
        //        }
        //    }
        //}

        //foundPositions = foundPositions.Distinct().ToList();

        if (foundCalendarIds.Count > 0)
            return true;
        else
            return false;
    }
}
