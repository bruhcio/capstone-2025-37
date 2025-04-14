using BBB.CSVData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemEffect
{
    void ExecutePreSymbol(PlayerSaveDataModel playerData, int itemId);
    void ExecutePostSymbol(PlayerSaveDataModel playerData,int itemId);
}

// 예시: 플레이어의 연구 포인트를 증가시키는 효과
public class Item_Test0 : IItemEffect
{
    public void ExecutePreSymbol(PlayerSaveDataModel playerData, int itemId)
    {
        var itemData = CSVDataContainer_ItemData.GetItemData(itemId);

        // 선 순위 효과로 연구 포인트를 증가시킵니다.
        Debug.Log($"{itemData.Name} 선 순위 효과 실행: 연구 포인트 50 추가");
        playerData.researchPoint += 50;
    }

    public void ExecutePostSymbol(PlayerSaveDataModel playerData, int itemId)
    {
        var itemData = CSVDataContainer_ItemData.GetItemData(itemId);

        // 후 순위 효과 실행
        Debug.Log($"{itemData.Name} 후 순위 효과 실행: 연구 포인트 150 추가");
        playerData.researchPoint += 150;
    }
}