using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpinCountUIText : MonoBehaviour
{
    public static SpinCountUIText Instance;
    [SerializeField] TMP_Text text;

    private void Awake()
    {
        Instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        text.text = PlayerSaveDataModel.data.spinCount.ToString();
    }
}
