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

public class AbsBullet : AbsProjectile
{
    #region Variables

    #endregion
    #region Methods
    private void Start()
    {
        // Destroy the object after its lifetime expires
        Destroy(this.gameObject, lifeTime_);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    /// Set Up each different type of Bullet this could be
    public override void Initialise()
    {
        // Set up the bullet class further
        if (projType_ == ProjTypes.StdSlug)
        {
            name_ = "StdSlug";
            projSpeed_ = 10f;
            dmg_ = 10;
        }
        else if (projType_ == ProjTypes.LazerPulse)
        {
            name_ = "LazerPulse";
            projSpeed_ = 50f;
            dmg_ = 10;
            ap_ = true;
        }
    }
    #endregion
}