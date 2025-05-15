using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LKAIROS.Assist;
using Tienma.Assist;

namespace Tienma.MainScene{
    public class BGMPlayer : MonoBehaviour
    {
        public static PriorityList<BGMPlayer> bgmPlayerList = new PriorityList<BGMPlayer>();

        public double priority;
        public string clipPath;
        public float startTime, volume;

        void OnEnable()
        {
            bgmPlayerList.Enqueue(priority, this);
            PlayBGM();
        }

        private void PlayBGM(){
            if(BGMManager.instance == null){
                return;
            }

            BGMPlayer targetPlayer = bgmPlayerList.PeekLast(); // 오름차순 정렬되기 때문에, priority가 제일 큰 놈을 재생하려면 맨 뒤에 있는 요소를 불러와야함
            if(targetPlayer == null){
                return;
            }
            BGMManager.instance.PlayBGM(targetPlayer.clipPath, targetPlayer.startTime, targetPlayer.volume);
        }

        private void OnDisable() {
            bgmPlayerList.RemoveValue(this);
            PlayBGM();
        }
    }
}