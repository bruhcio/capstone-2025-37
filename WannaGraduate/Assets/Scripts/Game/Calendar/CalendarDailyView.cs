using BBB.CSVData;
using DominoGames.Core.EventSystem;
using DominoGames.RPG;
using QFSW.QC.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VInspector.Libs;
using static PlayerSaveDataModel;
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
    public void RollSymbol(int calendarId, Action onDailyDirectionEnd)
    {
        this.calendarId = calendarId;
        this.symbolId = PlayerSaveDataModel.data.GetSymbolIdFromCalendar(calendarId);
        this.onDailyDirectionEnd = onDailyDirectionEnd;

        // 심볼 초기화
        InitSymbol();

        StartCoroutine(RollSymbolDirection());
    }

    // 심볼을 빈 상태로 만듭니다
    public void ClearSymbol()
    {
        this.symbolId = -1;

        if (symbolScript != null)
        {
            Destroy(symbolScript);
        }

        UpdateView();
    }


    public int GetBaseRevenue()
    {
        if (this.symbolId == -1)
        {
            return 0;
        }

        return Mathf.RoundToInt(GetComponent<StatSystem>().GetBuffValue(EStatTypes.EarnRP) * StatSystem.globalStats["Player"].GetBuffValue(EStatTypes.EarnRP));
    }

    // 심볼의 기본 수익 획득
    public void EarnBaseRevenue()
    {
        if (this.symbolId == -1)
        {
            return;
        }

        PlayerSaveDataModel.data.researchPoint += GetBaseRevenue();
        GetComponent<ObjectScaleBouncer>().PlayEffect();
        RPEarnTextEffect.Instantiate(gameObject, GetBaseRevenue());
    }

    public int GetSymbolId()
    {
        return this.symbolId;
    }


    // 심볼 초기화 작업
    private void InitSymbol()
    {
        if (this.symbolId == -1)
        {
            DominoEventSystem.Pub(EEventTypes.OnOneDaySpecialEffectEnd);
            return;
        }

        // rp 계산을 위한 stat system 초기화
        GetComponent<StatSystem>().ClearBuffsAll();
        var csvData = CSVDataContainer_SymbolData.GetSymbolData(this.symbolId);
        GetComponent<StatSystem>().AddBuff(EStatTypes.EarnRP, EStatCalcTypes.Constant, csvData.BaseRevenue);
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
        icon.sprite = ResourcesCache.GetSymbolSprite(symbolId);
        backgroundImage.sprite = ResourcesCache.symbolBackgroundSprites[symbolData.Rarity];
    }
    private void UpdateView()
    {
        UpdateView(this.symbolId);
    }

    IEnumerator RollSymbolDirection()
    {
        int randIdx = 0;
        int symbolId;

        int randImageCount = Random.Range(5, 15);
        icon.gameObject.SetActive(true);

        for (int i = 0; i < randImageCount; i++)
        {
            randIdx = Random.Range(0, PlayerSaveDataModel.data.ownedSymbols.Count);
            symbolId = PlayerSaveDataModel.data.ownedSymbols.Values.ToList()[randIdx];
            var symbolData = CSVDataContainer_SymbolData.GetSymbolData(symbolId);
            icon.sprite = ResourcesCache.GetSymbolSprite(symbolId);
            backgroundImage.sprite = ResourcesCache.symbolBackgroundSprites[symbolData.Rarity];

            yield return null;
        }

        UpdateView();

        if (this.symbolId >= 0)
        {
            GetComponent<ObjectScaleBouncer>().PlayEffect();
        }

        yield return new WaitForSeconds(0.5f);
        this.onDailyDirectionEnd?.Invoke();
    }
}
