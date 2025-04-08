using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DominoGames.DominoRx.DataModel;
using DominoGames.DominoRx.RxField;

public class PlayerSaveDataModel : RxDataModel<PlayerSaveDataModel>, IRxDataModelInitializable
{
    public int spinCount = 6; // 연구 실적 평가까지 남은 개월 수 (남은 spin 횟수)
    public int researchPoint = 0; // 연구 포인트

    public List<int> ownedSymbols = new() { 0, 0, 0, 0, 1, 3, 5, 10, 15, 20, 20, 22, 32, 35 }; // 현재 보유 심볼
    public List<int> calendar = new(); // 심볼 id 를 담고 있는 캘린더 데이터입니다

    public void InitializeData()
    {

    }
}