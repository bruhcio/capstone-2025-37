using BBB.CSVData;
using DominoGames.RPG;
using RNGNeeds;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        List<int> result = new(PlayerSaveDataModel.data.ownedSymbols);

        for (int i = result.Count; i < 20; i++)
        {
            result.Add(-1);
        }

        // shuffle list
        result = result.OrderBy(_ => Guid.NewGuid()).ToList();

        // Clear and Randomize Calendar Symbol Data
        PlayerSaveDataModel.data.calendar = result;
    }
    #endregion
}
