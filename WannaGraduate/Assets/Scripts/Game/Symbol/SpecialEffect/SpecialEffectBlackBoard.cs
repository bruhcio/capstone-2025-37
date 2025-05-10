using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectBlackBoard
{
    private static Dictionary<int, SymbolSpecialEffectDataContainer> symbolEffectData = new();

    public static void SetSymbolEffectData(int symbolInstanceId, SymbolSpecialEffectDataContainer data)
    {
        symbolEffectData[symbolInstanceId] = data;
    }

    public static SymbolSpecialEffectDataContainer GetSymbolEffectData(int symbolInstanceId)
    {
        return symbolEffectData[symbolInstanceId];
    }

    public static void ClearSymbolEffectData()
    {
        symbolEffectData.Clear();
    }
}

public class SymbolSpecialEffectDataContainer {
    public int evolvingDuration = 0; // 진화 까지 남은 턴
    public int evolvingDestSymbolId = 0; // 진화 시 변경 될 symbol id (기존 심볼 삭제, 이 id를 통해서 새로운 심볼 생성)

}