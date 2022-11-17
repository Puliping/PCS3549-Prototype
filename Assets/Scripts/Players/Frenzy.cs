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
    private Queue<Coroutine> chargeCoroutineQueue;
    private bool enemyHitOnAttack = true;
    private int hitsNeededForCharge = 3;
    private int currentHits = 0;
    private int currentCharges = 0;
    private int maxCharges = 10;
    private float chargeUptime = 5f;

    public override void OnSkill()
    {

        if (canUseSkill)
        {
            Debug.Log("OnSkill");
            StartCoroutine(SkillCooldown(baseSkillCD));
        }
    }

    public override void OnAttack()
    {
        base.OnAttack();
        HitEnemy();
    }

    public override void HitEnemy()
    {
        base.HitEnemy();
        // TODO: lógica de atingir inimigo precisa estar feita para atualizar
        if (enemyHitOnAttack)
        {
            if (++currentHits == hitsNeededForCharge)
            {
                if (currentCharges >= maxCharges)
                {
                    // refresh oldest charge
                    Debug.Log("Frenzy: refresh oldest charge");
                    StopCoroutine(chargeCoroutineQueue.Peek());
                    removeOldestCharge();
                }
                currentHits = 0;
                Coroutine newCharge = StartCoroutine(Charge());
                chargeCoroutineQueue.Enqueue(newCharge);
            }
        }
        Debug.Log("Frenzy: currently has " + currentCharges  + " charges");
        Debug.Log("Frenzy: currentHits = " + currentHits);
    }

    IEnumerator Charge()
    {
        Debug.Log("Frenzy: new charge");
        currentCharges++;
        UpdateModifiers();
        yield return new WaitForSeconds(chargeUptime);
        removeOldestCharge();
    }

    void removeOldestCharge()
    {
        Debug.Log("Frenzy: oldest charge removed");
        if (currentCharges >= 0)
        {
            currentCharges--;
            chargeCoroutineQueue.Dequeue();
            UpdateModifiers();
        }
        else
        {
            Debug.LogError("Frenzy com cargas negativas");
        }
    }

    void UpdateModifiers()
    {
        movSpeedMultiplier = currentCharges * baseMovSpeedMultiplierPerCharge;
        attackMultiplier = currentCharges * baseAttackMultiplierPerCharge;
    }
}
