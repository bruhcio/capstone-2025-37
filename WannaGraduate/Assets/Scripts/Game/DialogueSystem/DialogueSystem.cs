using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] DialogueView view;
    // 대화 데이터 로드 (n번째 대화 묶음의 m번째 대화 텍스트, 화자)

    // 다이얼로그 시작

    // 입력 처리 (터치, 클릭) => 스킵, 다음



    public static DialogueSystem Instance;

    private void Awake()
    {
        Instance = this;
    }



    public void StartDialogue(int dialogueId)
    {

    }

    public void OnControlInput()
    {

    }
}
