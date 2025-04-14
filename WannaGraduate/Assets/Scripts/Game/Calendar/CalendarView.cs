using BBB.CSVData;
using DominoGames.Core.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarView : UI_Base
{
    public static CalendarView Instance;
    [UIAutoAttachField] public List<CalendarDailyView> dayObjects;


    // Ä¶¸°´õ ÃÊ±âÈ­
    public void ClearCalendar()
    {
        for (int i = 0; i < dayObjects.Count; i++)
        {
            dayObjects[i].ClearSymbol();
        }
    }

    // ÀÏÁ¤ °èÈ¹ ½ºÇÉ
    public void RollCalendar()
    {
        ClearCalendar();
        StartCoroutine(RollCalendarDirection());
    }

    // ±âº» ¼öÀÍ È¹µæ
    private void EarnBaseRevenue()
    {

    }

    List<IEnumerator> specialEffectCoroutines = new();
    // Æ¯¼ö È¿°ú Àç»ý
    private IEnumerator PlaySpecialEffect()
    {
        for(int i = 0; i < specialEffectCoroutines.Count; i++)
        {
            yield return specialEffectCoroutines[i];
        }
    }


    // ÀÏÁ¤ ½ºÇÉ ¿¬Ãâ
    IEnumerator RollCalendarDirection()
    {
        oneDaySpecialEffectEnd = 0;

        // Update day view
        for (int i = 0; i < dayObjects.Count; i++)
        {
            dayObjects[i].RollSymbol(i, OnCalendarDailyDirectionEnd, coroutine =>
            {
                specialEffectCoroutines.Add(coroutine);
            });
            yield return new WaitForSeconds(0.02f);
        }
    }

    int dailyDirectionCount = 0;
    private void OnCalendarDailyDirectionEnd()
    {
        dailyDirectionCount++;

        if(dailyDirectionCount >= 20)
        {
            StartCoroutine(PlaySpecialEffect());
        }
    }

    int oneDaySpecialEffectEnd = 0;
    private void BindEvents()
    {
        DominoEventSystem.Sub(EEventTypes.OnOneDaySpecialEffectEnd, () =>
        {
            oneDaySpecialEffectEnd++;

            if(oneDaySpecialEffectEnd >= 20)
            {
                DominoEventSystem.Pub(EEventTypes.OnSpecialEffectEnd);
            }
        });
    }

    private void Awake()
    {
        Instance = this;
        BindEvents();
    }

}
