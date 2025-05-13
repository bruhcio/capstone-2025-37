using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolvingAfterTurnSpeialEffect : ISymbolSpecialEffect
{
    public void Evaluate(List<string> parameter, int selfCalendarIdx, List<int> interactingCalendarIdx)
    {
        int evolvingTurn = (int)SpecialEffectBlackBoard.GetSymbolEffectData((int)PlayerSaveDataModel.data.calendarSIds[selfCalendarIdx], int.Parse(parameter[0]));

        evolvingTurn--;

        SpecialEffectBlackBoard.SetSymbolEffectData((int)PlayerSaveDataModel.data.calendarSIds[selfCalendarIdx], evolvingTurn);

        if (evolvingTurn <= 0)
        {
            int sid = (int)PlayerSaveDataModel.data.calendarSIds[selfCalendarIdx];
            PlayerSaveDataModel.data.RemoveOwnedSymbol(sid, selfCalendarIdx);
            PlayerSaveDataModel.data.AddOwnedItem(int.Parse(parameter[1]));
            CalendarView.Instance.dayObjects[selfCalendarIdx].SetSymbol(int.Parse(parameter[1]));
        }
    }
}
