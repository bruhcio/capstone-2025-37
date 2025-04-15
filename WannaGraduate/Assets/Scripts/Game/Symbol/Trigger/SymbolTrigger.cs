using BBB.CSVData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 평가에 필요한 정보들을 담는 컨텍스트 클래스
public class TriggerContext
{
    // 현재 심볼의 그리드 상 위치
    public Vector2Int SymbolIndex { get; set; }

    // 추가 데이터 (필요시 추가)
    public Dictionary<string, object> ExtraData { get; private set; }
}

public interface ITrigger
{
    bool Evaluate(TriggerContext context, ref List<Vector2Int> foundPositions);
}