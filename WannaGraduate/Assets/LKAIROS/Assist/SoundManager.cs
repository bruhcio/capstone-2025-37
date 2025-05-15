using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using DominoGames.Util;

// Docs : https://www.notion.so/SoundManager-cs-7b24e8d0988a4d01accbb0b9e4e5373e

namespace LKAIROS.Assist{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        private void Awake() {
            if(instance == null){
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else{
                DestroyImmediate(gameObject);
            }
        }

        Dictionary<string, int> audioIdCounter = new Dictionary<string, int>();
        Dictionary<string, Dictionary<int, AudioSourceSetting>> audioSourcesInPlaying = new Dictionary<string, Dictionary<int, AudioSourceSetting>>();
        Dictionary<string, HashSet<AudioSourceSetting>> audioGroup = new Dictionary<string, HashSet<AudioSourceSetting>>();
        Dictionary<string, float> groupVolume = new Dictionary<string, float>();
        Dictionary<string ,float> userGroupVolume = new Dictionary<string, float>();

        float defaultVolume = 1f;

        //-------- Public API ---------
        [Button]
        public AudioSourceSetting Play(string clipPath, bool isLoop, bool useRewind, float time = 0f, string group = ""){
            AudioSourceSetting setting = null;

            if(useRewind){
                setting = _GetOneAudioSourceSettingInPlayingOrNull(clipPath);
            }

            if(setting == null){
                setting = _GetNewAudioSourceSetting(clipPath, isLoop, group);
            }

            setting.SetTime(time);
            setting.Play();
            
            if(!isLoop && !useRewind){
                Coroutine destroyCoroutine = setting.GetDestroyCoroutine();
                if(destroyCoroutine != null){
                    StopCoroutine(destroyCoroutine);
                }

                setting.SetDestroyCoroutine(StartCoroutine(_DestroyAudioSourceInstanceAfterClipDuration(setting)));
            }

            return setting;
        }

        [Button]
        public void StopByAudioSourceSetting(AudioSourceSetting setting){
            if(!setting.GetIsStop()){
                _StopAndDestroyAudioSourceSetting(setting);
            }
        }

        [Button]
        public void StopAllByClipPath(string clipPath){
            if(!audioSourcesInPlaying.ContainsKey(clipPath)){
                return;
            }

            string targetGroup = "";

            List<AudioSourceSetting> settings = audioSourcesInPlaying[clipPath].Values.ToList();
            for(int i=0;i<settings.Count;i++){
                _StopAndDestroyAudioSourceSetting(settings[i]);
                targetGroup = settings[i].GetGroup();
            }

            audioSourcesInPlaying[clipPath].Clear();

            if(audioGroup.ContainsKey(targetGroup)){
                audioGroup[targetGroup].RemoveWhere(x => x.GetClipPath() == clipPath);
            }
        }

        [Button]
        public void StopAll(){
            foreach(string clipPath in audioSourcesInPlaying.Keys){
                List<AudioSourceSetting> settings = audioSourcesInPlaying[clipPath].Values.ToList();
                for(int i=0;i<settings.Count;i++){
                    _StopAndDestroyAudioSourceSetting(settings[i]);
                }
            }

            audioSourcesInPlaying.Clear();
            audioGroup.Clear();
        }

        [Button]
        public void StopByGroup(string group){
            if(!audioGroup.ContainsKey(group)){
                return;
            }
            
            List<AudioSourceSetting> settings = audioGroup[group].ToList();
            for(int i=0;i<settings.Count;i++){
                _StopAndDestroyAudioSourceSetting(settings[i]);
            }

            audioGroup[group].Clear();
        }

        [Button]
        public void SetVolumeAll(float percent, float lerpDuration = 0f){
            defaultVolume = percent;
            List<string> groups = groupVolume.Keys.ToList();
            for(int i=0;i<groups.Count;i++){
                SetVolumeByGroup(groups[i], percent, lerpDuration);
            }
        }

        [Button]
        public void SetVolumeByGroup(string group, float percent, float lerpDuration = 0f){
            _SetGroupVolume(group, percent);

            if(audioGroup.ContainsKey(group)){
                foreach(AudioSourceSetting setting in audioGroup[group]){
                    if(lerpDuration == 0){
                        setting.SetVolume(percent);
                    }
                    else{
                        setting.SetVolumeLerp(_GetFinalGroupVolume(group), lerpDuration);
                    }
                }
            }
        }

        [Button]
        public void SetUserVolumeByGroup(string group, float percent){
            _SetUserGroupVolume(group, percent);

            if(audioGroup.ContainsKey(group)){
                foreach(AudioSourceSetting setting in audioGroup[group]){
                    setting.SetVolume(_GetFinalGroupVolume(group));
                }
            }
        }

        public float GetVolumeByGroup(string group){
            if(groupVolume.ContainsKey(group)){
                return groupVolume[group];
            }

            return 1f;
        }

        public float GetUserVolumeByGroup(string group){
            if(userGroupVolume.ContainsKey(group)){
                return userGroupVolume[group];
            }

            return 1f;
        }

        public float GetFinalVolumeByGroup(string group){
            return _GetGroupVolume(group) * _GetUserGroupVolume(group);
        }




        //-------- Private Functions ---------
        private IEnumerator _DestroyAudioSourceInstanceAfterClipDuration(AudioSourceSetting targetSetting){
            yield return new WaitForSecondsRealtime(targetSetting.GetClipDuration());
            _StopAndDestroyAudioSourceSetting(targetSetting);
        }
        private void _StopAndDestroyAudioSourceSetting(AudioSourceSetting targetSetting){
            targetSetting.Stop();
            audioSourcesInPlaying[targetSetting.GetClipPath()].Remove(targetSetting.GetId());
            string targetGroup = targetSetting.GetGroup();
            if(targetGroup != ""){
                audioGroup[targetGroup].Remove(targetSetting);
            }

            ResourcesObjectPooler.Destroy(targetSetting.GetInstance());
        }

        private int _GetAudioInstanceId(string clipPath){
            if(!audioIdCounter.ContainsKey(clipPath)){
                audioIdCounter.Add(clipPath, 0);
            }

            return audioIdCounter[clipPath]++;
        }

        private GameObject _CreateAudioInstance(){
            GameObject audioSourceInstance = ResourcesObjectPooler.Instantiate("AudioSourcePrefab");
            audioSourceInstance.transform.SetParent(transform);
            return audioSourceInstance;
        }

        private AudioSourceSetting _GetOneAudioSourceSettingInPlayingOrNull(string clipPath){
            if(audioSourcesInPlaying.ContainsKey(clipPath)){
                if(audioSourcesInPlaying[clipPath].Count > 0){
                    int targetKey = audioSourcesInPlaying[clipPath].Keys.ToArray()[0];
                    return audioSourcesInPlaying[clipPath][targetKey];
                }
            }

            return null;
        }

        private float _GetGroupVolume(string group){
            if(!groupVolume.ContainsKey(group)){
                groupVolume.Add(group, defaultVolume);
            }

            return groupVolume[group];
        }
        private void _SetGroupVolume(string group, float volume){
            if(!groupVolume.ContainsKey(group)){
                groupVolume.Add(group, volume);
            }

            groupVolume[group] = volume;
        }

        private float _GetUserGroupVolume(string group){
            if(!userGroupVolume.ContainsKey(group)){
                userGroupVolume.Add(group, defaultVolume);
            }

            return userGroupVolume[group];
        }
        private void _SetUserGroupVolume(string group, float volume){
            if(!userGroupVolume.ContainsKey(group)){
                userGroupVolume.Add(group, volume);
            }

            userGroupVolume[group] = volume;
        }
        private float _GetFinalGroupVolume(string group){
            return _GetUserGroupVolume(group) * _GetGroupVolume(group);
        }
        
        private AudioSourceSetting _GetNewAudioSourceSetting(string clipPath, bool isLoop, string group){
            int instanceId = _GetAudioInstanceId(clipPath);
            AudioSourceSetting result = new AudioSourceSetting(instanceId, _CreateAudioInstance(), clipPath, isLoop, group, _GetFinalGroupVolume(group));

            if(!audioSourcesInPlaying.ContainsKey(clipPath)){
                audioSourcesInPlaying.Add(clipPath, new Dictionary<int, AudioSourceSetting>());
            }
            audioSourcesInPlaying[clipPath].Add(instanceId, result);

            if(group != ""){
                if(!audioGroup.ContainsKey(group)){
                    audioGroup.Add(group, new HashSet<AudioSourceSetting>());
                }
                audioGroup[group].Add(result);
            }

            return result;
        }
    }

    public class AudioSourceSetting{
        private static Dictionary<string, AudioClip> cachedClips = new Dictionary<string, AudioClip>();
        private AudioClip GetAudioClip(string clipPath){
            if(!cachedClips.ContainsKey(clipPath)){
                cachedClips.Add(clipPath, Resources.Load<AudioClip>(clipPath));
            }
            return cachedClips[clipPath];
        }
        
        private int id;
        private string clipPath, group;
        private AudioSource source;
        private GameObject instance;
        private Coroutine destroyCoroutine = null, volumeLerpCoroutine = null;
        private bool isStop = false;
        private float destVolume = 1f;

        public AudioSourceSetting(int id, GameObject instance, string clipPath, bool isLoop, string group, float volume){
            this.id = id;
            this.instance = instance;
            this.clipPath = clipPath;
            this.source = instance.GetComponent<AudioSource>();
            this.source.clip = GetAudioClip(clipPath);
            this.source.loop = isLoop;
            this.destVolume = volume;
            this.source.volume = destVolume;
            this.group = group;
        }

        public void SetTime(float time){
            this.source.time = time;
            this.isStop = false;
        }
        public float GetTime(){
            return this.source.time;
        }

        public void Play(){
            this.source.Play();
        }

        public void SetDestroyCoroutine(Coroutine destroyCoroutine){
            this.destroyCoroutine = destroyCoroutine;
        }
        public Coroutine GetDestroyCoroutine(){
            return this.destroyCoroutine;
        }

        public GameObject GetInstance(){
            return instance;
        }

        public float GetClipDuration(){
            return source.clip.length;
        }

        public string GetClipPath(){
            return clipPath;
        }

        public int GetId(){
            return id;
        }

        public string GetGroup(){
            return group;
        }

        public void Stop(){
            this.isStop = true;
        }
        public bool GetIsStop(){
            return this.isStop;
        }

        public void SetVolume(float volume){
            this.destVolume = volume;
            if(volumeLerpCoroutine == null){
                this.source.volume = destVolume;
            }
        }

        public void SetVolumeLerp(float destVolume, float duration){
            if(volumeLerpCoroutine != null){
                SoundManager.instance.StopCoroutine(volumeLerpCoroutine);
            }

            this.destVolume = destVolume;
            volumeLerpCoroutine = SoundManager.instance.StartCoroutine(VolumeLerpRoutine(duration));
        }
        IEnumerator VolumeLerpRoutine(float duration){
            float startVolume = this.source.volume;
            int frames = Mathf.FloorToInt(duration * Application.targetFrameRate);
            for(int i=0;i<frames;i++){
                this.source.volume = (this.destVolume - startVolume) / frames * i + startVolume;
                yield return null;
            }

            this.source.volume = this.destVolume;
            volumeLerpCoroutine = null;
        }
    }
}