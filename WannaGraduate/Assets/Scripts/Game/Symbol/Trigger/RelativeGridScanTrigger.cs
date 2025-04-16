using BBB.CSVData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RelativeGridScanTrigger : ITrigger
{
    public bool Evaluate(TriggerContext context, ref List<Vector2Int> foundPositions)
    {
        if (context == null)
            return false;

        // ExtraData에서 "RelativeSymbolEffectArea"와 "TargetSymbols"를 가져옵니다.
        if (!context.ExtraData.TryGetValue("RelativeSymbolEffectArea", out object relativeAreaObj) ||
            !context.ExtraData.TryGetValue("TargetSymbols", out object targetSymbolIdsObj))
        {
            Debug.LogWarning($"{context.SymbolIndex} 에 필요한 ExtraData 가 누락되었습니다.");
            return false;
        }

        // 캐스팅
        RelativeSymbolEffectArea relativeArea = (RelativeSymbolEffectArea)relativeAreaObj;
        List<int> targetSymbolIds = (List<int>)targetSymbolIdsObj;

        Vector2Int currentIndex = context.SymbolIndex; // 현재 심볼의 그리드 위치
        List<Vector2Int> offsets = GetOffsetsForRelativeArea(relativeArea); // 설정된 상대 영역에 해당하는 오프셋

        var calendarSymbols = PlayerSaveDataModel.data.calenderSymbols;
        if (calendarSymbols == null || calendarSymbols.Count == 0)
        {
            Debug.LogWarning("캘린더 심볼 데이터가 비어있습니다.");
            return false;
        }

        // (1) 기본 오프셋 기반 검사 : 현재 셀을 기준으로 주변(상,하,좌,우,대각선)
        foreach (Vector2Int offset in offsets)
        {
            Vector2Int checkCell = currentIndex + offset;
            foreach (var symbol in calendarSymbols)
            {
                // symbol.GridIndex는 현재 심볼의 위치가 저장된 Vector2Int
                if (symbol.GridIndex.Equals(checkCell))
                {
                    if (targetSymbolIds.Contains(symbol.Data.Id))
                    {
                        Debug.Log($"타겟 심볼 ID {symbol.Data.Id} 를 위치 {checkCell} 에서 발견하였습니다.");
                        foundPositions.Add(symbol.GridIndex);
                    }
                }
            }
        }

        // (2) SameRow 검사: ExtraData에 SameRow 플래그가 있으면, 현재 행 전체를 검사
        if (relativeArea.HasFlag(RelativeSymbolEffectArea.SameRow))
        {
            foreach (var symbol in calendarSymbols)
            {
                if (symbol.GridIndex.y == currentIndex.y)
                {
                    if (targetSymbolIds.Contains(symbol.Data.Id))
                    {
                        Debug.Log($"타겟 심볼 ID {symbol.Data.Id} 를 같은 행 {currentIndex.y} 에서 발견하였습니다.");
                        foundPositions.Add(symbol.GridIndex);
                    }
                }
            }
        }

        // (3) SameColumn 검사: ExtraData에 SameColumn 플래그가 있으면, 현재 열 전체를 검사
        if (relativeArea.HasFlag(RelativeSymbolEffectArea.SameColumn))
        {
            foreach (var symbol in calendarSymbols)
            {
                if (symbol.GridIndex.x == currentIndex.x)
                {
                    if (targetSymbolIds.Contains(symbol.Data.Id))
                    {
                        Debug.Log($"타겟 심볼 ID {symbol.Data.Id}를 같은 열 {currentIndex.x}에서 발견하였습니다.");
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

    private List<Vector2Int> GetOffsetsForRelativeArea(RelativeSymbolEffectArea area)
    {
        List<Vector2Int> offsets = new List<Vector2Int>();

        if (area.HasFlag(RelativeSymbolEffectArea.Self))
            offsets.Add(new Vector2Int(0, 0));
        if (area.HasFlag(RelativeSymbolEffectArea.Top))
            offsets.Add(new Vector2Int(0, 1));
        if (area.HasFlag(RelativeSymbolEffectArea.Bottom))
            offsets.Add(new Vector2Int(0, -1));
        if (area.HasFlag(RelativeSymbolEffectArea.Left))
            offsets.Add(new Vector2Int(-1, 0));
        if (area.HasFlag(RelativeSymbolEffectArea.Right))
            offsets.Add(new Vector2Int(1, 0));
        if (area.HasFlag(RelativeSymbolEffectArea.TopLeft))
            offsets.Add(new Vector2Int(-1, 1));
        if (area.HasFlag(RelativeSymbolEffectArea.TopRight))
            offsets.Add(new Vector2Int(1, 1));
        if (area.HasFlag(RelativeSymbolEffectArea.BottomLeft))
            offsets.Add(new Vector2Int(-1, -1));
        if (area.HasFlag(RelativeSymbolEffectArea.BottomRight))
            offsets.Add(new Vector2Int(1, -1));

        // SameRow와 SameColumn은 별도 로직(예, 전체 행 또는 열에 대해 검사)으로 처리 가능하므로 생략
        return offsets;
    }
}