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
