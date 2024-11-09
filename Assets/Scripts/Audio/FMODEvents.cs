using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity; 

public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference Music { get; private set; }
   
    [field: Header("Dialogue")]
    [field: SerializeField] public EventReference Dialogue { get; private set; }
    
    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }
    
    [field: Header("doorOpen")]
    [field: SerializeField] public EventReference doorOpen { get; private set; }    
    
    [field: Header("doorClose")]
    [field: SerializeField] public EventReference doorClose { get; private set; }
    
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one AudioManager in scene.");
        }
        instance = this;
    }
}

