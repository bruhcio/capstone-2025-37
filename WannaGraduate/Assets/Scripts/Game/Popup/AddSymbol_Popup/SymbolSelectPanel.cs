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

    List<Color> panelColors = new() { Color.white, new(0.5f, 1f, 0f, 1f), new(0f, 1f, 1f, 1f), new(1f, 0.75f, 0f, 1f) };

    int symbolId;

    public void UpdateUI(int targetSymbolId)
    {
        var symbolData = CSVDataContainer_SymbolData.GetSymbolData(targetSymbolId);

        symbolId = targetSymbolId;
        iconImage.sprite = Resources.Load<Sprite>("Sprites/Symbols/" + targetSymbolId);
        nameText.text = LocalizationManager.GetTermTranslation("Rarity." + symbolData.Rarity) + " " + LocalizationManager.GetTermTranslation("SymbolName." + targetSymbolId);
        baseRpText.text = symbolData.BaseRevenue.ToString();
        specialEffectText.text = LocalizationManager.GetTermTranslation("SymbolEffect." + targetSymbolId);

        SymbolItemTextMaker.ProcessString(specialEffectText.text, specialEffectText);
        Debug.Log(specialEffectText.text);

        GetComponent<Image>().color = panelColors[symbolData.Rarity];
    }

    public void OnClick()
    {
        // 심볼을 Owned Symbol에 추가
        PlayerSaveDataModel.data.AddOwnedSymbol(symbolId);
        PopupSystem.GameSceneJYS.AddSymbol_Popup.Hide(popup =>
        {
            popup.gameObject.SetActive(false);
        });
    }
}
