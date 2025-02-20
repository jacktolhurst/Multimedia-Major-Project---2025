using UnityEngine;
using System.Collections.Generic;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [System.Serializable]
    public class SoundEventClass{
        public string name;
        
        [field: SerializeField] public EventReference eventReference {get; private set;}
        [HideInInspector] public FMOD.Studio.EventInstance eventInstance;

        [HideInInspector] public Vector3 position;

        public float BPM;
        private float baseBPM;
        private float originalBPM;

        public bool continuous;
        public bool dontPlay;
        [HideInInspector] public bool playNow;

        public void FirstUpdate(){
            baseBPM = BPM;
            originalBPM = BPM;
            eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventReference);
        }

        public void UpdateSound(){
            BPM = baseBPM;
        }

        public void PlaySound(Vector3 newPosition){
            position = newPosition;
            playNow = true;
        }

        public void StopSound(){
            position = Vector3.zero;
            playNow = false;
            AudioManager.instance.StopSound(eventInstance);
        }

        public float GetBPM(){
            return BPM;
        }

        public float GetOriginalBPM(){
            return originalBPM;
        }

        public void ChangeBPM(float newBPM){
            float prevBPM = BPM;
            baseBPM = newBPM;
            BPM = newBPM;
            AudioManager.instance.ChangeBPM(this, prevBPM);
        }

        public void ChangeMaxDistance(float maxDist){
            AudioManager.instance.ChangeMaxDistance(eventInstance, maxDist);
        }

        public void ChangeMinDistance(float minDist){
            AudioManager.instance.ChangeMinDistance(eventInstance, minDist);
        }
    }

    public List<SoundEventClass> soundEvents = new List<SoundEventClass>();

    public static FMODEvents instance {get; private set;}

    void Awake(){
        if(instance != null){
            Debug.LogError("More then one FMOD event manager");
        }
        instance = this;    

        foreach(SoundEventClass soundEvent in soundEvents){
            soundEvent.FirstUpdate();
        }
    }

    void Update(){
        foreach(SoundEventClass soundEvent in soundEvents){
            soundEvent.UpdateSound();
        }
    }
}
