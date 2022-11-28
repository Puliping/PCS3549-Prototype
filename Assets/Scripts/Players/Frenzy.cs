using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Frenzy : Player
{
    public float baseSkillCD = 10f;
    public float baseMovSpeedMultiplierPerCharge = .05f;
    public float baseAttackMultiplierPerCharge = .01f;

    // Charge variables
    private int hitsNeededForCharge = 1;
    private int currentHits = 0;
    private int maxCharges = 10;
    private int currentChargeCounter = 0;
    private float chargeUptime = 5f;
    // Actual number of charges is hard-capped by maxCharges
    private int currentCharges => Mathf.Min(currentChargeCounter, maxCharges);

    public override void OnSkill()
    {

        if (canUseSkill)
        {
            Debug.Log("OnSkill");
            StartCoroutine(SkillCooldown(baseSkillCD));
            RemoveAllCharges();
        }
    }

    protected override void OnAttack()
    {
        base.OnAttack();
        HitEnemy();
    }

    public override void HitEnemy()
    {
        base.HitEnemy();
        if (++currentHits == hitsNeededForCharge)
        {
            // Just setting an upper bound so the charge number doesn't break anything
            if (currentChargeCounter <= 3 * maxCharges)
            {
                StartCoroutine(Charge());
                currentHits = 0;
            }
        }
        Debug.Log("Frenzy: currently has " + currentChargeCounter + " charges");
        Debug.Log("Frenzy: currentHits = " + currentHits);
    }

    IEnumerator Charge()
    {
        Debug.Log("Frenzy: new charge");
        AddCharge();
        yield return new WaitForSeconds(chargeUptime);
        RemoveCharge();
    }

    void AddCharge()
    {
        currentChargeCounter++;
        UpdateModifiers();
    }

    void RemoveCharge()
    {
        currentChargeCounter--;
        UpdateModifiers();
    }

    void RemoveAllCharges()
    {
        for (int i = 0; i < currentChargeCounter; i++)
        {
            StopCoroutine(Charge());
        }
        currentChargeCounter = 0;
        UpdateModifiers();
    }

    void UpdateModifiers()
    {
        movSpeedMultiplier = currentCharges * baseMovSpeedMultiplierPerCharge;
        attackMultiplier = currentCharges * baseAttackMultiplierPerCharge;
    }
}
