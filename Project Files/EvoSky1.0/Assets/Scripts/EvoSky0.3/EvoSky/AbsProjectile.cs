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

public abstract class AbsProjectile : MonoBehaviour
{
    #region Variables
    /// Who fired the projectile
    public AbsPlayer owner_;

    /// Projectile Name
    public string name_;

    /// What type of projectile is this
    public ProjTypes projType_;

    /// How long the Projectile is active
    protected float lifeTime_ = 5.0f;

    /// Speed of the projectile
    public float projSpeed_;

    /// The direction the projectile is moving in
    public Vector3 dir_;

    /// The direction multiplied by the speed
    private Vector3 velocity_;

    /// Damage of the projectile
    public int dmg_;

    /// Does the round pierce armour
    public bool ap_ = false;

    //AbsShip EnemyShip;

    #endregion
    #region Methods
    /// Set up the projectile
    public void SetUp(AbsPlayer Owner, Vector3 Dir, ProjTypes BulletType)
    {
        owner_ = Owner;
        dir_ = Dir;
        projType_ = BulletType;

        Initialise();
    }

    /// Fixed Update
    public virtual void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.fixedDeltaTime * projSpeed_);
    }

    /// Overriden in the base classes - to set up specific variables
    public virtual void Initialise() { }

    /// Updates the projectiles velocity
    protected void UpdateVelocity()
    {
        velocity_ = dir_ * projSpeed_;
        Debug.Log("new velocity is: " + velocity_);
    }

    /// When we hit another object
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Missile" && collision.gameObject.tag != "Bullet")
        {
            if (collision.gameObject.GetComponent<AbsPlayer>() != owner_)
            {
                // Destroy the projectile
                Destroy(gameObject);
            }
        }
    }

    #endregion 
}
public enum ProjTypes
{
    // Bullet Types
    StdSlug,
    APRound,
    LazerPulse,
    // Missile Types
    StdMissile,
    PhoenixMissile,
    PhotonTorpedo,
}