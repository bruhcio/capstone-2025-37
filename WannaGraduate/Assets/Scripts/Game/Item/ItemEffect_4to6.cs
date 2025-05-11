using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전류 (전자과)
// 실험장비 1개당 +1
public class ItemEffect_4 : IItemEffect
{
    public void Excute(PlayerSaveDataModel playerData, CalendarView calendarView, int itemId)
    {
        const int targetSymbol = 11;
        int earn = 0;

        foreach (var dayObj in calendarView.dayObjects)
        {
            if (dayObj.GetSymbolId() == targetSymbol)
            {
                earn++;
            }
        }

        // ISH
        // TODO: 결과를 playerData 등에 반영
        if (earn > 0)
        {
            Debug.Log($"전류: total earn = {earn}");
        }
    }
}

// 멀티탭(전자과)
// 회로도 상하좌우 4칸 내 실험장비, 렌즈 1개당 +2
public class ItemEffect_5 : IItemEffect
{
    // 보너스 심볼 ID
    private static readonly HashSet<int> BonusSymbols = new HashSet<int> { 11, 13 };

    public void Excute(PlayerSaveDataModel playerData, CalendarView calendarView, int itemId)
    {
        const int targetSymbol = 10;
        int earn = 0;

        for (int y = 0; y < Item.GridUtils.Rows; y++)
        {
            for (int x = 0; x < Item.GridUtils.Cols; x++)
            {
                // 대상 심볼이 아니라면 건너뛰기
                if (calendarView.GetDailyView(y, x).GetSymbolId() != targetSymbol)
                    continue;

                // 4방향 이웃 검사
                foreach (var (dY, dX) in Item.GridUtils.Directions)
                {
                    int ny = y + dY;
                    int nx = x + dX;

                    // 경계 체크
                    if (ny < 0 || ny >= Item.GridUtils.Rows || nx < 0 || nx >= Item.GridUtils.Cols)
                        continue;

                    int neighbourId = calendarView.GetDailyView(ny, nx).GetSymbolId();
                    if (BonusSymbols.Contains(neighbourId))
                        earn += 2;
                }
            }
        }

        // ISH
        // TODO: 결과를 playerData 등에 반영
        if (earn > 0)
        {
            Debug.Log($"멀티탭: total earn = {earn}");
        }
    }
}


// LED전구(전자과)
// 렌즈 전체 강화 → 렌즈 1개당 +1
public class ItemEffect_6 : IItemEffect
{
    public void Excute(PlayerSaveDataModel playerData, CalendarView calendarView, int itemId)
    {
        // ISH
        // TODO: 심볼 강화형이므로 확인 필요
    }
}