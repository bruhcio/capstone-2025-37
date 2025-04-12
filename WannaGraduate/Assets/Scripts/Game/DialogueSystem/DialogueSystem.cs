using BBB.CSVData;
using I2.Loc;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] DialogueView view;
    // 대화 데이터 로드 (n번째 대화 묶음의 m번째 대화 텍스트, 화자)

    // 다이얼로그 시작

    // 입력 처리 (터치, 클릭) => 스킵, 다음

    int dialogueIndex = 0;
    List<CSVDataRow_DialogueData> dialogueList = new();


    public static DialogueSystem Instance;

    private void Awake()
    {
        Instance = this;
    }



    [Button]
    public void StartDialogue(string dialogueId)
    {
        dialogueIndex = 0;
        dialogueList = CSVDataContainer_DialogueData.dialogueDataById[dialogueId];

        view.SetViewActive(true);
        PlayDialogue();
    }

    public void OnControlInput()
    {
        if (view.IsTweening())
        {
            // 대화 연출 진행중이면 complete
            view.SkipTween();
        }
        else
        {
            // 대화 연출 끝났으면 다음으로 넘어가기
            if(dialogueIndex >= dialogueList.Count)
            {
                view.SetViewActive(false);
            }
            else
            {
                PlayDialogue();
            }
        }
    }







    private void PlayDialogue()
    {
        var dialogueData = dialogueList[dialogueIndex];
        view.PlayView(Resources.Load<Sprite>("Sprites/Portrait/" + dialogueData.CharacterId),
           LocalizationManager.GetTermTranslation("CharacterName." + dialogueData.CharacterId),
           dialogueData.GetDialogueTranslation());

        dialogueIndex++;
    }
}
