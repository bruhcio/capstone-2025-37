using DominoGames.UI.PopupSystem;
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
        hideButton.interactable = false;
        StartCoroutine(PopupDirection());
    }

    IEnumerator PopupDirection()
    {
        yield return new WaitForSeconds(0.5f);
        currentRPText.text = "";
        minusRPText.text = "";
        resultRPText.text = "";

        currentRPText.text = PlayerSaveDataModel.data.researchPoint.ToString("N0");
        currentRPText.GetComponent<ObjectScaleBouncer>().PlayEffect();

        yield return new WaitForSeconds(0.35f);

        minusRPText.text = "<color=#FF7C7C>" + EnvVar.researchMinus[PlayerSaveDataModel.data.researchMinusIndex].ToString("N0") + "</color>";
        minusRPText.GetComponent<ObjectScaleBouncer>().PlayEffect();

        yield return new WaitForSeconds(0.35f);

        PlayerSaveDataModel.data.researchPoint -= EnvVar.researchMinus[PlayerSaveDataModel.data.researchMinusIndex];
        resultRPText.text = PlayerSaveDataModel.data.researchPoint.ToString("N0");
        resultRPText.GetComponent<ObjectScaleBouncer>().PlayEffect();

        hideButton.interactable = true;
    }

    public void HidePopup()
    {
        PopupSystem.GameSceneJYS.ResearchResult_Popup.Hide();

        if(PlayerSaveDataModel.data.researchPoint <= 0)
        {
            Debug.Log("게임 오버!");
        }
        else
        {
            PopupSystem.GameSceneJYS.AddSymbol_Popup.Show(null);
        }
    }
}
