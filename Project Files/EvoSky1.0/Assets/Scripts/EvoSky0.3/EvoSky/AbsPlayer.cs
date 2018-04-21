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

public abstract class AbsPlayer : MonoBehaviour
{
    AbsProjectile EnemyProj;

    #region Variables
    #region Core Variables
    /// Player's Statuses
    public PlayerMainState mainState_ = PlayerMainState.Alive;
    /// The player's current actions
    public PlayerSubState subState_ = PlayerSubState.Idle;       
    /// The player's Dodging state
    public PlayerDodgeState dodgeState_ = PlayerDodgeState.None;
        
    /// The direction the player is faceing
    public Vector3 dir_;
    /// The ship the player is using
    public AbsShip ship_;
    /// The ships name
    public string shipName_;
    /// Upgrade points
    public int scrap_ = 0;

    /// Player level
    public int level_ = 0;
    /// The amount of scrap needed to level up
    public int[] scrapToLevel = {100, 200, 300, 400, 500, 600 };

    /// Booleans for control of the available actions
    public bool canBoost_ = true;
    public bool canDodge_ = true;
    public bool canUpgrade_ = false;
    public bool canShootLarge_ = true;
    public bool canShootSmall_ = true;
    /// Boolean for checking if the player takes damage
    public bool invulnerable_ = false;

    /// Boolean for if the player has an active pick-up
    public bool hasPickUp_ = false;
    /// The player's current Pick up
    public PickUp currentPickUp_;
    #endregion
    #region Action Variables
    /// The start time for the action
    protected float actionStartTime_;
    /// The time the player has been performing an action for
    public float actionTime_ = 0;

    /// The startpoint for the action
    protected Vector3 actionStartPoint_;
    /// The endpoint for the action
    protected Vector3 actionEndPoint_;
        
    /// The time that the current action will be off cooldown
    public float coolDownTime_;
    /// The time that firing missiles will be off cooldown
    public float largeFireCoolDownTime_;
    /// Seconds to wait until we can fire the next burst of bullets
    public float smallFireCD_ = 0.2f;
    /// The time the player has been waiting to fire the next burst of bullets
    protected float shootSmallCoolDownTime_;
    /// Seconds to wait until we can fire the next missile
    public float largeFireCD_ = 5.0f;

    public float largeFireTimeElapsed_ = 0.0f;
    #endregion
    #region Dodge Variables
    /// The starting angle
    protected Vector3 startAngle_;
    /// The destination angle
    protected Vector3 destAngle_;

    /// The distance to dodgeRoll
    public float dodgeDist_ = 25;
    /// Seconds to wait between roll dodges
    public float dodgeRollCD_ = 5.0f;
    /// Duration of roll dodge
    public float dodgeRollDur_ = 0.75f;

    /// Seconds to wait between flip dodges
    public float dodgeFlipCD_ = 5.0f;
    /// Duration of flip dodge
    public float dodgeFlipDur_ = 1.0f;

    /// Dodge time elapsed
    public float dodgeTimeElapsed_ = 0.0f;
    #endregion
    #region Boost variables
    /// The maximum boost allowed
    public int maxBoost_ = 250;
    /// How much boost the player has left
    public int boostRemaining_ = 0;
    /// Minimum boost to boost
    public int minBoostToBoost_ = 25;
    /// Boost to remove per use
    public int boostPer_ = 25;
    /// Distance to boost
    public float boostDist_ = 35;
    /// How long the boost takes
    public float boostDur_ = 0.5f;

    /// the boost particles to use
    public Transform BoostParticles;
    #endregion
    #endregion
    #region Methods
    /// Move the players ship
    /// for HuPlayer use controller
    /// for AiPlayer use AI
    abstract public void Move();

    /// Manages the players inputs - for Ai use decision making stuff
    abstract public void DidInput();

    /// Check what actions are on cooldown
    /// if they have cooled down reset them
    public void CheckCoolDowns()
    {
        if (!canShootLarge_)
        {
            if (largeFireCoolDownTime_ <= Time.time)
            {
                canShootLarge_ = true;
                largeFireTimeElapsed_ = 0.0f;
            }
            else
            {
                largeFireTimeElapsed_ += Time.deltaTime;
            }
        }
        if (!canShootSmall_)
        {
            if (shootSmallCoolDownTime_ <= Time.time)
            {
                canShootSmall_ = true;
            }
        }
        if (!canDodge_)
        {
            if (coolDownTime_ <= Time.time)
            {
                canDodge_ = true;
                dodgeTimeElapsed_ = 0;
            }
            else
            {
                dodgeTimeElapsed_ += Time.deltaTime;
            }
        }
    }
    #region Boosting Methods
    /// Start boosting
    public void Boost()
    {
        // Change the player's substate so nothing happens more than its meant to
        subState_ = PlayerSubState.Boosting;
        // Set the canboost_ Boolean
        canBoost_ = false;
        // The player can't take damage during the boost
        invulnerable_ = true;
        // Remove the player's boost
        boostRemaining_ -= boostPer_;

        // Get the start time for the dodge
        actionStartTime_ = Time.time;
        // Get the startpoint for the dodge
        actionStartPoint_ = transform.position;

        // Set the endPoint
        Vector3 AddPos = transform.forward * boostDist_;
        actionEndPoint_ = actionStartPoint_ + AddPos;

        // Add the particle effect
        Transform BP = (Transform)Instantiate(BoostParticles, transform.FindChild("Barrel").position, Quaternion.Euler(0, 180, 0));
        BP.transform.parent = transform;
    }
    /// While we are boosting
    public void IsBoosting()
    {
        // Update the actionTime_
        actionTime_ += Time.fixedDeltaTime;
        // A check in case the timer is bigger than the duration
        if (actionTime_ > boostDur_)
        {
            actionTime_ = boostDur_;
        }
        // Perform the lerp
        transform.position = Vector3.Lerp(actionStartPoint_, actionEndPoint_, actionTime_ / boostDur_);
        // If we've reached the end of the Lerping
        if (actionTime_ / boostDur_ == 1)
        {
            DidFinishBoost();
        }
    }
    /// Once we are done boosting
    public void DidFinishBoost()
    {
        // Reset the player's status
        subState_ = PlayerSubState.Idle;
        // Reset the actionTime_
        actionTime_ = 0;
        // The player can now take damage again
        invulnerable_ = false;

        //remove the particle effect
        Destroy(GameObject.Find("boostparticles(Clone)"));
    }
    /// Can the player Boost
    public bool CheckBoostStatus()
    {
        if (boostRemaining_ > minBoostToBoost_)
        {
            return true;
        }
        return false;
    }        
    #endregion
    #region Dodging Methods
    /// Start dodging on the spot
    public void DodgeFlip()
    {
        // Change the player's substate so nothing happens more than its meant to
        subState_ = PlayerSubState.Dodging;
        // Change the players Dodge State
        dodgeState_ = PlayerDodgeState.Flip;
        // Set the canDodge_ Bollean
        canDodge_ = false;
        // The player cannot take Damage while Dodging
        invulnerable_ = true;

        // Set the coolDownTime_
        coolDownTime_ = Time.time + dodgeFlipCD_;
        // Get the start time for the dodge
        actionStartTime_ = Time.time;
        // Get the start angle
        startAngle_ = transform.eulerAngles;

        // Set the dest angle
        destAngle_ = new Vector3(-360, 0, 0) + startAngle_;
    }
    /// Rolls to a side - side take as a parameter
    public void DodgeRoll(bool IsLeft)
    {
        // Change the player's substate so nothing happens more than its meant to
        subState_ = PlayerSubState.Dodging;
        // Change the players dodge state
        dodgeState_ = IsLeft ? PlayerDodgeState.LeftRoll : PlayerDodgeState.RightRoll;
        // Set the canDodge_ Boolean
        canDodge_ = false;
        // The player cannot take damage while Dodging
        invulnerable_ = true;

        // Set the coolDownTime_
        coolDownTime_ = Time.time + dodgeRollCD_;
        // Get the start time for the dodge
        actionStartTime_ = Time.time;
        // Get the Startpoint for the dodge
        actionStartPoint_ = transform.position;
        // Get the start angle
        startAngle_ = transform.eulerAngles;

        // Set the endpoint for the dodge
        Vector3 AddPos = IsLeft ? -transform.right : transform.right;
        AddPos *= dodgeDist_;
        actionEndPoint_ = transform.position + AddPos;
        // Set the destination angle
        destAngle_ = IsLeft ? new Vector3(0, 0, 360) : new Vector3(0, 0, -360);
        destAngle_ += startAngle_;
    }
    /// Depending on the dodge state perform the dodging action
    public void IsDodging()
    {
        if (dodgeState_ == PlayerDodgeState.Flip)
        {
            // Update the actionTime_
            actionTime_ += Time.fixedDeltaTime;

            // A check in case the timer is bigger than the duration
            if (actionTime_ > dodgeFlipDur_)
            {
                actionTime_ = dodgeFlipDur_;
            }

            // Perform the lerp
            transform.eulerAngles = Vector3.Lerp(startAngle_, destAngle_, actionTime_/dodgeFlipDur_);
                
            // If we've reached the end of the Lerping
            if (actionTime_ / dodgeFlipDur_ == 1)
            {
                DidFinishDodge();
            }
        }
        else /// Roll Dodge
        {
            // Update the actionTime_
            actionTime_ += Time.fixedDeltaTime;

            // A check in case the timer is bigger than the duration
            if (actionTime_ > dodgeRollDur_)
            {
                actionTime_ = dodgeRollDur_; 
            }

            // Perform the Lerp
            transform.position = Vector3.Lerp(actionStartPoint_, actionEndPoint_, actionTime_/ dodgeRollDur_);
            transform.eulerAngles = Vector3.Lerp(startAngle_, destAngle_, actionTime_ / dodgeRollDur_);

            // If we've reached the end of the Lerping
            if (actionTime_ / dodgeRollDur_ == 1 )
            {
                DidFinishDodge();
            }
        }
    }
    /// Once we are done dodging
    public void DidFinishDodge()
    {
        // Reset the player's statuses
        subState_ = PlayerSubState.Idle;
        dodgeState_ = PlayerDodgeState.None;
        // Reset the actionTime_
        actionTime_ = 0;
        // The player can now take damage again
        invulnerable_ = false;
    }
    #endregion
    #region Shooting Methods
    /// Fires the small cannon - Bullets
    public void ShootSmall()
    {
        Debug.DrawLine(transform.position, transform.position + (dir_ * 100), Color.cyan, 5.0f);

        // Set the canShootLarge_ boolean
        canShootSmall_ = false;

        // Set the TimeStamp
        shootSmallCoolDownTime_ = Time.time + smallFireCD_;

        // TODO: instantiate the correct number of bullets

        // Set the position to instantiate at
        Vector3 Pos = transform.FindChild("Barrel").position;

        // create the new bullet
        Transform bulletToFire = ship_.bullet_;

        //set default variables
        bulletToFire.GetComponent<AbsBullet>().SetUp(this, dir_, ship_.bulletType_);

        ((Transform)Instantiate(bulletToFire, Pos, Quaternion.identity)).LookAt(transform.position + (dir_ * 100));
    }

    /// Fires the large cannon - Missile
    public void ShootLarge()
    {
        Debug.DrawLine(transform.position, transform.position + (dir_ * 100), Color.white, 5.0f);

        // Set the canShootLarge_ boolean
        canShootLarge_ = false;

        // Set the TimeStamp
        largeFireCoolDownTime_ = Time.time + largeFireCD_;

        // Set the position to instantiate at
        Vector3 Pos = transform.FindChild("Barrel").position;

        // create the new bullet
        Transform missileToFire = ship_.missile_;

        //set deafault variables
        missileToFire.GetComponent<AbsMissile>().SetUp(this, dir_, ship_.missileType_);

        ((Transform)Instantiate(missileToFire, Pos, Quaternion.identity)).LookAt(transform.position + (dir_ * 100));
    }

    /// when the playyer collides with an object
    public virtual void OnCollisionEnter(Collision collision)
    {
        if (!invulnerable_)
        {
            if (collision.gameObject.tag == "Missile" || collision.gameObject.tag == "Bullet")
            {
                EnemyProj = collision.gameObject.GetComponent<AbsProjectile>();

                // if this isnt the bullets owner
                if (this != EnemyProj.owner_)
                {
                    //Debug.Log("THERE WAS A COLLISION from " + name);
                    //Debug.Log(name + " hit by " + collision.gameObject.name);

                    if (!invulnerable_)
                    {
                        if (!EnemyProj.ap_)
                        {
                            ship_.TakeDamage(EnemyProj.dmg_, EnemyProj.owner_);
                        }
                        else
                        {
                            ship_.TakeAPDamage(EnemyProj.dmg_, EnemyProj.owner_);
                        }
                    }
                }
            }
            else if (collision.gameObject.tag == "AiPlayer" || collision.gameObject.tag == "HuPlayer")
            {
                Debug.Log(this.name + " has been hit by " + collision.gameObject.name);
                ship_.TakeDamage(10, collision.gameObject.GetComponent<AbsPlayer>());
            }
        }
    }
    #endregion
    #region Upgrade methods
    /// Start Upgrading
    public void Upgrade()
    {
        // Change the player's substate so nothing happens more than its meant to
        //subState_ = PlayerSubState.Upgrading;

        // Set the canUpgrade boolean
        canUpgrade_ = false;

        // TODO: implement an upgrade menu screen

        level_++;

        // Swap the palyers Ship
        Destroy(ship_);
        ship_ = gameObject.AddComponent<CutlasHeavyDestroyer>() as AbsShip;

        ship_.missile_ = ((GameObject)Resources.Load("Missile")).transform;
        ship_.bullet_ = ((GameObject)Resources.Load("Bullet")).transform;

        ship_.Start();
        ship_.bulletType_ = ProjTypes.APRound;
        ship_.missileType_ = ProjTypes.PhotonTorpedo;
        ship_.IsPlayerShip_ = true;

        shipName_ = ship_.name_;

        transform.localScale = new Vector3(level_, level_, level_);

    }
    /// Can the player Upgrade
    public bool CheckUpgradeStatus()
    {
        if (level_ < scrapToLevel.Count())
        {
            // If the player's current scrap is greater than the threshold to level up
            if (scrap_ >= scrapToLevel[level_])
            {
                return true; 
            }
        }

        return false;
    }
    /// Finished Upgrading
    public void DidUpgrade()
    {
        // Change the player's substate so nothing happens more than its meant to
        subState_ = PlayerSubState.Idle;
    }

    #endregion
    #endregion
}