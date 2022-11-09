using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookGuy : Player
{
    [SerializeField] public LayerMask hookableLayers;
    [SerializeField] public LayerMask enemyLayers;
    [SerializeField] public LineRenderer lr;
    [SerializeField] private MovementController mc;

    // Skill variables
    public float baseSkillCD = 8f;
    public float baseMaxSkillRange = 10f;
    public float baseSkillHitDamage = 1f;
    public float baseSkillContactDamage = 1f;
    public float baseSkillHookSpeed = 10f;
    public float baseSkillHookShotSpeed = 10f;
    public Vector2 hitLocation;
    private bool pulling = false;
    private Enemy enemyHit;
    private float baseMovSpeed;

    public void Update()
    {
        if (pulling)
        {
            // Update current position based on hook movement
            transform.position = Vector2.Lerp(transform.position, hitLocation, baseSkillHookSpeed * Time.deltaTime);
            lr.SetPosition(0, transform.position);

            // Final destination reached
            if (Vector2.Distance(transform.position, hitLocation) < 0.5f)
            {
                // Return speed to old value
                movSpeed = baseMovSpeed;
                mc.UpdatePlayerSpeed();

                pulling = false;
                lr.enabled = false;

                // Deal damage to the enemy
                if (enemyHit != null)
                {
                    // enemyHit.ReceiveDamage(baseSkillContactDamage);
                    Debug.Log("hit " + enemyHit.name);
                }
                // Reset enemy hit by hook
                enemyHit = null;
            }
        }
    }

    public override void OnSkill()
    {
        if (canUseSkill)
        {
            Debug.Log("OnSkill");
            StartCoroutine(SkillCooldown(baseSkillCD));
            // Raycast to aim position
            RaycastHit2D hit = Physics2D.Raycast(transform.position, aimPosition, baseMaxSkillRange, hookableLayers);
            if (hit.collider != null)
            {
                hitLocation = hit.point;
                lr.enabled = true;
                lr.positionCount = 2;

                // Find if it's an enemy
                enemyHit = hit.collider.GetComponentInChildren<Enemy>();
                // enemyHit.ReceiveDamage(baseSkillHitDamage);
                Debug.Log("hit " + enemyHit.name);

                StartCoroutine(Hook());
            }
        }
    }

    // Reference: https://youtu.be/idiq5WjCAD8
    IEnumerator Hook()
    {
        // Update player speed to 0 while hook is active
        baseMovSpeed = movSpeed;
        movSpeed = 0;
        mc.UpdatePlayerSpeed();

        float currentTime = 0;
        float maxHookingTime = 10;

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);

        Vector2 newPos;

        for (; currentTime < maxHookingTime; currentTime += baseSkillHookShotSpeed * Time.deltaTime)
        {
            // I don't know if this adds momentum or not. Probably not
            newPos = Vector2.Lerp(transform.position, hitLocation, currentTime / maxHookingTime);
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, newPos);
            yield return null;
        }

        lr.SetPosition(1, hitLocation);
        pulling = true;
    }
}
