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
    public void RollSymbol(int calendarId, Action onDailyDirectionEnd, Action<IEnumerator> bindSpecialEffect)
    {
        this.calendarId = calendarId;
        this.symbolId = PlayerSaveDataModel.data.calenderSymbols[calendarId].Data.Id;
        this.onDailyDirectionEnd = onDailyDirectionEnd;
        this.bindSpecialEffect = bindSpecialEffect;

        // 심볼 초기화
        InitSymbol();

        //심볼 트리거 체크
        if (CheckTrigger(out List<Vector2Int> foundPositions))
        {
            // 심볼 특수 효과 (RP_Symbol_Id)

        }

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


        // 심볼 효과 스크립트 로드
        Type type = Type.GetType("RP_Symbol_" + this.symbolId + ", Assembly-CSharp");

        if (type == null)
        {
            type = Type.GetType("RP_Symbol, Assembly-CSharp");
        }

        symbolScript = (RP_Symbol)gameObject.AddComponent(type);
        this.bindSpecialEffect?.Invoke(symbolScript.SpecialEffect(null));
    }

    private bool CheckTrigger(out List<Vector2Int> foundPositions)
    {
        foundPositions = new List<Vector2Int>();

        if (this.symbolId == -1)
        {
            return false;
        }


        // 심볼 트리거 타입 및 파라미터 데이터 로드
        var csvData = CSVDataContainer_SymbolData.GetSymbolData(this.symbolId);

        // 트리거 스크립트 로드
        Type type = Type.GetType(csvData.Trigger + ", Assembly-CSharp");

        if (type == null)
        {
            Debug.LogError($"Invalid trigger type for id={this.symbolId}");
            return false;
        }

        // 트리거 객체 생성
        var trigger = (ITrigger)Activator.CreateInstance(type);

        // 트리거 파라미터 생성
        var triggerParameter = new TriggerParameter
        {
            SymbolIndex = new Vector2Int(calendarId / 5, calendarId % 5),
            RelativeArea = csvData.RelativeArea,
            //AbsoluteArea = csvData.AbsoluteArea,
            TargetSymbols = csvData.TargetSymbols.ToList(),
        };

        if (trigger.Evaluate(triggerParameter, ref foundPositions))
        {
            Debug.Log($"Trigger={type.Name}: return true");
            return true;
        }

        Debug.Log($"Trigger={type.Name}: return false");
        return false;
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
        CSVDataRow_SymbolData symbolData;

        int randImageCount = Random.Range(5, 15);
        icon.gameObject.SetActive(true);

        for (int i = 0; i < randImageCount; i++)
        {
            randIdx = Random.Range(0, PlayerSaveDataModel.data.ownedSymbols.Count);
            symbolData = PlayerSaveDataModel.data.ownedSymbols[randIdx];
            icon.sprite = ResourcesCache.GetSymbolSprite(symbolData.Id);
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
