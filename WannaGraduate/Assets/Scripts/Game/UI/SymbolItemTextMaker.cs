using DominoGames.Util;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class SymbolItemTextMaker : MonoBehaviour
{
    public enum TagType
    {
        Symbol,
        Item
    }

    // 태그 추출용 정규식
    private static readonly Regex tagRegex = new Regex(@"<(?<type>symbol|item)=(?<value>\d+)>");
    private static readonly Regex colorTagRegex = new Regex(@"<color=.*?>|</color>", RegexOptions.IgnoreCase);

    public static string RemoveColorTags(string input)
    {
        return colorTagRegex.Replace(input, "");
    }


    [Button]
    public void DebugProcess ()
    {
        ProcessString(GetComponent<TMP_Text>().text, GetComponent<TMP_Text>());
    }

    public static void ProcessString(string input, TMP_Text targetText)
    {
        int childCount = targetText.transform.childCount;
        for (int i = 0; i < childCount - 1; i++)
        {
            ResourcesObjectPooler.Destroy(targetText.transform.GetChild(1).gameObject);
        }

        string[] splitParts = tagRegex.Split(input);

        MatchCollection matches = tagRegex.Matches(input);
        var extracted = new List<(TagType type, int value)>();

        foreach (Match match in matches)
        {
            string typeStr = match.Groups["type"].Value;
            TagType type = (TagType)Enum.Parse(typeof(TagType), Capitalize(typeStr));
            extracted.Add((type, int.Parse(match.Groups["value"].Value)));
        }

        string tmpText = tagRegex.Replace(input, "  `  ");
        targetText.text = tmpText;
        int tagIndex = 0;

        targetText.ForceMeshUpdate(true, true);

        if(extracted.Count == 0)
        {
            return;
        }

        tmpText = RemoveColorTags(tmpText);
        Debug.Log(tmpText);

        // <symbol=1> 또는 <symbol=3> 매 턴 1개 제거 <symbol=3> +2 asdfsaf <symbol=0>

        for (int i = 0; i < tmpText.Length; i++)
        {
            if (tmpText[i].ToString().Equals("`"))
            {
                var tagType = extracted[tagIndex].type;
                var tagValue = extracted[tagIndex].value;

                Vector3 worldPos = GetCharLocalPosition(i, targetText);

                // 2. 부모(Canvas or 텍스트 상위 UI)의 local 좌표로 변환
                Vector3 localPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    targetText.rectTransform,
                    RectTransformUtility.WorldToScreenPoint(null, worldPos),
                    null,
                    out Vector2 localPoint);

                localPos = localPoint;

                GameObject iconObject = ResourcesObjectPooler.Instantiate("TextIconObject");
                iconObject.GetComponent<Image>().sprite = ResourcesCache.GetSymbolSprite(tagValue);
                iconObject.transform.SetParent(targetText.transform, false);
                iconObject.transform.localPosition = localPos + new Vector3(0f, -10f);
                iconObject.transform.localScale = Vector3.one;

                tagIndex++;
            }
        }
    }

    private static string Capitalize(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return char.ToUpper(input[0]) + input.Substring(1);
    }

    private static Vector3 GetCharLocalPosition(int charIndex, TMP_Text tmp)
    {
        TMP_TextInfo textInfo = tmp.textInfo;

        if (charIndex < 0 || charIndex >= textInfo.characterCount)
        {
            Debug.LogWarning("인덱스 범위 초과");
            return Vector3.zero;
        }

        TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
        Vector3 localPos = (charInfo.bottomLeft + charInfo.topRight) / 2f;

        Debug.Log(localPos);

        return tmp.transform.TransformPoint(localPos);
    }
}