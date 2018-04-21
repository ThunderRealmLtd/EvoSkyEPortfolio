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

public class HuPlayer : AbsPlayer
{
    #region Variables
    public string defaultShip_ = "DauntlasLightCruiser";

    public AudioSource AudioSource_;
    public AudioClip GameStartSound;
    public AudioClip BoostAudio;
    public AudioClip FireBulletAudio;
    public AudioClip FireMissileAudio;
    public AudioClip FlipDodgeAudio;
    public AudioClip RollDodgeAudio;
    public AudioClip LevelUpAudio;
    public AudioClip ShieldsAreDownAudio;
    public AudioClip halfhp;


    public AudioSource MovementAudioSource_;

    bool shieldUp_ = true;
    bool warningGiven_ = false;
    #endregion
    #region Methods
    private void Start()
    {
        ship_ = gameObject.AddComponent<DauntlasLightCruiser>() as AbsShip;

        ship_.missile_ = ((GameObject)Resources.Load("Missile")).transform;
        ship_.bullet_ = ((GameObject)Resources.Load("Bullet")).transform;

        ship_.Start();
        shipName_ = ship_.name_; // TODO: for debugging remove later
        ship_.IsPlayerShip_ = true;

        AudioSource_.PlayOneShot(GameStartSound, 0.5f);
    }

    private void FixedUpdate()
    {
        // Move the player around the environment
        // based on inputs from the controller
        Move();

        // Did the player press any buttons
        DidInput();

        // Check what actions are on cooldown
        CheckCoolDowns();

        // Check if we can upgrade
        canUpgrade_ = CheckUpgradeStatus();

        // Check if we can boost
        canBoost_ = CheckBoostStatus();

        // If the player is idle
        if (subState_ == PlayerSubState.Idle)
        {
            // for testing the boosting
            // TODO: use boost regen
            boostRemaining_++;
            if (boostRemaining_ > maxBoost_)
            {
                boostRemaining_ = maxBoost_;
            }
        }
        // If not, is the player dodging
        else if (subState_ == PlayerSubState.Dodging)
        {
            IsDodging();
        }
        // If not, is the player is boosting
        else if (subState_ == PlayerSubState.Boosting)
        {
            IsBoosting();
        }

        if (shieldUp_)
        {
            if (ship_.sp_ <= 0)
            {
                shieldUp_ = false;
                AudioSource_.PlayOneShot(ShieldsAreDownAudio, 0.5f);
            } 
        }
        if (!warningGiven_)
        {
            if (ship_.hp_ < 50)
            {
                warningGiven_ = true;
                AudioSource_.PlayOneShot(halfhp, 0.5f);
            } 
        }

    }

    override public void Move()
    {
        // Get the amount to move the players ship
        float moveHorizontal = Input.GetAxis("LeftStickX");
        float moveVertical = Input.GetAxis("LeftStickY");

        if (moveHorizontal != 0 || moveVertical != 0)
        {
            if (!MovementAudioSource_.isPlaying)
            {
                MovementAudioSource_.Play();
            }
        }
        else
        {
            MovementAudioSource_.Stop();
        }

        Vector3 tempPos = new Vector3(moveHorizontal, 0.0f, moveVertical);

        tempPos.Normalize();
        tempPos *= ship_.shipSpeed_;
        tempPos += transform.position;

        // Move the player to that position
        transform.position = tempPos;

        // Get the amount to rotate the players Ship
        float rotateHorizontal = Input.GetAxis("RightStickX");
        float rotateVertical = Input.GetAxis("RightStickY");

        if (rotateHorizontal != 0 || rotateVertical != 0)
        {
            // Change the players the direction
            dir_ = new Vector3(rotateHorizontal, 0.0f, rotateVertical);
            dir_.Normalize();

            // If there is rotation to do then rotate the ship
            transform.LookAt(transform.position + dir_);

            // Shoot the small barrel in the direction we're facing
            if (canShootSmall_)
            {
                AudioSource_.PlayOneShot(FireBulletAudio, 0.05f);
                ShootSmall(); 
            } 
        }
    }

    override public void DidInput()
    {
        // Check if we can perform an action
        if (subState_ == PlayerSubState.Idle)
        {
            // Which Button Was pressed
            if (Input.GetButtonDown("Boost"))
            {
                // Check for if we can perform this action
                if (canBoost_)
                {
                    AudioSource_.PlayOneShot(BoostAudio, 1.0f);
                    Boost(); 
                }
            }
            else if (Input.GetButtonDown("ShootMissile"))
            {
                // Check for if we can perform this action
                if (canShootLarge_)
                {
                    AudioSource_.PlayOneShot(FireMissileAudio, 0.5f);
                    ShootLarge(); 
                }
            }
            else if (Input.GetButtonDown("FlipDodge"))
            {
                // Check for if we can perform this action
                if (canDodge_)
                {
                    AudioSource_.PlayOneShot(FlipDodgeAudio, 1.0f);
                    DodgeFlip(); 
                }
            }
            else if (Input.GetAxis("RollDodgeLeft") < 0 || Input.GetAxis("RollDodgeRight") > 0)
            {
                // Check for if we can perform this action
                if (canDodge_)
                {
                    AudioSource_.PlayOneShot(RollDodgeAudio, 1.0f);
                    // Which roll Dodge do we want to perform
                    if (Input.GetAxis("RollDodgeRight") > 0)
                    {
                        DodgeRoll(true);
                    }
                    else
                    {
                        DodgeRoll(false);
                    } 
                }
            } 
        }
        if (Input.GetButtonDown("Upgrade"))
        {
            // Check for if we can perform this action
            if (canUpgrade_)
            {
                AudioSource_.PlayOneShot(LevelUpAudio, 1.0f);
                Upgrade();
            }
            else
            {
                DidUpgrade();
            }
        }
    }
    #endregion
}