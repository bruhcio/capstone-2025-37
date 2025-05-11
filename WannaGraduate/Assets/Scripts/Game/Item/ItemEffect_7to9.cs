using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 의대추천서(생명학과)
// 논문이 존재하면 매턴 +5
public class ItemEffect_7 : IItemEffect
{
    public void Excute(PlayerSaveDataModel playerData, CalendarView calendarView, int itemId)
    {
        const int targetSymbol = 11;
        int earn = 0;

        foreach (var dayObj in calendarView.dayObjects)
        {
            if (dayObj.GetSymbolId() == targetSymbol)
            {
                earn += 5;
                break;
            }
        }

        // ISH
        // TODO: 결과를 playerData 등에 반영
        if (earn > 0)
        {
            Debug.Log($"의대추천서: total earn = {earn}");
        }
    }
}

// 조언서(생명학과)
// 쥐 진화 턴 -1
public class ItemEffect_8 : IItemEffect
{
    public void Excute(PlayerSaveDataModel playerData, CalendarView calendarView, int itemId)
    {
        // ISH
        // TODO: 심볼 강화형이므로 확인 필요
        throw new System.NotImplementedException();
    }
}

// 보고서(실험)(생명학과)
// 실험 심볼 점수 +2
public class ItemEffect_9 : IItemEffect
{
    public void Excute(PlayerSaveDataModel playerData, CalendarView calendarView, int itemId)
    {
        // ISH
        // TODO: 심볼 강화형이므로 확인 필요
        throw new System.NotImplementedException();
    }
}