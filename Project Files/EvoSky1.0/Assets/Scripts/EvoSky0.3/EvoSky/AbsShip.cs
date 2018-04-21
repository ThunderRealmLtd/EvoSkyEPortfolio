///////////////////////////////////////////////////
//                                               //
//        Copyright Richard Lucas 2016           //
//                                               //
///////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

using EvoSky;

public abstract class AbsShip : MonoBehaviour
{
    #region Variables
    public AudioSource ShipAudio;
    public AudioClip ExplodeAudio;
    public AudioClip HealthHitAudio;
    public AudioClip ShieldHitAudio;

    public bool IsPlayerShip_;

    #region Core Variables
    /// Ship name
    public string name_;

    /// Ship size - Increased hit area
    public int size_;

    /// Hit points remaining
    public int hp_;
    public int maxHp_;

    /// Shield points remaining
    public int sp_;
    public int maxSp_;

    /// Movement speed
    public float shipSpeed_;

    /// Damage reduction
    public int armour_;

    /// Boost remaining
    public int boost_;
    public int maxBoost_;

    /// How much scrap the ship is worth when killed - TODO: to be calculated when the HP reaches 0
    public int worth_ = 50;
    #endregion
    #region Combat Variables
    /// Can we be hit by enemy projectiles
    public bool invulnerable_ = false;

    /// Small cannon
    public Transform bullet_;
    public ProjTypes bulletType_ = ProjTypes.StdSlug;

    /// Large cannon
    public Transform missile_;
    public ProjTypes missileType_ = ProjTypes.StdMissile;

    /// The number of bullets to fire at a time - starts at 2
    public int smallBarrels_ = 2;

    /// The number of bullets to fire in a burst
    public int smallBarrelBurst = 2;

    /// The number of missiles to fire at a time - starts at 1
    public int largeBarrels_ = 1; 
    #endregion
    #endregion
    #region Methods
    public virtual void Start() { }

    /// To initialise all the variables
    protected void Initialise(string Name, int HP, int SP, float Speed, int Armour)
    {
        name_ = Name;
        hp_ = HP;
        maxHp_ = HP;
        sp_ = SP;
        maxSp_ = SP;
        shipSpeed_ = Speed;
        armour_ = Armour;
    }

    /// So the ship can take damage
    public void TakeDamage(int Dmg, AbsPlayer DamageDealer)
    {
        // if there is sheild remaining take tha damage to the shield
        if (sp_ > 0)
        {
            sp_ -= Dmg * (armour_/10);
            if (IsPlayerShip_)
            {
                ShipAudio.PlayOneShot((AudioClip)Resources.Load("Shield hit"));
            }
        }
        else
        {
            hp_ -= Dmg * (armour_ / 10);
            if (IsPlayerShip_)
            {
                ShipAudio.PlayOneShot((AudioClip)Resources.Load("Health hit"));
            }
        }

        if (hp_ <= 0)
        {
            
            ShipDestryed(DamageDealer);
        }

    }

    /// So the ship can take armour piercing damage
    public void TakeAPDamage(int Dmg, AbsPlayer DamageDealer)
    {
        hp_ -= Dmg;
        if (IsPlayerShip_)
        {
            ShipAudio.PlayOneShot((AudioClip)Resources.Load("Health hit"));
        }

        if (hp_ <= 0)
        {
            ShipDestryed(DamageDealer);
        }
    }

    /// When the ship is killed
    public virtual void ShipDestryed(AbsPlayer Destroyer)
    {
        if (IsPlayerShip_)
        {
            Debug.Log("Player ded");
            Application.Quit();
            Application.LoadLevel(0);
        }

        FindObjectOfType<HuPlayer>().GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("explode"));
        // Add the ships worht to the destroyers scrap
        Destroyer.scrap_ += worth_;

        // destroy the gameobject
        Destroy(gameObject);
        
    }
    #endregion
}