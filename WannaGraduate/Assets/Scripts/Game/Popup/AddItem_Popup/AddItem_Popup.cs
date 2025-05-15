using BBB.CSVData;
using DominoGames.UI.PopupSystem;
using RNGNeeds;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddItem_Popup : PopupBase
{
    [UIAutoAttachField, SerializeField] List<SymbolSelectPanel> itemPanels;

    public override void OnHide()
    {

    }

    public override void OnShow(object args)
    {
        // 모든 심볼들 중에 하나가 랜덤하게 등장합니다
        for (int i = 0; i < itemPanels.Count; i++)
        {
            var targetSymbol = CSVDataContainer_ItemData.GetRandomItem();
            itemPanels[i].UpdateUI(targetSymbol.Id);
        }
    }

    public void SkipSelection()
    {
        PopupSystem.GameSceneJYS.AddSymbol_Popup.Hide(popup =>
        {
            popup.gameObject.SetActive(false);
        });
    }
}
