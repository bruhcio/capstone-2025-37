using BBB.CSVData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RelativeGridScanTrigger : ITrigger
{
    public bool Evaluate(TriggerParameter context, ref List<Vector2Int> foundPositions)
    {
        if (context == null)
            return false;

        Vector2Int currentIndex = context.SymbolIndex; // 현재 심볼의 그리드 위치
        List<Vector2Int> offsets = GetOffsetsForRelativeArea(context.RelativeArea); // 설정된 상대 영역에 해당하는 오프셋

        var calendarSymbols = PlayerSaveDataModel.data.calenderSymbols;
        if (calendarSymbols == null || calendarSymbols.Count == 0)
        {
            Debug.LogWarning("Emtpy list: calenderSymbols");
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
                    if (context.TargetSymbols.Contains(symbol.Data.Id))
                    {
                        Debug.Log($"Find symbol id={symbol.Data.Id}, in cell={checkCell}");
                        foundPositions.Add(symbol.GridIndex);
                    }
                }
            }
        }

        // (2) SameRow 검사: ExtraData에 SameRow 플래그가 있으면, 현재 행 전체를 검사
        if (context.RelativeArea.HasFlag(RelativeSymbolEffectArea.SameRow))
        {
            foreach (var symbol in calendarSymbols)
            {
                if (symbol.GridIndex.y == currentIndex.y)
                {
                    if (context.TargetSymbols.Contains(symbol.Data.Id))
                    {
                        Debug.Log($"Find symbol id={symbol.Data.Id}, in row={currentIndex.y}");
                        foundPositions.Add(symbol.GridIndex);
                    }
                }
            }
        }

        // (3) SameColumn 검사: ExtraData에 SameColumn 플래그가 있으면, 현재 열 전체를 검사
        if (context.RelativeArea.HasFlag(RelativeSymbolEffectArea.SameColumn))
        {
            foreach (var symbol in calendarSymbols)
            {
                if (symbol.GridIndex.x == currentIndex.x)
                {
                    if (context.TargetSymbols.Contains(symbol.Data.Id))
                    {
                        Debug.Log($"Find symbol id={symbol.Data.Id}, in col={currentIndex.x}");
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