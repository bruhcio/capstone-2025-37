using BBB.CSVData;
using DominoGames.RPG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CalendarDailyView : UI_Base
{
    [SerializeField, UIAutoAttachField] Image icon;
    [SerializeField] Image backgroundImage;

    int symbolId;
    System.Action onDailyDirectionEnd;
    RP_Symbol symbolScript;

    // 심볼을 스핀합니다
    public void RollSymbol(int symbolId, System.Action onDailyDirectionEnd)
    {
        this.symbolId = symbolId;
        this.onDailyDirectionEnd = onDailyDirectionEnd;
        InitSymbol();
        StartCoroutine(RollSymbolDirection());
    }

    // 심볼을 빈 상태로 만듭니다
    public void ClearSymbol()
    {
        this.symbolId = -1;

        if(symbolScript != null)
        {
            Destroy(symbolScript);
        }
        
        UpdateView();
    }


    public int GetBaseRevenue()
    {
        return Mathf.RoundToInt(GetComponent<StatSystem>().GetBuffValue(EStatTypes.EarnRP) * StatSystem.globalStats["Player"].GetBuffValue(EStatTypes.EarnRP));
    }

    // 심볼의 기본 수익 획득
    public void EarnBaseRevenue()
    {
        PlayerSaveDataModel.data.researchPoint += GetBaseRevenue();
        RPEarnTextEffect.Instantiate(gameObject, GetBaseRevenue());
    }


    // 심볼의 특수 효과 이벤트를 등록합니다
    public void BindSpecialEffect()
    {

    }















    // 심볼 초기화 작업
    private void InitSymbol()
    {
        GetComponent<StatSystem>().ClearBuffsAll();
        var csvData = CSVDataContainer_SymbolData.GetSymbolData(this.symbolId);
        GetComponent<StatSystem>().AddBuff(EStatTypes.EarnRP, EStatCalcTypes.Constant, symbolScript.GetBaseRevenue(csvData.BaseRevenue));

        Type type = Type.GetType("RP_Symbol_" + this.symbolId);

        if (type == null)
        {
            type = Type.GetType("RP_Symbol");
        }

        symbolScript = (RP_Symbol)gameObject.AddComponent(type);
    }

    private void UpdateView()
    {
        if (symbolId == -1)
        {
            icon.gameObject.SetActive(false);
            backgroundImage.sprite = ResourcesCache.symbolBackgroundSprites[0];
            return;
        }

        var symbolData = CSVDataContainer_SymbolData.GetSymbolData(symbolId);
        icon.sprite = ResourcesCache.symbolSprites[symbolId];
        backgroundImage.sprite = ResourcesCache.symbolBackgroundSprites[symbolData.Rarity];
    }

    IEnumerator RollSymbolDirection()
    {
        int randIdx = 0;
        CSVDataRow_SymbolData symbolData;

        int randImageCount = Random.Range(5, 15);
        icon.gameObject.SetActive(true);

        for (int i = 0; i < randImageCount;i++)
        {
            randIdx = Random.Range(0, PlayerSaveDataModel.data.ownedSymbols.Count);
            symbolData = CSVDataContainer_SymbolData.GetSymbolData(PlayerSaveDataModel.data.ownedSymbols[randIdx]);
            icon.sprite = ResourcesCache.symbolSprites[PlayerSaveDataModel.data.ownedSymbols[randIdx]];
            backgroundImage.sprite = ResourcesCache.symbolBackgroundSprites[symbolData.Rarity];

            yield return null;
            yield return null;
            yield return null;
        }

        UpdateView();
        GetComponent<ObjectScaleBouncer>().PlayEffect();

        yield return new WaitForSeconds(0.5f);
        this.onDailyDirectionEnd?.Invoke();
    }
}
