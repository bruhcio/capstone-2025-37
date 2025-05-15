using BBB.CSVData;
using DominoGames.UI.PopupSystem;
using LKAIROS.Assist;
using RNGNeeds;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddSymbol_Popup : PopupBase
{
    [SerializeField] ProbabilityList<int> symbolRankList;
    [UIAutoAttachField, SerializeField] List<SymbolSelectPanel> symbolPanels;
    [SerializeField] TMP_Text leftMonthText;

    public override void OnHide()
    {
        SoundManager.instance.Play("Sounds/SFX/ButtonClick", false, true, 0, "SFX");
    }

    public override void OnShow(object args)
    {
        SoundManager.instance.Play("Sounds/SFX/GachaDetailLast_3", false, true, 0, "SFX");
        leftMonthText.text = "<color=#00FFFF>" + (PlayerSaveDataModel.data.spinCount) + "달</color> 후에 연구 실적 평가";

        // 모든 심볼들 중에 하나가 랜덤하게 등장합니다
        for (int i = 0; i < symbolPanels.Count; i++)
        {
            int rank = symbolRankList.PickValue();

            var targetSymbol = CSVDataContainer_SymbolData.GetRandomSymbolDataByRarity(rank);
            symbolPanels[i].UpdateUI(targetSymbol.Id);
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