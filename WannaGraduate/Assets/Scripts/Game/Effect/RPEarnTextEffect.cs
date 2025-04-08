using DG.Tweening;
using DominoGames.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RPEarnTextEffect : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    public void PlayEffect(int earnValue)
    {
        GetComponent<ObjectScaleBouncer>().PlayEffect();
        transform.DOLocalMoveY(85f, 0.5f).SetEase(Ease.InQuad).SetDelay(0.5f);
        GetComponent<CanvasGroup>().DOFade(0f, 0.5f).SetEase(Ease.InQuad);
        text.text = "+" + earnValue.ToString("N0");

        ResourcesObjectPooler.Destroy(gameObject);
    }



    public static void Instantiate(GameObject parent, int earnValue)
    {
        var effect = ResourcesObjectPooler.Instantiate("EarnRPTextEffect");
        effect.transform.SetParent(parent.transform);
        effect.transform.position = parent.transform.position;
        effect.GetComponentInParent<RPEarnTextEffect>().PlayEffect(earnValue);
    }
}
