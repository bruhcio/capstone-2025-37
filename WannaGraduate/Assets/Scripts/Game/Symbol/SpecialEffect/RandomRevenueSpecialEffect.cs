using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRevenueSpecialEffect : ISymbolSpecialEffect
{
    public void Evaluate(List<string> parameter, int selfCalendarIdx, List<int> interactingCalendarIdx)
    {
        int randomRevenue = Random.Range(int.Parse(parameter[0]), int.Parse(parameter[1] + 1));
        PlayerSaveDataModel.data.AddResearchPoint(randomRevenue, CalendarView.Instance.dayObjects[selfCalendarIdx].gameObject);
    }
}