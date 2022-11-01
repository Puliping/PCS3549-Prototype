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
    public float meleeAttackRange = 0.3f;
    public LayerMask enemyLayers;
    public float damageToReceive = 0;
    public delegate void receiveDamage();
    /*public event receiveDamage takeDamageAfterInvulnTime;*/
    [SerializeField]
    private MovementController movementController;
    [SerializeField]
    private TextMeshProUGUI textDamage;
    [SerializeField]
    private TextMeshProUGUI textWeapon;

    public void GoAtack()
    {

    }
    public void Dash()
    {

    }
    public void Skill()
    {

    }
    public void GoChargedAttack()
    {

    }
    public void HitedEnemy()
    {

    }
    public void ReceiveXp()
    {

    }
    public void UpPlayer()
    {

    }
    public void UpdateVisual()
    {

    }
    public void CmdUpdateVisual()
    {

    }
    public void RpcUpdateVisual()
    {

    }

    public void UpdateAnimations()
    {

    }
    public void CmdUpdateAnimations()
    {

    }
    public void RpcUpdateAnimations()
    {

    }
    public void UpdateCanvas()
    {


    }
    public void CmdUpdateCanvas()
    {

    }
    public void RpcUpdateCanvas()
    {

    }
    public void Interact()
    {

    }
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
    /* Deveria estar na arma nao no player
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
    */
    /*Usar essa funcao para o WeaponMater*/
    IEnumerator OnChangeAttack() {
        melee = !melee;
        textWeapon.gameObject.SetActive(true);
        textWeapon.text = melee ? "Sword" : "Bow";
        yield return new WaitForSeconds(.35f);
        textWeapon.gameObject.SetActive(false);
    }
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

    public void ReceiveDamage(float damage)
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
            Die();
        }

        isInvulnerable = true;
        StartCoroutine(InvulnerabilityCooldown());
    }

    public void Die()
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
            ReceiveDamage(damageToReceive);
        }
    }
}
