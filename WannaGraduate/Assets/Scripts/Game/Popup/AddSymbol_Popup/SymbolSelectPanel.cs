using BBB.CSVData;
using DominoGames.UI.PopupSystem;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SymbolSelectPanel : UI_Base
{
    [UIAutoAttachField, SerializeField] Image iconImage;
    [UIAutoAttachField, SerializeField] TMP_Text nameText, baseRpText, specialEffectText;

    int symbolId;

    public void UpdateUI(int targetSymbolId)
    {
        symbolId = targetSymbolId;
        iconImage.sprite = Resources.Load<Sprite>("Sprites/Symbols/" + targetSymbolId);
        nameText.text = LocalizationManager.GetTermTranslation("SymbolName." + targetSymbolId);
        baseRpText.text = CSVDataContainer_SymbolData.GetSymbolData(targetSymbolId).BaseRevenue.ToString();
        specialEffectText.text = LocalizationManager.GetTermTranslation("SymbolEffect." + targetSymbolId);
    }

    public void OnClick()
    {
        // 심볼을 Owned Symbol에 추가
        PlayerSaveDataModel.data.ownedSymbols.Add(CSVDataContainer_SymbolData.GetSymbolData(symbolId));
    }
}
