using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0,1)]
    
    public float masterVolume = 1;
    [Range(0,1)]
    
    public float musicVolume = 1;
    [Range(0,1)]
    
    public float ambienceVolume = 1;
    [Range(0,1)]
    
    public float SFXVolume = 1;
    
    private Bus masterBus;
    private Bus musicBus;
    private Bus ambienceBus;
    private Bus sfxBus;
    
    private List<EventInstance> eventInstances; 
    private List<StudioEventEmitter> eventEmitters;
    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one AudioManager in scene.");
        }
        instance = this;

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
        
        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.Music);
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        ambienceBus.setVolume(ambienceVolume);
        sfxBus.setVolume(SFXVolume);
    }
    
    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }
    

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
    
    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference); 
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }
    
    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
