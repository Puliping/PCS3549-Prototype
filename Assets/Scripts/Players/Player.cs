using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;
using System;
using UnityEngine.UI;

public abstract class Player : MonoBehaviour
{
    public Camera mainCamera;
    public Image imageSkillCD;
    public Image imageDashCD;

    [Header("Stats")]
    public float hp;
    public float movSpeed;
    public float invulnerableDuration = .5f;
    public float attackMultiplier = 1f;
    public float movSpeedMultiplier = 1f;
    protected Weapon weapon;
    public GameObject weaponGameObject;
    protected bool isInvulnerable = false;
    public Vector2 aimPosition;
    public Slider slider;
    protected float fullhp;
    
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
        LevelManager.Instance.mainCamera = mainCamera;
        weaponGameObject = Instantiate(weaponGameObject, this.transform.position, Quaternion.identity, transform);
        weapon = weaponGameObject.GetComponentInChildren<Weapon>();
        fullhp = hp;
        slider.value = 1;
    }

    public float skillCooldown;
    public virtual void OnSkill() { }

    public IEnumerator SkillCooldown()
    {
        canUseSkill = false;
        imageSkillCD.fillAmount = 1;
        StartCoroutine(ImageSkill());
        yield return new WaitForSeconds(skillCooldown);
        canUseSkill = true;
        Debug.Log("Skill is ready");
    }
    protected IEnumerator ImageSkill()
    {
        float temp = 0;
        while (temp < skillCooldown)
        {
            imageSkillCD.fillAmount = (skillCooldown - temp)/ skillCooldown;
            yield return null;
            temp = temp + Time.deltaTime;
        }
        yield return null;
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
    public virtual void Interact(GameObject Interact)
    {
        Destroy(weaponGameObject.gameObject);
        weaponGameObject = Interact;
        var interactTemp = weaponGameObject.GetComponent<InteractObject>();
        Destroy(interactTemp.rb);
        Destroy(interactTemp.collider2D);
        weaponGameObject.gameObject.transform.parent = this.gameObject.transform;
        weaponGameObject.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        weapon = weaponGameObject.GetComponentInChildren<Weapon>();
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

    protected virtual void OnAttack()
    {
        weapon.Attack(aimPosition);
    }

    protected virtual void OnChargedAttack()
    {
        weapon.ChargedAttack(aimPosition);
    }

    protected bool canDash = true;
    public float dashCooldown = 0.5f;
    protected virtual void OnDash()
    {
        if (!canDash) return;
        StartCoroutine(UseDash());
    }

    protected IEnumerator UseDash()
    {
        // disable future dashes, will need to change if dash gets more than one charge
        canDash = false;
        imageDashCD.fillAmount = 1;
        StartCoroutine(ImageDash());
        // set up invincibility dash, if that's the case
        // float speedMod = 1.2f;
        if (dashType == DashType.Dash)
        {
            StartCoroutine(InvulnerabilityCooldown(0.25f));
            // speedMod = 1.5f;
        }

        movementController.dashing = true;

        yield return new WaitForSeconds(.26f);

        movementController.dashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
    protected IEnumerator ImageDash()
    {
        float temp = 0;
        while (temp < dashCooldown)
        {
            imageDashCD.fillAmount = (dashCooldown - temp)/ dashCooldown;
            yield return null;
            temp = temp + Time.deltaTime;
        }
        yield return null;
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
        slider.value = hp / fullhp;
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
