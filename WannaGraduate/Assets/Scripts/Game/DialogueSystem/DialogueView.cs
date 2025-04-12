using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView : MonoBehaviour
{
    [SerializeField] Image portraitImage;
    [SerializeField] TMP_Text nameText, contentText;

    TweenerCore<string, string, StringOptions> doTextTween;

    bool activeState = false;

    public void SetViewActive(bool status)
    {
        if(status == activeState)
        {
            return;
        }

        if (status)
        {
            activeState = true;
            gameObject.SetActive(true);
            GetComponent<CanvasGroup>().alpha = 0f;
            GetComponent<CanvasGroup>().DOFade(1f, 0.5f).SetEase(Ease.OutQuart);
            GetComponent<Animator>().Play("StartDialogue", 0, 0f);
        }
        else
        {
            activeState = false;
            GetComponent<CanvasGroup>().alpha = 1f;
            GetComponent<CanvasGroup>().DOFade(0f, 0.5f).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }

    public void PlayView(Sprite portraitSprite, string name, string content)
    {
        portraitImage.sprite = portraitSprite;
        nameText.text = name;
        contentText.text = "";
        doTextTween = contentText.DOText(content, content.Length * 0.03f).SetEase(Ease.Linear);
    }

    public bool IsTweening()
    {
        return doTextTween.IsPlaying();
    }

    public void SkipTween()
    {
        doTextTween.Complete();
    }
}
