using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookGuy : Player
{
    [SerializeField] public LayerMask hookableLayers;
    [SerializeField] public LayerMask enemyLayers;
    [SerializeField] public LineRenderer lr;

    // Skill variables
    private float baseSkillCD = 3f;
    private float baseMaxSkillRange = 9f;
    private float baseSkillHitDamage = 1f;
    private float baseSkillContactDamage = 1f;
    private float baseSkillHookShotSpeed = 10f;
    private float baseMomentumStrengthCoefficient = 0.8f;
    private float baseDashMomentumStrengthCoefficient => baseMomentumStrengthCoefficient * 3.5f;
    private float baseSkillArea = 0.8f;
    private float baseSkillPullingSpeed = 0.85f;
    private float skillCurrentHookSpeed;
    private float skillAcceleratingFactor = 1.004f;
    private Vector2 moveDirectionOnSkill;
    private Vector2 positionOnSkill;
    private Vector2 hitLocation;
    private Vector2 bezierP1;
    private bool pulling = false;
    private bool targetFound = false;
    private Enemy enemyHit;
    private float baseMovSpeed;
    private float currentTime;
    private float maxHookingTime;
    private float maxPullingTime;

    public new void Start()
    {
        base.Start();
        // Layermasks that can be hooked
        hookableLayers = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Walls");
    }

    // Bezier curve with 3 points to add momentum to the 
    public Vector2 Bezier(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        var ab = Vector2.Lerp(a, b, t);
        var bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
    }

    public void Update()
    {
        if (pulling)
        {
            // Update current position based on hook movement
            // Bezier curve takes the position when the skill was activated, P1, final position and a time
            transform.position = Bezier(positionOnSkill, bezierP1, hitLocation, currentTime / maxPullingTime);
            // Time must be updated per frame
            currentTime += Time.deltaTime * skillCurrentHookSpeed;
            skillCurrentHookSpeed *= skillAcceleratingFactor;

            // Set current position as hook starting point
            lr.SetPosition(0, transform.position);

            // Final destination reached
            if (Vector2.Distance(transform.position, hitLocation) < 0.5f)
            {
                // Return speed to old value
                movSpeed = baseMovSpeed;
                movementController.UpdatePlayerSpeed();

                pulling = false;
                lr.enabled = false;

                // Deal damage to the enemies
                // Detect hits
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, baseSkillArea, enemyLayers);
                foreach (Collider2D target in hits)
                {
                    // Get only components on the Enemy layer
                    Enemy enemyComponent = target.GetComponentInChildren<Enemy>();

                    // Deal damage
                    enemyComponent.TakeDamage(baseSkillContactDamage);
                    Debug.Log("hit " + target.name);
                }
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
                targetFound = true;
                hitLocation = hit.point;
                lr.enabled = true;
                // Hook has beginning and end points
                lr.positionCount = 2;

                // Find if it's an enemy
                enemyHit = hit.collider.GetComponentInChildren<Enemy>();
                if (enemyHit != null)
                {
                    enemyHit.TakeDamage(baseSkillHitDamage);
                    Debug.Log("hit " + enemyHit.name);
                }
                // Reset enemy hit by hook
                enemyHit = null;

                // Begin hook itself
                StartCoroutine(Hook());
            }

            // Raycast didn't hit anything
            else
            {
                hitLocation = new(transform.position.x + aimPosition.x * baseMaxSkillRange, transform.position.y + aimPosition.y * baseMaxSkillRange);
                lr.enabled = true;
                lr.positionCount = 2;
                StartCoroutine(Hook());
            }
        }
    }

    protected override void OnDash()
    {
        if (!canDash) return;
        if (!pulling)
        {
            base.OnDash();
        }
        else
        {
            StartCoroutine(HookDash());
        }
    }

    IEnumerator HookDash()
    {
        // disable future dashes, will need to change if dash gets more than one charge
        canDash = false;

        Vector2 dashDirection = aimPosition.normalized;

        // Update Bezier curve point
        bezierP1 = new(transform.position.x + dashDirection.x * baseDashMomentumStrengthCoefficient, transform.position.y + dashDirection.y * baseDashMomentumStrengthCoefficient);

        // Reset some variables so that the curve is recalculated, but the speed is maintained
        // Non-zero chance that this makes the pull go farther than the actual point, but I didn't seem to have any issues
        // I'm just assuming it's ok
        currentTime = 0;
        positionOnSkill = transform.position;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    // Reference: https://youtu.be/idiq5WjCAD8
    IEnumerator Hook()
    {
        // Calculate the duration of the hook animation based on hook length
        maxHookingTime = 5f * (Vector2.Distance(transform.position, hitLocation)) / baseMaxSkillRange;
        // Calculate the base time the character takes to get to the destination based on hook length
        maxPullingTime = 0.8f * (Vector2.Distance(transform.position, hitLocation)) / baseMaxSkillRange;

        // Hook starts at the character's position
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);

        Vector2 newPos;

        // Loop in which the hook goes towards the target position
        for (currentTime = 0; currentTime < maxHookingTime; currentTime += baseSkillHookShotSpeed * Time.deltaTime)
        {
            // Linear interpolation to get the line between current and target positions,
            // taking maxHookingTime in total
            newPos = Vector2.Lerp(transform.position, hitLocation, currentTime / maxHookingTime);

            // Update hook beginning and end
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, newPos);

            // Wait until next frame, I think?
            yield return null;
        }

        // Reutilizing this variable because I'm an idiot
        currentTime = 0;
        lr.SetPosition(1, hitLocation);

        if (targetFound)
        {
            // Get current move direction
            moveDirectionOnSkill = movementController.GetMoveDirection();
            positionOnSkill = transform.position;

            // Point based on the movement direction when the skill was activated
            bezierP1 = new(transform.position.x + moveDirectionOnSkill.x * baseMomentumStrengthCoefficient, transform.position.y + moveDirectionOnSkill.y * baseMomentumStrengthCoefficient);

            // Update player speed to 0 while hook is active
            baseMovSpeed = movSpeed;
            movSpeed = 0;
            movementController.UpdatePlayerSpeed();

            pulling = true;
            targetFound = false;
            skillCurrentHookSpeed = baseSkillPullingSpeed;
        }
        else
        {
            // Hook retracts if no target has been found
            StartCoroutine(RetractHook());
        }
    }

    IEnumerator RetractHook()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, hitLocation);

        Vector2 newPos;

        for (currentTime = 0; currentTime < maxHookingTime; currentTime += baseSkillHookShotSpeed * Time.deltaTime)
        {
            // Now the hook goes backwards
            newPos = Vector2.Lerp(transform.position, hitLocation, 1 - (currentTime / maxHookingTime));
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, newPos);
            yield return null;
        }

        lr.enabled = false;
    }
}
