using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveSpecialEffect : ISymbolSpecialEffect
{
    public void Evaluate(List<string> parameter, int selfCalendarIdx, List<int> interactingCalendarIdx)
    {
        for (int i = 0; i < interactingCalendarIdx.Count; i++)
        {
            CalendarView.inactiveSidForThisTurn.Add((int)PlayerSaveDataModel.data.calendarSIds[interactingCalendarIdx[i]]);
        }
    }
}