using BBB.CSVData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 평가에 필요한 정보들을 담는 클래스
public class TriggerParameter
{
    // 현재 심볼의 그리드 상 위치
    public Vector2Int SymbolIndex { get; set; }

    // 추가 데이터 (필요시 추가)
    //public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

    public RelativeSymbolEffectArea RelativeArea;
    public AbsoluteSymbolEffectArea AbsoluteArea;
    public List<int> TargetSymbols;
}

public interface ITrigger
{
    bool Evaluate(TriggerParameter parameter, ref List<Vector2Int> foundPositions);
}