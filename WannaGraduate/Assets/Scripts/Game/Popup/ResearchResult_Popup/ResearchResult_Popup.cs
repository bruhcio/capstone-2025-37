using DominoGames.Core.EventSystem;
using DominoGames.UI.PopupSystem;
using LKAIROS.Assist;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchResult_Popup : PopupBase
{
    [SerializeField] TMP_Text currentRPText, minusRPText, resultRPText;

    [SerializeField] Button hideButton;

    public override void OnHide()
    {

    }

    public override void OnShow(object args)
    {
        SoundManager.instance.Play("Sounds/SFX/GachaDetail1", false, true, 0, "SFX");
        hideButton.interactable = false;
        StartCoroutine(PopupDirection());
    }

    IEnumerator PopupDirection()
    {
        currentRPText.text = "";
        minusRPText.text = "";
        resultRPText.text = "";

        yield return new WaitForSeconds(0.5f);
        SoundManager.instance.Play("Sounds/SFX/BuySound1", false, true, 0, "SFX");

        currentRPText.text = PlayerSaveDataModel.data.researchPoint.ToString("N0");
        currentRPText.GetComponent<ObjectScaleBouncer>().PlayEffect();

        yield return new WaitForSeconds(0.35f);
        SoundManager.instance.Play("Sounds/SFX/BuySound1", false, true, 0, "SFX");

        minusRPText.text = "<color=#FF7C7C>" + EnvVar.researchMinus[PlayerSaveDataModel.data.researchMinusIndex].ToString("N0") + "</color>";
        minusRPText.GetComponent<ObjectScaleBouncer>().PlayEffect();

        yield return new WaitForSeconds(0.35f);
        SoundManager.instance.Play("Sounds/SFX/BuySound1", false, true, 0, "SFX");

        PlayerSaveDataModel.data.researchPoint -= EnvVar.researchMinus[PlayerSaveDataModel.data.researchMinusIndex];
        resultRPText.text = PlayerSaveDataModel.data.researchPoint.ToString("N0");
        resultRPText.GetComponent<ObjectScaleBouncer>().PlayEffect();

        hideButton.interactable = true;
    }

    public void HidePopup()
    {
        SoundManager.instance.Play("Sounds/SFX/ButtonClick", false, true, 0, "SFX");

        DominoEventSystem.Pub(EEventTypes.OnSymbolSelectEnd);
        PopupSystem.GameSceneJYS.ResearchResult_Popup.Hide();

        if(PlayerSaveDataModel.data.researchPoint <= 0)
        {
            Debug.Log("게임 오버!");
        }
        else
        {
            PopupSystem.GameSceneJYS.AddSymbol_Popup.Show(null, popup =>
            {
                popup.gameObject.SetActive(true);
            });
        }

        PlayerSaveDataModel.data.spinCount = 6;
        PlayerSaveDataModel.data.researchMinusIndex++;
    }
}
