using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DominoGames.DominoRx.DataModel;
using DominoGames.DominoRx.RxField;

public class PlayerSaveDataModel : RxDataModel<PlayerSaveDataModel>, IRxDataModelInitializable
{
    public int week = 0;

    public void InitializeData()
    {

    }
}
