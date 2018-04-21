using UnityEngine;
using System.Collections;
using Image = UnityEngine.UI.Image;

using EvoSky;

public class UIBars : MonoBehaviour
{
    public Image healthBar_;
    public float hpPerCent_ = 1.0f;

    public Image shieldBar_;
    public float spPerCent_ = 1.0f;

    public Image scrapBar_;
    public float scrapPerCent_ = 0.0f;

    public Image boostBar_;
    public float boostPerCent_ = 0.0f;

    public Image missileBar_;
    public float missilePerCent_ = 0.0f;

    public Image sideDodgeBar_;
    public Image flipDodgeBar_;
    public float dodgePerCent_ = 0.0f;

    AbsPlayer player;

    AbsShip playerShip;

	// Use this for initialization
	void Start ()
    {
        healthBar_ = GameObject.Find("Camera").transform.FindChild("Canvas").FindChild("Healthbar").GetComponent<Image>();
        shieldBar_ = GameObject.Find("Camera").transform.FindChild("Canvas").FindChild("Shieldbar").GetComponent<Image>();
        scrapBar_ = GameObject.Find("Camera").transform.FindChild("Canvas").FindChild("Scrapbar").GetComponent<Image>();
        boostBar_ = GameObject.Find("Camera").transform.FindChild("Canvas").FindChild("Boostbar").GetComponent<Image>();

        missileBar_ = GameObject.Find("Camera").transform.FindChild("Canvas").FindChild("Missile").GetComponent<Image>();
        sideDodgeBar_ = GameObject.Find("Camera").transform.FindChild("Canvas").FindChild("Sidedodge").GetComponent<Image>();
        flipDodgeBar_ = GameObject.Find("Camera").transform.FindChild("Canvas").FindChild("Flipdodge").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        player = FindObjectOfType<HuPlayer>();
        playerShip = player.ship_;

        // Set the bars
        SetHpBar();
        SetSpBar();
        SetScrapBar();
        SetBoostBar();

        // Set the ability cooldowns
        SetMissileCD();
        SetDodgeCD();

    }

    // Functions for each bar hp, sp, boost
    void SetHpBar()
    {
        if (playerShip.hp_ != playerShip.maxHp_)
        {
            hpPerCent_ = ((float)playerShip.hp_ / (float)playerShip.maxHp_);
        }

        healthBar_.fillAmount = hpPerCent_;
    }

    void SetSpBar()
    {
        if (playerShip.sp_ != playerShip.maxSp_)
        {
            spPerCent_ = ((float)playerShip.sp_ / (float)playerShip.maxSp_); 
        }

        shieldBar_.fillAmount = spPerCent_;
    }

    void SetScrapBar()
    {
        scrapPerCent_ = ((float)player.scrap_ / (float)player.scrapToLevel[player.level_]);

        scrapBar_.fillAmount = scrapPerCent_;
    }

    void SetBoostBar()
    {
        boostPerCent_ = ((float)player.boostRemaining_ / (float)player.maxBoost_);

        boostBar_.fillAmount = boostPerCent_;
    }

    // cooldowns - radial
    void SetMissileCD()
    {
        if (!player.canShootLarge_)
        {
            missilePerCent_ = 1 - (player.largeFireTimeElapsed_ / player.largeFireCD_);
        }
        else
        {
            missilePerCent_ = 0;
        }

        missileBar_.fillAmount = missilePerCent_;
    }

    void SetDodgeCD()
    {
        if (!player.canDodge_)
        {
            dodgePerCent_ = 1 - (player.dodgeTimeElapsed_ / player.dodgeRollCD_);
        }
        else
        {
            dodgePerCent_ = 0;
        }

        sideDodgeBar_.fillAmount = dodgePerCent_;
        flipDodgeBar_.fillAmount = dodgePerCent_;
    }


    // stat panel? - speed armour bullet type name large and small barrels
}
