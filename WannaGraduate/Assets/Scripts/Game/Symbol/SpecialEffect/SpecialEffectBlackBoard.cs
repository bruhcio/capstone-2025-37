using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectBlackBoard
{
    private static Dictionary<int, object> symbolEffectData = new();

    public static object GetSymbolEffectData(int symbolInstanceId, object defaultValue)
    {
        if (!symbolEffectData.ContainsKey(symbolInstanceId))
        {
            symbolEffectData[symbolInstanceId] = defaultValue;
        }

        return symbolEffectData[symbolInstanceId];
    }

    public static void SetSymbolEffectData(int symbolInstanceId, object value)
    {
        symbolEffectData[symbolInstanceId] = value;
    }

    public static void ClearSymbolEffectData()
    {
        symbolEffectData.Clear();
    }
}