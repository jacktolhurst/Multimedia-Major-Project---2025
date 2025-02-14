using UnityEngine;
using System.Collections.Generic;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get; private set;}

    private List<EventInstance> eventInstances = new List<EventInstance>();

    void Awake(){
        if(instance != null){
            Debug.LogError("More then one audio manager");
        }
        instance = this;
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos){
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference){
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp(){
        foreach(EventInstance eventInstance in eventInstances){ 
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    void OnDestroy(){
        CleanUp();
    }
}
