using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Frenzy : Player
{
    public float baseSkillCD = 10f;

    // Charge variables
    private bool enemyHitOnAttack;
    private int hitsNeededForCharge = 3;
    private int currentHits = 0;
    private int currentCharges = 0;
    private int maxCharges = 10;

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
        // TODO: lógica de atingir inimigo precisa estar feita para atualizar
        if (enemyHitOnAttack)
        {
            if (++currentHits == hitsNeededForCharge)
            {
                if (currentCharges >= maxCharges)
                {
                    // refresh oldest charge
                }
                else
                {
                    currentCharges++;
                }
            }
        }
    }
}
