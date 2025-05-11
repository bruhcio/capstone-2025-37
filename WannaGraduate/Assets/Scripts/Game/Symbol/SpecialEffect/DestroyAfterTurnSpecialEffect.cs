using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTurnSpecialEffect : ISymbolSpecialEffect
{
    public void Evaluate(List<string> parameter, int selfCalendarIdx, List<int> interactingCalendarIdx)
    {
        int destroyTurn = (int)SpecialEffectBlackBoard.GetSymbolEffectData((int)PlayerSaveDataModel.data.calendarSIds[selfCalendarIdx], int.Parse(parameter[0]));

        destroyTurn--;

        SpecialEffectBlackBoard.SetSymbolEffectData((int)PlayerSaveDataModel.data.calendarSIds[selfCalendarIdx], destroyTurn);

        if(destroyTurn <= 0)
        {
            int sid = (int)PlayerSaveDataModel.data.calendarSIds[selfCalendarIdx];
            PlayerSaveDataModel.data.RemoveOwnedSymbol(sid, selfCalendarIdx);
        }
    }
}