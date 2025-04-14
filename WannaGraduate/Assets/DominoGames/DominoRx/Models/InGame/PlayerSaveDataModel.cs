using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DominoGames.DominoRx.DataModel;
using DominoGames.DominoRx.RxField;
using BBB.CSVData;




public class PlayerSaveDataModel : RxDataModel<PlayerSaveDataModel>, IRxDataModelInitializable
{
    public int spinCount = 6;       // 연구 실적 평가까지 남은 개월 수 (남은 spin 횟수)
    public int researchPoint = 0;   // 연구 포인트

    public Dictionary<int, CSVDataRow_SymbolData> originSymbols = new();    // 원형 심볼 테이블
    public List<CSVDataRow_SymbolData> ownedSymbols = new();                // 소유 심볼
    public List<int> ownedItems = new();                                    // 소유 아이템
    public List<CSVDataRow_SymbolData> calenderSymbols = new();             // 캘린더 내 심볼

    public void InitializeData()
    {
        originSymbols.Clear();
        ownedSymbols.Clear();
        ownedItems.Clear();
        calenderSymbols.Clear();

        // 원형 심볼 데이터 추가
        foreach (var symbolData in CSVDataContainer_SymbolData.data.m_Items)
        {
            originSymbols.Add(symbolData.Id, symbolData);
        }

        // 기본 데이터 추가
        for (int i = 0; i < 5; i++)
        {
            Debug.Log($"Add Symbol ID {i}");
            ownedSymbols.Add(originSymbols[i]);
        }
    }
}