using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RPUIText : MonoBehaviour
{
    public static RPUIText Instance;

    [SerializeField] TMP_Text text;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        text.text = PlayerSaveDataModel.data.researchPoint.ToString("N0");
    }
}
