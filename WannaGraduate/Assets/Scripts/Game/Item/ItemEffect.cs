using BBB.CSVData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public static class GridUtils
    {
        public static readonly int Rows = 5;
        public static readonly int Cols = 4;

        public static readonly (int dY, int dX)[] Directions =
        {
            (-1,  0),  // ╩С
            ( 1,  0),  // го
            ( 0, -1),  // аб
            ( 0,  1),  // ©Л
        };
    }
}
public interface IItemEffect
{
    void Excute(PlayerSaveDataModel playerData, CalendarView calendarView, int itemId);
}