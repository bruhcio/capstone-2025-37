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
        text.text = "<size=80%>다음 실적 평가까지</size>  " + PlayerSaveDataModel.data.spinCount.ToString() + "개월";
    }
}
