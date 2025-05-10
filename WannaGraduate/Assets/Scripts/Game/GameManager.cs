using BBB.CSVData;
using DominoGames.Core.EventSystem;
using DominoGames.RPG;
using RNGNeeds;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerSaveDataModel;

public class GameManager : MonoBehaviour
{
    [SerializeField] CalendarView calendarView;

    public bool canRollCalendar = true;

    #region Public
    [Button]
    public void RollCalendar()
    {
        RandomizeCalendar();
        calendarView.RollCalendar();
    }

    // 턴 시작 시점 아이템 효과 (예: 선 효과 실행)
    public void OnTurnStart()
    {
    }

    // 심볼 효과 적용 전에 발동하는 아이템 효과 (예: 선 효과 실행)
    public void OnPreSymbol()
    {
        ExecuteEffectsForOrder(ItemOrder.PreSymbol, (itemId, itemData) =>
        {
            ExecuteItemEffect(itemData, PlayerSaveDataModel.data, itemId, effect =>
            {
                effect.ExecutePreSymbol(PlayerSaveDataModel.data, itemId);
            });
        });
    }

    // 심볼 효과 적용 후 발동하는 아이템 효과 (예: 후 효과 실행)
    public void OnPostSymbol()
    {
        ExecuteEffectsForOrder(ItemOrder.PostSymbol, (itemId, itemData) =>
        {
            ExecuteItemEffect(itemData, PlayerSaveDataModel.data, itemId, effect =>
            {
                effect.ExecutePreSymbol(PlayerSaveDataModel.data, itemId);
            });
        });
    }

    // 턴 종료 시점 아이템 효과 (예: 후 효과 실행)
    public void OnTurnEnd()
    {

    }

    #endregion


    #region Private
    private void ExecuteEffectsForOrder(ItemOrder targetOrder, Action<int, CSVDataRow_ItemData> executeEffect)
    {
        foreach (int itemId in PlayerSaveDataModel.data.ownedItems.Values)
        {
            CSVDataRow_ItemData itemData = CSVDataContainer_ItemData.GetItemData(itemId);
            if (itemData.Order == targetOrder)
            {
                executeEffect?.Invoke(itemId, itemData);
            }
        }
    }

    public void ExecuteItemEffect(CSVDataRow_ItemData itemData, PlayerSaveDataModel playerData, int itemId, Action<IItemEffect> effectAction)
    {
        string typeName = "Item_" + itemData.Name;  // 예: "Item_IncreaseResearchPointEffect"
        string assemblyQualifiedName = $"{typeName}, Assembly-CSharp"; // 어셈블리 이름은 프로젝트에 맞게 조정

        // 리플렉션으로 효과 타입을 가져옵니다.
        Type effectType = Type.GetType(assemblyQualifiedName);
        if (effectType == null)
        {
            Debug.LogError($"Could not find effect type: {assemblyQualifiedName}");
            return;
        }

        IItemEffect effectInstance = Activator.CreateInstance(effectType) as IItemEffect;
        if (effectInstance == null)
        {
            Debug.LogError($"The created effect instance cannot be cast to IItemEffect: {assemblyQualifiedName}");
            return;
        }

        effectAction?.Invoke(effectInstance);
    }


    private void RandomizeCalendar()
    {
        PlayerSaveDataModel.data.calendarSIds = PlayerSaveDataModel.data.ownedSymbols.Keys.Select<int, int?>(x => x).ToList();

        for (int i = PlayerSaveDataModel.data.calendarSIds.Count; i < 20; i++)
        {
            PlayerSaveDataModel.data.calendarSIds.Add(null);
        }

        // shuffle list
        System.Random rand = new();
        PlayerSaveDataModel.data.calendarSIds = PlayerSaveDataModel.data.calendarSIds.OrderBy(_ => rand.Next()).ToList();
        Debug.Log(PlayerSaveDataModel.data.calendarSIds.Count);
    }


    private void Awake()
    {
        BindDialogues();
    }



    // 대화 설정
    private void BindDialogues()
    {
        DialogueSystem.Instance.StartDialogue("Start");

        DominoEventSystem.Sub(EEventTypes.OnEarnBaseRevenueEnd, () =>
        {
            if (PlayerSaveDataModel.data.dialogueIndex == 0)
            {
                DialogueSystem.Instance.StartDialogue("AfterSpin");
                PlayerSaveDataModel.data.dialogueIndex++;
            }
        });

        DominoEventSystem.Sub(EEventTypes.OnSymbolSelectEnd, () =>
        {
            if(PlayerSaveDataModel.data.researchPoint >= 0)
            {
                DialogueSystem.Instance.StartDialogue("AfterResult." + PlayerSaveDataModel.data.researchMinusIndex);
                PlayerSaveDataModel.data.dialogueIndex++;
            }
        });
    }
    #endregion
}
