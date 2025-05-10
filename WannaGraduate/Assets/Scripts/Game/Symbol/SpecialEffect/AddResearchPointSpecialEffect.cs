using DominoGames.Core.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddResearchPointSpecialEffect : ISymbolSpecialEffect
{
    public void Evaluate(List<string> parameter, int selfCalendarIdx, List<int> interactingCalendarIdx)
    {
        if (parameter[0] == "self" || parameter[0] == "all")
        {
            // 본인
            RPEarnTextEffect.Instantiate(CalendarView.Instance.dayObjects[selfCalendarIdx].gameObject, int.Parse(parameter[1]));
        }
        
        
        if (parameter[0] == "others" || parameter[0] == "all")
        {
            // 상호작용 한 심볼
            foreach(int idx in interactingCalendarIdx)
            {
                RPEarnTextEffect.Instantiate(CalendarView.Instance.dayObjects[idx].gameObject, int.Parse(parameter[1]));
            }
        }
    }
}