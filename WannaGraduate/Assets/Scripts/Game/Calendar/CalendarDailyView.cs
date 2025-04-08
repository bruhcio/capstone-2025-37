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

    int calendarId;
    int symbolId;
    Action onDailyDirectionEnd;
    Action<IEnumerator> bindSpecialEffect;
    RP_Symbol symbolScript;

    // 심볼을 스핀합니다
    public void RollSymbol(int calendarId, Action onDailyDirectionEnd, Action<IEnumerator> bindSpecialEffect)
    {
        this.calendarId = calendarId;
        this.symbolId = PlayerSaveDataModel.data.calendar[calendarId];
        this.onDailyDirectionEnd = onDailyDirectionEnd;
        this.bindSpecialEffect = bindSpecialEffect;
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

















    // 심볼 초기화 작업
    private void InitSymbol()
    {
        if (this.symbolId == -1)
        {
            return;
        }

        // rp 계산을 위한 stat system 초기화
        GetComponent<StatSystem>().ClearBuffsAll();
        var csvData = CSVDataContainer_SymbolData.GetSymbolData(this.symbolId);
        GetComponent<StatSystem>().AddBuff(EStatTypes.EarnRP, EStatCalcTypes.Constant, csvData.BaseRevenue);

        // 심볼 효과 스크립트 로드
        Type type = Type.GetType("RP_Symbol_" + this.symbolId + ", Assembly-CSharp");

        if (type == null)
        {
            type = Type.GetType("RP_Symbol, Assembly-CSharp");
        }

        symbolScript = (RP_Symbol)gameObject.AddComponent(type);
        this.bindSpecialEffect?.Invoke(symbolScript.SpecialEffect());
    }

    private void UpdateView(int symbolId)
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
    private void UpdateView()
    {
        UpdateView(this.symbolId);
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

        if(this.symbolId >= 0)
        {
            GetComponent<ObjectScaleBouncer>().PlayEffect();
        }

        yield return new WaitForSeconds(0.5f);
        this.onDailyDirectionEnd?.Invoke();
    }
}
