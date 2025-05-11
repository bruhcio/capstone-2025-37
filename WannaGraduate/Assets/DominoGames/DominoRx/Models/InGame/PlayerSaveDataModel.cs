using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DominoGames.DominoRx.DataModel;
using DominoGames.DominoRx.RxField;
using BBB.CSVData;




public class PlayerSaveDataModel : RxDataModel<PlayerSaveDataModel>, IRxDataModelInitializable
{
    public static int symbolInstanceId = int.MinValue;
    public static int itemInstanceId = int.MinValue;

    public int researchMinusIndex = 0; // 연구 실적 평가 차수 (n차 연구 실적 평가)
    public int spinCount = 6;       // 연구 실적 평가까지 남은 개월 수 (남은 spin 횟수)
    public int researchPoint = 0;   // 연구 포인트

    public Dictionary<int, int> ownedSymbols = new();                       // 소유 심볼
    public Dictionary<int, int> ownedItems = new();                         // 소유 아이템
    public List<int?> calendarSIds = new();                                  // 캘린더 내 심볼 인스턴스 id (ownedSymbol id)


    public string playerName = "";

    public int dialogueIndex = 0;   // 대화 인덱스

    public void InitializeData()
    {
        CSVDataContainer_SymbolData.InitData();

        // 기본 데이터 추가
        for (int i = 0; i < 5; i++)
        {
            Debug.Log($"Add Symbol ID {i}");
            AddOwnedSymbol(i);
        }
    }

    // calendarIndex -> symbol instance id (sid) -> ownedSymbols 접근
    public int GetSymbolIdFromCalendar(int calendarId)
    {
        int? sid = calendarSIds[calendarId];

        if(sid == null)
        {
            return -1;
        }

        return ownedSymbols[(int)sid];
    }

    public void AddOwnedSymbol(int symbolId)
    {
        ownedSymbols.Add(symbolInstanceId++, symbolId);
    }

    public void AddOwnedItem(int itemId)
    {
        ownedItems.Add(itemInstanceId++, itemId);
    }

    public void RemoveOwnedSymbol(int sid, int calendarId = -1)
    {
        var csvData = CSVDataContainer_SymbolData.GetSymbolData(PlayerSaveDataModel.data.ownedSymbols[sid]);

        if(calendarId == -1)
        {
            RPEarnTextEffect.Instantiate(null, csvData.OnDestroyRP);
        }
        else
        {
            RPEarnTextEffect.Instantiate(CalendarView.Instance.dayObjects[calendarId].gameObject, csvData.OnDestroyRP);
            CalendarView.Instance.dayObjects[calendarId].ClearSymbol();
        }

        ownedSymbols.Remove(sid);
    }

    public void RemoveOwnedItem(int sid)
    {
        ownedItems.Remove(sid);
    }
}