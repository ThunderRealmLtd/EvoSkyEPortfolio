///////////////////////////////////////////////////
//                                               //
//        Copyright Richard Lucas 2016           //
//                                               //
///////////////////////////////////////////////////


using UnityEngine;
using System.Collections;

using EvoSky;

public class CutlasHeavyDestroyer : AbsShip
{
    public override void Start()
    {
        // Set up the ship
        Initialise("Cutlas Heavy Destroyer", 200, 50, 0.75f, 20);
        ShipAudio = GetComponent<AudioSource>();
        ExplodeAudio = (AudioClip)Resources.Load("explode");
        HealthHitAudio = (AudioClip)Resources.Load("Health hit");
        ShieldHitAudio = (AudioClip)Resources.Load("Shield hit");
    }
}