using BBB.CSVData;
using DominoGames.Core.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DominoGames.UI.PopupSystem;
using DominoGames.RPG;
using System.Globalization;
using System.Linq;
using System;
using DG.Tweening;
using UnityEngine.Purchasing;

public class CalendarView : UI_Base
{
    public static CalendarView Instance;
    [SerializeField] Button spinButton;
    [UIAutoAttachField] public List<CalendarDailyView> dayObjects;


    // 특정 캘린더의 StatSystem 반환
    public StatSystem GetStatSystem(int calendarIndex)
    {
        return dayObjects[calendarIndex].GetComponent<StatSystem>();
    }

    public CalendarDailyView GetDailyView(Vector2Int gridPosition)
    {
        return dayObjects[gridPosition.y * 4 + gridPosition.x];
    }
    public CalendarDailyView GetDailyView(int y, int x)
    {
        return dayObjects[y * 4 + x];
    }



    // 캘린더 초기화
    public void ClearCalendar()
    {
        for (int i = 0; i < dayObjects.Count; i++)
        {
            dayObjects[i].ClearSymbol();
        }
    }

    // 일정 계획 스핀
    public void RollCalendar()
    {
        PlayerSaveDataModel.data.spinCount--;
        spinButton.interactable = false;
        ClearCalendar();
        StartCoroutine(RollCalendarDirection());
    }

    public bool CheckTrigger(int targetCalendarIdx, out List<int> foundCalendarIdx)
    {
        foundCalendarIdx = new List<int>();

        int? sidNullable = PlayerSaveDataModel.data.calendarSIds[targetCalendarIdx];

        if (sidNullable == null)
        {
            return false;
        }

        int sid = (int)sidNullable;
        int symbolId = PlayerSaveDataModel.data.ownedSymbols[sid];

        // 심볼 트리거 타입 및 파라미터 데이터 로드
        var csvData = CSVDataContainer_SymbolData.GetSymbolData(symbolId);

        // 트리거 스크립트 로드
        Type type = Type.GetType(csvData.Trigger + ", Assembly-CSharp");

        if (type == null)
        {
            Debug.LogError($"Invalid trigger type for id={symbolId}");
            return false;
        }

        // 트리거 객체 생성
        var trigger = (ITrigger)Activator.CreateInstance(type);

        // 트리거 파라미터 생성
        var triggerParameter = new TriggerParameter
        {
            CalendarIndex = targetCalendarIdx,
            RelativeArea = csvData.RelativeArea,
            //AbsoluteArea = csvData.AbsoluteArea,
            TargetSymbols = csvData.TargetSymbols.ToList(),
        };

        if (trigger.Evaluate(triggerParameter, ref foundCalendarIdx))
        {
            Debug.Log($"Trigger={type.Name}: return true");
            return true;
        }

        Debug.Log($"Trigger={type.Name}: return false");
        return false;
    }









    // 기본 수익 획득
    private void EarnBaseRevenue()
    {
        List<CalendarDailyView> revList = new(dayObjects);
        revList.Sort((a, b) => (a.GetBaseRevenue() - b.GetBaseRevenue()));
        StartCoroutine(EarnBaseRevenueCoroutine(revList));
    }
    IEnumerator EarnBaseRevenueCoroutine(List<CalendarDailyView> revList)
    {
        DominoEventSystem.Pub(EEventTypes.OnEarnBaseRevenueStart);
        int startIndex = 0;
        for(int i = 0; i < revList.Count; i++)
        {
            if (revList[i].GetBaseRevenue() == 0)
            {
                startIndex = i + 1;
                continue;
            }

            if(startIndex != i)
            {
                if (i == revList.Count - 1 || revList[i].GetBaseRevenue() != revList[i + 1].GetBaseRevenue())
                {
                    for (int j = startIndex; j <= i; j++)
                    {
                        revList[j].EarnBaseRevenue();
                    }

                    startIndex = i + 1;

                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

        spinButton.interactable = true;

        if (PlayerSaveDataModel.data.spinCount <= 0)
        {
            PopupSystem.GameSceneJYS.ResearchResult_Popup.Show(null);
        }
        else
        {
            PopupSystem.GameSceneJYS.AddSymbol_Popup.Show(null, popup =>
            {
                popup.gameObject.SetActive(true);
            });
        }

        DominoEventSystem.Pub(EEventTypes.OnEarnBaseRevenueEnd);
    }




    // 특수 효과 재생
    private IEnumerator PlaySpecialEffect()
    {
        for(int i = 0; i < PlayerSaveDataModel.data.calendarSIds.Count; i++)
        {
            if (PlayerSaveDataModel.data.calendarSIds[i] == null)
            {
                continue;
            }

            int sid = (int)PlayerSaveDataModel.data.calendarSIds[i];
            int symbolId = PlayerSaveDataModel.data.ownedSymbols[sid];

            List<int> interactedCalendarIdx = new();
            if(CheckTrigger(i, out interactedCalendarIdx))
            {
                // 심볼 흔들기
                dayObjects[i].transform.DOScale(1.5f, 0.3f).SetEase(Ease.OutQuart);
                foreach(int idx in interactedCalendarIdx)
                {
                    dayObjects[idx].transform.DOShakePosition(0.5f, 20f, 20, 90, false, false);
                }
                dayObjects[i].transform.DOShakePosition(0.5f, 20f, 20, 90, false, false);
                yield return new WaitForSeconds(0.5f);

                // 심볼 특수 효과 타입 및 파라미터 로드
                var csvData = CSVDataContainer_SymbolData.GetSymbolData(symbolId);

                // 특수효과 스크립트 로드
                Type type = Type.GetType(csvData.SpecialEffect + ", Assembly-CSharp");

                if (type == null)
                {
                    Debug.LogError($"Invalid special effect type for id={symbolId}");
                    continue;
                }

                // 특수효과 객체 생성
                var specialEffect = (ISymbolSpecialEffect)Activator.CreateInstance(type);
                specialEffect.Evaluate(csvData.SpecialEffectParams.ToList(), i, interactedCalendarIdx);

                dayObjects[i].transform.DOScale(1f, 0.3f).SetEase(Ease.OutQuart);
                yield return new WaitForSeconds(0.3f);
            }
        }

        DominoEventSystem.Pub(EEventTypes.OnSpecialEffectEnd);
        EarnBaseRevenue();
    }


    // 일정 스핀 연출
    IEnumerator RollCalendarDirection()
    {
        dailyDirectionCount = 0;
        // Update day view
        for (int i = 0; i < dayObjects.Count; i++)
        {
            dayObjects[i].RollSymbol(i, OnCalendarDailyDirectionEnd);
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

    private void Awake()
    {
        Instance = this;

        ClearCalendar();
        spinButton.interactable = true;
    }

}
