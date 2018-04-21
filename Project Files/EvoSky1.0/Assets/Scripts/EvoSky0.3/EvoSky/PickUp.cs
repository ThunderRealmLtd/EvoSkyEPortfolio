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

public class PickUp : MonoBehaviour
{
    #region Variables
    /// Time in seconds for the pickup to be active
    public int duration_;

    /// The type of pickup picked up
    public PickUpType type_;

    /// what drop % is the pickup
    public int rarity_;

    /// Is the pickup pickup-able?
    public bool canBePickedUp_;
#endregion
    #region Methods
    #endregion
}
public enum PickUpType
{
    /// Makes the player fire 2 more bullets  - and missiles?
    Shotgun,
    /// All bullets kill on hit
    Instakill,
    /// Take no damage but do no extra damage to ships when they collide
    SuperArmour,
    /// crates a circle around the player that reflects bullets back in the direction they came from
    RefelctOrb,
    /// Destroys all ships in an area around the palyer - Or does high damage in an area around the palyer so that late game it doesnt instakill ships
    Nuke
}