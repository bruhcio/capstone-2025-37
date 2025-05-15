using BBB.CSVData;
using DominoGames.Core.EventSystem;
using DominoGames.RPG;
using RNGNeeds;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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

    #endregion


    #region Private
    private void RandomizeCalendar()
    {
        PlayerSaveDataModel.data.calendarSIds = PlayerSaveDataModel.data.ownedSymbols.Keys.Select<int, int?>(x => x).ToList();

        for (int i = PlayerSaveDataModel.data.calendarSIds.Count; i < 20; i++)
        {
            PlayerSaveDataModel.data.calendarSIds.Add(null);
        }

        if (PlayerSaveDataModel.data.calendarSIds.Count > 20)
            PlayerSaveDataModel.data.calendarSIds.SetLength(20);

        // shuffle list
        System.Random rand = new();
        PlayerSaveDataModel.data.calendarSIds = PlayerSaveDataModel.data.calendarSIds.OrderBy(_ => rand.Next()).ToList();
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
