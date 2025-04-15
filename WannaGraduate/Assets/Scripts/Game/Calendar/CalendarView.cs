using BBB.CSVData;
using DominoGames.Core.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarView : UI_Base
{
    public static CalendarView Instance;
    [SerializeField] Button spinButton;
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
        List<CalendarDailyView> revList = new(dayObjects);
        revList.Sort((a, b) => (a.GetBaseRevenue() - b.GetBaseRevenue()));
        StartCoroutine(EarnBaseRevenueCoroutine(revList));
    }
    IEnumerator EarnBaseRevenueCoroutine(List<CalendarDailyView> revList)
    {
        int startIndex = 0;
        for(int i = 0; i < revList.Count; i++)
        {
            if (revList[i].GetBaseRevenue() == 0)
            {
                continue;
            }

            Debug.Log("called");

            if(i == revList.Count - 1 || revList[i].GetBaseRevenue() != revList[i + 1].GetBaseRevenue())
            {
                for(int j = startIndex; j <= i; j++)
                {
                    revList[j].EarnBaseRevenue();
                }

                startIndex = i + 1;

                yield return new WaitForSeconds(0.5f);
            }
        }
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
            Debug.Log(oneDaySpecialEffectEnd);
            oneDaySpecialEffectEnd++;

            if(oneDaySpecialEffectEnd >= 20)
            {
                DominoEventSystem.Pub(EEventTypes.OnSpecialEffectEnd);
                EarnBaseRevenue();
            }
        });
    }

    private void Awake()
    {
        Instance = this;
        BindEvents();
    }

}
