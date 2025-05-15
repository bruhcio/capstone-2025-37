using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LKAIROS.Assist;

namespace Tienma.Assist{
    public class BGMManager : MonoBehaviour
    {
        public static BGMManager instance;
        bool isInit = false;

        private void Awake() {
            if(instance == null){
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else{
                Destroy(gameObject);
            }
        }

        private string prevClipPath = "";

        public void PlayBGM(string clipPath, float time, float volume = 1f){
            if(prevClipPath == clipPath){
                SoundManager.instance.SetVolumeByGroup("BGM", volume, 0.5f);
                return;
            }

            prevClipPath = clipPath;

            if(bgmTransitionCoroutine != null){
                StopCoroutine(bgmTransitionCoroutine);
            }

            bgmTransitionCoroutine = StartCoroutine(BGMTransitionRoutine(clipPath, time, volume));
        }
        Coroutine bgmTransitionCoroutine;
        IEnumerator BGMTransitionRoutine(string clipPath, float time, float volume){
            if(!isInit){
                SoundManager.instance.SetVolumeByGroup("BGM", 0f, 0f);
            }
            else{
                SoundManager.instance.SetVolumeByGroup("BGM", 0f, 0.5f);
                yield return new WaitForSecondsRealtime(0.5f);
                SoundManager.instance.StopByGroup("BGM");
            }

            isInit = true;
            
            SoundManager.instance.Play(clipPath, true, false, time, "BGM");
            SoundManager.instance.SetVolumeByGroup("BGM", volume, 0.5f);
        }
    }
}