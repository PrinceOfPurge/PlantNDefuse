using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity; 

public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference Music { get; private set; }
    
    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }
    
    [field: Header("FasterBomb")]
    [field: SerializeField] public EventReference FasterBomb { get; private set; }    
    
    [field: Header("BombTicking")]
    [field: SerializeField] public EventReference BombTicking { get; private set; }
    
    [field: Header("BombExplosion")]
    [field: SerializeField] public EventReference BombExplosion { get; private set; }
    
    [field: Header("Dog Whines")]
    [field: SerializeField] public EventReference DogWhine { get; private set; }
    
    [field: Header("Wire")]
    [field: SerializeField] public EventReference Wire { get; private set; }
    
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

