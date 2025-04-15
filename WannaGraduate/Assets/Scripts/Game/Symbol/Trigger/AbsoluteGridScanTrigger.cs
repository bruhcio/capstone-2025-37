using BBB.CSVData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PlayerSaveDataModel;

public class AbsoluteGridScanTrigger : ITrigger
{
    public bool Evaluate(TriggerContext context, ref List<Vector2Int> foundPositions)
    {
        if (context == null)
            return false;

        // ExtraData에서 "AbsoluteSymbolEffectArea"와 "TargetSymbols"를 가져옵니다.
        if (!context.ExtraData.TryGetValue("AbsoluteSymbolEffectArea", out object absoluteAreaObj) ||
            !context.ExtraData.TryGetValue("TargetSymbols", out object targetSymbolIdsObj))
        {
            Debug.LogWarning($"{context.SymbolIndex} 에 필요한 ExtraData 가 누락되었습니다.");
            return false;
        }

        // 캐스팅
        AbsoluteSymbolEffectArea absoluteArea = (AbsoluteSymbolEffectArea)absoluteAreaObj;
        List<int> targetSymbolIds = (List<int>)targetSymbolIdsObj;

        // 그리드 데이터
        var calenderSymbols = PlayerSaveDataModel.data.calenderSymbols;
        if (calenderSymbols == null || calenderSymbols.Count != 20)
        {
            Debug.LogWarning("캘린더 심볼 데이터가 비어있습니다.");
            return false;
        }

        // 사전(Dictionary)를 이용해 각 GridIndex로 CalendarSymbol을 빠르게 조회할 수 있도록 함
        Dictionary<Vector2Int, CalendarSymbol> gridDictionary = new Dictionary<Vector2Int, CalendarSymbol>();
        foreach (var symbol in calenderSymbols)
        {
            gridDictionary[symbol.GridIndex] = symbol;
        }

        // 0부터 19까지 각 셀을 검사합니다.
        for (int i = 0; i < 20; i++)
        {
            // i번째 셀에 대응하는 플래그 생성 (예: i=0이면 1<<0, i=1이면 1<<1, ...)
            AbsoluteSymbolEffectArea cellFlag = (AbsoluteSymbolEffectArea)(1 << i);
            if (absoluteArea.HasFlag(cellFlag))
            {
                // 셀 인덱스 i를 기반으로 행과 열 계산 (4행 x 5열)
                int row = i / 5;
                int col = i % 5;
                Vector2Int targetCoordinate = new Vector2Int(col, row);

                // Dictionary에서 targetCoordinate에 해당하는 CalendarSymbol을 조회합니다.
                if (gridDictionary.TryGetValue(targetCoordinate, out CalendarSymbol symbol))
                {
                    // 해당 심볼의 ID가 대상 심볼 목록에 포함되어 있다면 조건 충족
                    if (targetSymbolIds.Contains(symbol.Data.Id))
                    {
                        Debug.Log($"타겟 심볼 ID {symbol.Data.Id} 를 위치 {targetCoordinate} 에서 발견하였습니다.");
                        foundPositions.Add(symbol.GridIndex);
                    }
                }
            }
        }

        foundPositions = foundPositions.Distinct().ToList();

        if (foundPositions.Count > 0)
            return true;
        else
            return false;
    }
}
