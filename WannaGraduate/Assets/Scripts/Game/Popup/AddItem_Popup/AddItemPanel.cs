using BBB.CSVData;
using DominoGames.UI.PopupSystem;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddItemPanels : MonoBehaviour
{
    [UIAutoAttachField, SerializeField] Image iconImage;
    [UIAutoAttachField, SerializeField] TMP_Text nameText, specialEffectText;

    List<Color> panelColors = new() { Color.white, new(0.5f, 1f, 0f, 1f), new(0f, 1f, 1f, 1f), new(1f, 0.75f, 0f, 1f) };

    int itemId;

    public void UpdateUI(int targetItemId)
    {
        var symbolData = CSVDataContainer_ItemData.data.m_Items[itemId];

        itemId = itemId;
        iconImage.sprite = Resources.Load<Sprite>("Sprites/Items/" + itemId);
        nameText.text = LocalizationManager.GetTermTranslation("ItemName." + targetItemId);
        specialEffectText.text = LocalizationManager.GetTermTranslation("ItemEffect." + targetItemId);

        SymbolItemTextMaker.ProcessString(specialEffectText.text, specialEffectText);

        GetComponent<Image>().color = panelColors[symbolData.Rarity];
    }

    public void OnClick()
    {
        // 심볼을 Owned Symbol에 추가
        PlayerSaveDataModel.data.AddOwnedSymbol(itemId);
        PopupSystem.GameSceneJYS.AddSymbol_Popup.Hide(popup =>
        {
            popup.gameObject.SetActive(false);
        });
    }
}
