///////////////////////////////////////////////////
//                                               //
//        Copyright Richard Lucas 2016           //
//                                               //
///////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using EvoSky;

public class AiPlayer : AbsPlayer
{
    #region Variables
    AbsPlayer Player;

    /// The distance between the player and the AI ship
    public float distToPlayer_;

    /// the distance between this AI ship and the nearest other AI ship
    public float distToNearestAi_;

    /// The AIs playstyle
    public AiPlayStyle playStyle_;

    System.Random rng;
    #endregion
    #region Methods
    private void Start()
    {
        Player = FindObjectOfType<HuPlayer>();
        ship_ = gameObject.AddComponent<DauntlasLightCruiser>() as AbsShip;

        ship_.missile_ = ((GameObject)Resources.Load("Missile")).transform;
        ship_.bullet_ = ((GameObject)Resources.Load("Bullet")).transform;

        ship_.Start();
        shipName_ = ship_.name_;

        RandomisePlaystyle();


    }

    private void FixedUpdate()
    {
        // calculate the distance to the player
        CalcPlayerDist();

        // Check what actions are on cooldown
        CheckCoolDowns();

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



        if (playStyle_ == AiPlayStyle.Rush)
        {
            RushStyle();
        }
        else if (playStyle_ == AiPlayStyle.Aggressive)
        {
            AggressiveStyle();
        }
        else if (playStyle_ == AiPlayStyle.Wary)
        {
            WaryStyle();
        }
    }

    public void LookAtPlayer()
    {
        transform.LookAt(Player.transform.position);
        Vector3 tempdir = Player.transform.position - transform.position;

        tempdir.Normalize();
        dir_ = tempdir;
    }

    public void CalcPlayerDist()
    {
        distToPlayer_ = Vector3.Distance(Player.transform.position, transform.position);
    }

    public void FireBullet()
    {
        if (canShootSmall_)
        {
            ShootSmall();
        }
    }

    public void FireMissile()
    {
        if (canShootLarge_)
        {
            ShootLarge();
        }
    }

    public void BoostToPlayer()
    {
        if (canBoost_)
        {
            Boost();
        }
    }

    public void RandomisePlaystyle()
    {
        rng = new System.Random((int)transform.position.x);
        int rn = rng.Next(10);
        if (rn < 5) // Wary style
        {
            playStyle_ = AiPlayStyle.Wary;            
        }
        else if (rn >= 5 && rn < 9) // aggressive style
        {            
            playStyle_ = AiPlayStyle.Aggressive;
        }
        else // Rush style
        {
            playStyle_ = AiPlayStyle.Rush;
        }

    }

    public void RushStyle()
    {
        if (distToPlayer_ < 20)
        {
            LookAtPlayer();
        }
        if (distToPlayer_ < 15)
        {
            FireMissile();
        }
        if (distToPlayer_ < 7.5 && distToPlayer_ > 5)
        {
            BoostToPlayer();
        }
    }

    public void AggressiveStyle()
    {
        if (distToPlayer_ < 20)
        {
            LookAtPlayer();
        }
        if (distToPlayer_ < 15)
        {
            FireBullet();
        }
        if (distToPlayer_ < 7.5)
        {
            FireMissile();
        }
        if (distToPlayer_ < 5 && distToPlayer_ > 4)
        {
            BoostToPlayer();
        }
    }

    public void WaryStyle()
    {
        if (distToPlayer_ < 25)
        {
            LookAtPlayer();
        }
        if (distToPlayer_ < 20f)
        {
            FireBullet();
        }
        if (distToPlayer_ < 7)
        {
            FireMissile();
        }
    }

    public override void Move()
    {
        throw new NotImplementedException();
    }
    public override void DidInput()
    {
        throw new NotImplementedException();
    }
    #endregion
}

public enum AiPlayStyle
{
    /// Stays away from other ships
    LoneWolf,
    /// Stays near other ships
    TeamPlayer,
    /// Stays away from the Player - maybe a set distance
    Wary,
    /// Gets close to the player - within a set distance
    Aggressive,
    /// Trys to ram the player using boost
    Rush
}