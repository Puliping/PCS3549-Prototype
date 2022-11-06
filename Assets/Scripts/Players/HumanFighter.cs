using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanFighter : Player
{
    public LayerMask enemyLayers;

    // Skill variables
    public float baseSkillDamage = 1f;
    public float baseSkillCD = 12f;
    public float baseSkillStunDuration = 1.5f;
    public float baseSkillRange = 1f;
    public GameObject skillSprite;
    public float baseSkillAniamtionDuration = 1f;

    public override void OnSkill()
    {
        if (canUseSkill)
        {
            Debug.Log("OnSkill");
            StartCoroutine(SkillCooldown(baseSkillCD));
            StartCoroutine(AnimateSkill());

            // Detect hits
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, baseSkillRange, enemyLayers);
            foreach (Collider2D target in hits)
            {
                // Get only components on the Enemy layer
                Enemy enemyComponent = target.GetComponentInChildren<Enemy>();

                // Deal damage and stun
                // enemyComponent.ReceiveDamage(baseSkillDamage);
                // enemyComponent.ReceiveEffect(baseSkillDamage);
                Debug.Log("hit " + target.name);
            }
        }
    }

    // Placeholder "animation"
    public IEnumerator AnimateSkill()
    {
        skillSprite.transform.position = transform.position;
        skillSprite.SetActive(true);
        yield return new WaitForSeconds(baseSkillAniamtionDuration);
        skillSprite.SetActive(false);
        Debug.Log("AnimateSkill");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, baseSkillRange);
    }
}
