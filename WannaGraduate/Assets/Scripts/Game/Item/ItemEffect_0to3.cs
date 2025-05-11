using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GPU (컴공)
// 코드가 상하좌우 2칸 이내에 있으면 1턴당 +1
public class ItemEffect_0 : IItemEffect
{
    public void Excute(PlayerSaveDataModel playerData, CalendarView calendarView, int itemId)
    {
        // ISH
        // TODO: 심볼 강화형이므로 확인 필요
        throw new System.NotImplementedException();
    }
}

// 알고리즘 (컴공)
// 코드가 3개 이상 있으면 코드 1개당 +2
public class ItemEffect_1 : IItemEffect
{
    public void Excute(PlayerSaveDataModel playerData, CalendarView calendarView, int itemId)
    {
        const int earn = 2;
        int count = 0;

        foreach (var obj in calendarView.dayObjects)
        {
            if (obj?.GetSymbolId() is int id && id != -1)
            {
                if (id == 0)
                    count++;
            }
        }

        // ISH
        // TODO: 결과를 playerData 등에 반영
        if (count >= 3)
        {
            Debug.Log($"알고리즘: total earn = {count * earn}");
        }
    }
}

// 질문 (컴공)
// 매턴마다 버그 or 충돌 1개씩 없어짐
public class ItemEffect_2 : IItemEffect
{
    public void Excute(PlayerSaveDataModel playerData, CalendarView calendarView, int itemId)
    {
        foreach (var obj in calendarView.dayObjects)
        {
            if (obj?.GetSymbolId() is int id && id != -1)
            {
                // ISH
                // TODO: 결과를 playerData 등에 반영
                if (id == 2 || id == 5)
                {
                    Debug.Log("질문: 매턴마다 버그 or 충돌 1개씩 없어짐");
                    break;
                }
            }
        }
    }
}

// VPN (컴공)
// 충돌 부정 효과 없앰, 심볼을 없애지는 않음
public class ItemEffect_3 : IItemEffect
{
    public void Excute(PlayerSaveDataModel playerData, CalendarView calendarView, int itemId)
    {
        // ISH
        // TODO: 심볼 강화형이므로 확인 필요
        throw new System.NotImplementedException();
    }
}
