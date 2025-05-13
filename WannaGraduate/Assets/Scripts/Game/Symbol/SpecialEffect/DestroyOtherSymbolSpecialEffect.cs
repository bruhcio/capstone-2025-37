using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOtherSymbolSpecialEffect : ISymbolSpecialEffect
{
    public void Evaluate(List<string> parameter, int selfCalendarIdx, List<int> interactingCalendarIdx)
    {
        for(int i = 0; i < interactingCalendarIdx.Count; i++)
        {
            if(Random.Range(0f, 1f) < float.Parse(parameter[0]) * 0.01f)
            {
                PlayerSaveDataModel.data.RemoveOwnedSymbol((int)PlayerSaveDataModel.data.calendarSIds[interactingCalendarIdx[i]], interactingCalendarIdx[i]);
            }
        }
    }
}