using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMaster : Player
{
    List<Weapon> weaponList;
    private int currentWeaponNumber = 0;
    private int currentCharges = 0;
    private int usedCharges = 0;
    private int currentChargesForNextWeapon = 0;
    private int maxWeakCharges = 3;
    private int maxMediumCharges = 2;
    private int maxStrongCharges = 1;
    private int maxTotalCharges => maxWeakCharges + maxMediumCharges + maxStrongCharges;

    private float weakChargeMultiplier = 1.1f;
    private float mediumChargeMultiplier = 1.3f;
    private float strongChargeMultiplier = 1.6f;

    protected override void Start()
    {
        base.Start();
        // TODO: this doesn't work
        weaponList.Add(weapon);
    }

    public override void OnSkill()
    {
        if (canUseSkill)
        {
            nextWeapon();
        }
    }

    protected override void OnAttack()
    {
        // Set attackMultiplier to new value for the next attack
        attackMultiplier = getNextAttackMultiplier();
        Debug.Log("Current attack multiplier: " + attackMultiplier);
        base.OnAttack();
        HitEnemy();

        // Reset attackMultiplier
        attackMultiplier = 1f;
    }

    public override void HitEnemy()
    {
        base.HitEnemy();
        if (currentChargesForNextWeapon <= maxTotalCharges)
        {
            currentChargesForNextWeapon++;
        }
    }

    private float getNextAttackMultiplier()
    {
        // Used all charges
        if (usedCharges >= currentCharges)
        {
            return 1f;
        }
        // Next charge is weak
        else if (usedCharges <= maxWeakCharges)
        {
            usedCharges++;
            return weakChargeMultiplier;
        }
        // Next charge is medium
        else if (usedCharges <= maxWeakCharges + maxMediumCharges)
        {
            usedCharges++;
            return mediumChargeMultiplier;
        }
        // Next charge is strong
        else
        {
            usedCharges++;
            return strongChargeMultiplier;
        }
    }

    public void nextWeapon()
    {
        // Makes the list loop back to the first element if it's at its end
        currentWeaponNumber = (currentWeaponNumber + 1) % weaponList.Count;

        // I'm pretty sure this doesn't work like this
        weapon = weaponList[currentWeaponNumber];

        // Set accumulated charges for the current weapon and reset charges for the next
        currentCharges = currentChargesForNextWeapon;
        currentChargesForNextWeapon = 0;
    }
}
