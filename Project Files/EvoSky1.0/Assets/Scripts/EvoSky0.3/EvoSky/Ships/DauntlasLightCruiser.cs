///////////////////////////////////////////////////
//                                               //
//        Copyright Richard Lucas 2016           //
//                                               //
///////////////////////////////////////////////////


using UnityEngine;
using System.Collections;

using EvoSky;

public class DauntlasLightCruiser : AbsShip
{
    public override void Start()
    {
        // Set up the ship
        Initialise("Dauntlas Light Cruiser", 100, 100, 1.25f, 10);
        ShipAudio = GetComponent<AudioSource>();
        ExplodeAudio = (AudioClip)Resources.Load("explode");
        HealthHitAudio = (AudioClip)Resources.Load("Health hit");
        ShieldHitAudio = (AudioClip)Resources.Load("Shield hit");
    }
}
