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

public class AbsMissile : AbsProjectile
{
    #region Variables
    /// Does the missile have a target
    public bool hasTarget_ = false;

    /// The target for the missile to follow
    public AbsPlayer target_;

    /// The currently available Targets
    public AbsPlayer[] availableTargets_;

    /// Can the missile be dodged - set to false until upgraded
    public bool cantBeDodged_ = false;

    /// Does firing this missile spawn another missile to fire at a difference target? - Set to false until upgraded
    public bool MultiTarget_ = false;

    /// Does the missile fire and then when within a range break into smaller missiles? - set to false until upgraded
    public bool Scatter_ = false;
    #region Target Calculation Variables
    /// Is that missile Alligned to the target
    public bool targetAlligned_ = false;

    /// The starting angle
    protected Vector3 startAngle_;
    /// The destination angle
    protected Vector3 destAngle_;

    /// Duration of roll dodge
    public float targetAllignDur_ = 0.5f;

    /// The start time for the action
    protected float targetAllignStartTime_;
    /// The time the player has been performing the allignment for
    protected float targetAllignTime_ = 0;

    #endregion
    #endregion
    #region Methods
    /// Choose a target from a range in front of the player
    public void Start()
    {
        hasTarget_ = ChooseTarget();
        if (hasTarget_)
        {
            DidStartAllignment();
        }
    }

    public override void FixedUpdate()
    {
        // make the missile move faster overtime
        projSpeed_ += 0.5f;

        if (hasTarget_)
        {
            // If we arent facing the target yet
            if (!targetAlligned_)
            {
                // Rotate some more towards the target
                IsAlligning();
            } 
        }


        // Update the direction to travel based on the targets location
        base.FixedUpdate();
    }

    /// Chose a target from a range in front of the player
    public bool ChooseTarget()
    {
        // Which targets are we looking at
        AbsPlayer ClosestTarget = null;
        AbsPlayer CurrentTarget;

        //what is the shortest distance so far?
        float shortest = float.MaxValue;


        if (owner_.GetComponent<HuPlayer>() != null)
        {
            // find the Ai Players
            availableTargets_ = FindObjectsOfType<AiPlayer>();
        }
        else
        {
            // Find the Human Players
            availableTargets_ = FindObjectsOfType<HuPlayer>();
        }
        // if there are no targets
        if (availableTargets_.Length == 1)
        {
            return false;
        }

        // if there are
        for (int i = 0; i < availableTargets_.Length; i++)
        {
            CurrentTarget = availableTargets_[i];
            // Make sure the Owner is not one of them
            if (CurrentTarget != owner_)
            {
                // find the distance between the current target and the missile
                float Dist = Vector3.Distance(CurrentTarget.transform.position, transform.position);
                if (Dist < shortest)
                {
                    ClosestTarget = CurrentTarget;
                }
            }
        }
        target_ = ClosestTarget;
        Debug.Log("the target is " + target_.name + " with a " + target_.ship_.name_);
        return true;
    }

    /// Update the direction vector
    public Vector3 CalcTargetVector() // - TODO: not always working 100%
    {
        Vector3 Output = new Vector3(0, 0, 0);
        Vector3 targetVector = transform.InverseTransformPoint(target_.transform.position);

        float angle = Mathf.Atan2(targetVector.x, targetVector.z) * Mathf.Rad2Deg;
        Output = new Vector3(0, angle, 0);

        return Output;
    }

    /// Set Up each different type of Missile this could be
    public override void Initialise()
    {
        // Set up the missile class further
        if (projType_ == ProjTypes.StdMissile)
        {
            name_ = "StdMissile";
            projSpeed_ = 1.0f;
            dmg_ = 100;
        }
        else if (projType_ == ProjTypes.PhotonTorpedo)
        {
            name_ = "PhotonTorpedo";
            projSpeed_ = 5.0f;
            dmg_ = 200;
            ap_ = true;
        }
    }

    #region Target Allignment Methods
    public void DidStartAllignment()
    {
        // Get the start time for the dodge
        targetAllignStartTime_ = Time.time;

        // Get the start angle
        startAngle_ = transform.eulerAngles;

        // Set the dest angle - for testing
        destAngle_ = CalcTargetVector() + startAngle_;
    }

    public void IsAlligning()
    {
        // Update the actionTime_
        targetAllignTime_ += Time.fixedDeltaTime;

        // A check in case the timer is bigger than the duration
        if (targetAllignTime_ > targetAllignDur_)
        {
            targetAllignTime_ = targetAllignDur_;
        }

        // update the direction as we are moving
        destAngle_ = CalcTargetVector() + transform.eulerAngles;

        // Perform the lerp
        transform.eulerAngles = Vector3.Lerp(startAngle_, destAngle_, targetAllignTime_ / targetAllignDur_);

        // If we've reached the end of the Lerping
        if (targetAllignTime_ / targetAllignDur_ == 1)
        {
            DidFinishAllignment();
        }
    }

    public void DidFinishAllignment()
    {
        targetAlligned_ = true;
    }

    #endregion
    #endregion
}