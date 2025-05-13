using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolvingWithOthersSpecialEffect : ISymbolSpecialEffect
{
    public void Evaluate(List<string> parameter, int selfCalendarIdx, List<int> interactingCalendarIdx)
    {
        if(Random.Range(0f, 1f) < float.Parse(parameter[0]) * 0.01f)
        {
            PlayerSaveDataModel.data.RemoveOwnedSymbol((int)PlayerSaveDataModel.data.calendarSIds[interactingCalendarIdx[0]], interactingCalendarIdx[0]);
            PlayerSaveDataModel.data.RemoveOwnedSymbol((int)PlayerSaveDataModel.data.calendarSIds[selfCalendarIdx], selfCalendarIdx);
            PlayerSaveDataModel.data.AddOwnedSymbol(int.Parse(parameter[1]));
            CalendarView.Instance.dayObjects[selfCalendarIdx].SetSymbol(int.Parse(parameter[1]));
        }
    }
}
