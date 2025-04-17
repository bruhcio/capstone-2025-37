using BBB.CSVData;
using DominoGames.UI.PopupSystem;
using RNGNeeds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSymbol_Popup : PopupBase
{
    [SerializeField] ProbabilityList<int> symbolRankList;
    [UIAutoAttachField, SerializeField] List<SymbolSelectPanel> symbolPanels;

    public override void OnHide()
    {

    }

    public override void OnShow(object args)
    {
        // 모든 심볼들 중에 하나가 랜덤하게 등장합니다
        for(int i = 0; i < symbolPanels.Count; i++)
        {
            int rank = symbolRankList.PickValue();

            var targetSymbol = CSVDataContainer_SymbolData.GetRandomSymbolDataByRarity(rank);
            symbolPanels[i].UpdateUI(targetSymbol.Id);
        }
    }
}