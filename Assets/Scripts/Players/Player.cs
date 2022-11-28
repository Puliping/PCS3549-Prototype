using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;
using System;

public abstract class Player : MonoBehaviour
{
    public float hp;
    public float movSpeed;
    public float invulnerableDuration = .5f;
    public float attackMultiplier = 1f;
    public float movSpeedMultiplier = 1f;
    public Weapon weapon;
    public GameObject weaponGameObject;
    protected bool isInvulnerable = false;
    public Vector2 aimPosition;
    
    [SerializeField]
    protected MovementController movementController;
    [SerializeField]
    protected TextMeshProUGUI textDamage;
    [SerializeField]

    public static bool canUseSkill = true;

    protected Controls controls;

    protected enum DashType { Roll, Dash }

    [SerializeField]
    protected DashType dashType = DashType.Dash;

    protected virtual void Awake()
    {
        controls = new Controls();
        movementController = gameObject.GetComponent<MovementController>();
    }

    protected virtual void OnEnable()
    {
        // controls.Exploration.Attack.performed += Attack;
        controls.Exploration.Enable();
    }
    protected virtual void Start()
    {
        weaponGameObject = Instantiate(weaponGameObject, transform);
        weapon = weaponGameObject.GetComponentInChildren<Weapon>();
    }

    public virtual void OnSkill() { }

    public IEnumerator SkillCooldown(float SkillCD)
    {
        canUseSkill = false;
        yield return new WaitForSeconds(SkillCD);
        canUseSkill = true;
        Debug.Log("Skill is ready");
    }
    public virtual void HitEnemy()
    {

    }
    public virtual void ReceiveXp()
    {

    }
    public virtual void UpPlayer()
    {

    }
    public virtual void UpdateVisual()
    {

    }
    public virtual void CmdUpdateVisual()
    {

    }
    public virtual void RpcUpdateVisual()
    {

    }

    public virtual void UpdateAnimations()
    {

    }
    public virtual void CmdUpdateAnimations()
    {

    }
    public virtual void RpcUpdateAnimations()
    {

    }
    public virtual void UpdateCanvas()
    {


    }
    public virtual void CmdUpdateCanvas()
    {

    }
    public virtual void RpcUpdateCanvas()
    {

    }
    public virtual void Interact()
    {

    }
    protected virtual void OnAim(InputValue value)
    {
        Vector2 aim = value.Get<Vector2>();
        if (PlayerInput.GetPlayerByIndex(0).currentControlScheme == "Keyboard and Mouse")
        {
            aim.x -= Screen.width / 2;
            aim.y -= Screen.height / 2;
            aim.Normalize();
        }
        aimPosition = new Vector2(aim.x, aim.y);
    }

    // virtual void Attack(InputAction.CallbackContext context)
    // {
    //     if(context.started){
    //         weapon.Attack(aimPosition);
    //     }
    //     else if(context.canceled) 
    //     {
    //         if(context.duration > weapon.chargeTime) {
    //             weapon.ChargedAttack(aimPosition);
    //         }
    //     }
    // }

    // TODO: rubens tem que arrumar a on attack e o contexto
    protected virtual void OnAttack()
    {
        Debug.Log("OnAttack");
        // n esquece de pegar aqui
        weapon.Attack(aimPosition);
    }

    protected virtual void OnChargedAttack()
    {
        Debug.Log("charged attack");
        weapon.ChargedAttack(aimPosition);
    }

    protected bool canDash = true;
    public float dashCooldown = 1f;
    protected virtual void OnDash()
    {
        if (!canDash) return;
        StartCoroutine(UseDash());
    }

    protected IEnumerator UseDash()
    {
        // disable future dashes, will need to change if dash gets more than one charge
        canDash = false;

        // set up invincibility dash, if that's the case
        // float speedMod = 1.2f;
        if (dashType == DashType.Dash)
        {
            StartCoroutine(InvulnerabilityCooldown(1f));
            // speedMod = 1.5f;
        }

        movementController.dashing = true;
        movementController.dashDirection = aimPosition.normalized;

        yield return new WaitForSeconds(.5f);

        movementController.dashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    /*Usar essa funcao para o WeaponMaster*/
    // IEnumerator OnChangeAttack() {
    //     melee = !melee;
    //     textWeapon.gameObject.SetActive(true);
    //     textWeapon.text = melee ? "Sword" : "Bow";
    //     yield return new WaitForSeconds(.35f);
    //     textWeapon.gameObject.SetActive(false);
    // }

    // Deveria estar na arma e nao no player
    /*
    Vector3 meleePhysicsAttackPoint = Vector3.zero;
    IEnumerator MeleeAttackAt(Vector2 aimDirection)
    {
        // update attack point position
        firing = true;
        Vector3 meleeAttackPoint = transform.position;
        meleeAttackPoint += new Vector3(meleeAttackOffset * aimDirection.x, meleeAttackOffset * aimDirection.y, 0);

        meleePhysicsAttackPoint = transform.position + new Vector3(meleeAttackOffset * aimDirection.x*0.8f, meleeAttackOffset * aimDirection.y*0.8f, 0);

        meleeAttackSprite.transform.position = meleeAttackPoint;
        meleeAttackSprite.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90);

        // animate
        
        meleeAttackSprite.SetActive(true);

        yield return new WaitForSeconds(0.15f);
        // detect enemies
        Collider2D[] hits = Physics2D.OverlapCircleAll(meleePhysicsAttackPoint, meleeAttackRange, enemyLayers);

        foreach (Collider2D enemy in hits)
        {
            // deal damage
            Enemy enemyComponent = enemy.GetComponentInChildren<Enemy>();
            enemyComponent.TakeDamage(5f); // FFFFF

            Debug.Log("hit " + enemy.name);
        }

        // return memes
        yield return new WaitForSeconds(0.05f);
        meleeAttackSprite.SetActive(false);

        firing = false;
    }
    */
    /* Deveria estar na arma e nao no player
    IEnumerator FireAt(Vector2 aimDirection)
    {
        firing = true;
        GameObject projectile = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = aimDirection*10;
        projectile.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg);
        yield return new WaitForSeconds(0.2f);
        firing = false;
    }
    */
    public virtual void ReceiveDamage(float damage)
    {
        if (isInvulnerable) {
            Debug.Log("hit while invunerable");
            return;
        }
        hp -= damage;

        //knockback effect?

        textDamage.gameObject.SetActive(true);
        textDamage.text = "-" + damage.ToString();

        if (hp <= 0) Die();

        StartCoroutine(InvulnerabilityCooldown());
    }

    protected virtual void Die()
    {
        GameModeController.Instance.Defeat();
        movSpeed = 0;
        movementController.UpdatePlayerSpeed();
        movementController.enabled = false;
    }

    protected IEnumerator InvulnerabilityCooldown(float? duration = null)
    {
        float time = duration ?? invulnerableDuration;
        isInvulnerable = true;
        yield return new WaitForSeconds(time);
        textDamage?.gameObject.SetActive(false);
        isInvulnerable = false;
    }
}
