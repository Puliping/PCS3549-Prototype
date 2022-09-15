using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;

public class Player : MonoBehaviour
{
    public float hp;
    public float movSpeed;
    public float invulnerableTime = .5f;
    private bool isInvulnerable = false;
    public GameObject arrowPrefab;
    public int coroutineId;
    Vector2 aimDirection;
    bool firing;
    bool melee = true;
    public GameObject meleeAttackSprite;
    float meleeAttackOffset = 0.7f;
    public float meleeAttackRange = 0.5f;
    public LayerMask enemyLayers;
    public float damageToReceive = 0;
    public delegate void receiveDamage();
    public event receiveDamage takeDamageAfterInvulnTime;
    [SerializeField]
    private MovementController movementController;
    [SerializeField]
    private TextMeshProUGUI textDamage;
    [SerializeField]
    private TextMeshProUGUI textWeapon;
    void OnAim(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        if (PlayerInput.GetPlayerByIndex(0).currentControlScheme == "Keyboard and Mouse")
        {
            dir.x -= Screen.width / 2;
            dir.y -= Screen.height / 2;
            dir.Normalize();
        }
        aimDirection = new Vector2(dir.x, dir.y);
    }
    void OnFire()
    {
        if (melee)
        {
            if (!firing)
            {
                StartCoroutine(MeleeAttackAt(aimDirection));
            }
        }
        else
        {
            if (!firing)
            {
                StartCoroutine(FireAt(aimDirection));
            }
        }
    }
    
    IEnumerator OnChangeAttack() {
        melee = !melee;
        textWeapon.gameObject.SetActive(true);
        textWeapon.text = melee ? "Sword" : "Bow";
        yield return new WaitForSeconds(.35f);
        textWeapon.gameObject.SetActive(false);
    }

    IEnumerator MeleeAttackAt(Vector2 aimDirection)
    {
        // update attack point position
        Vector3 meleeAttackPoint = transform.position;
        meleeAttackPoint += new Vector3(meleeAttackOffset * aimDirection.x, meleeAttackOffset * aimDirection.y, 0);
        meleeAttackSprite.transform.position = meleeAttackPoint;
        meleeAttackSprite.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90);

        // animate
        yield return new WaitForSeconds(0.1f);
        meleeAttackSprite.SetActive(true);

        // detect enemies
        Collider2D[] hits = Physics2D.OverlapCircleAll(meleeAttackPoint, meleeAttackRange, enemyLayers);
        foreach (Collider2D enemy in hits)
        {
            // deal damage
            Enemy enemyComponent = enemy.GetComponentInChildren<Enemy>();
            enemyComponent.TakeDamage(5f); // FFFFF

            Debug.Log("hit " + enemy.name);
        }

        // return memes
        yield return new WaitForSeconds(0.1f);
        meleeAttackSprite.SetActive(false);
        yield return new WaitForSeconds(0f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(meleeAttackSprite.transform.position, meleeAttackRange);
    }

    IEnumerator FireAt(Vector2 aimDirection)
    {
        firing = true;
        GameObject projectile = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = aimDirection*10;
        projectile.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg);
        yield return new WaitForSeconds(0f);
        firing = false;
    }

    public void TakeDamage(float damage)
    {
        if(isInvulnerable)
        {
            return;
        }

        hp -= damage;
        //knockback effect?
        textDamage.gameObject.SetActive(true);
        textDamage.text = "-" + damage.ToString();

        if (hp <= 0)
        {
            morreu();
        }

        isInvulnerable = true;
        StartCoroutine(InvulnerabilityCooldown());
    }

    public void morreu()
    {
        GameModeController.Instance.Defeat();
        movSpeed = 0;
        movementController.Died();
    }

    IEnumerator InvulnerabilityCooldown()
    {
        yield return new WaitForSeconds(invulnerableTime);
        textDamage.gameObject.SetActive(false);
        isInvulnerable = false;
        if(damageToReceive != 0)
        {
            TakeDamage(damageToReceive);
        }
    }
}
