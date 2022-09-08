using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

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
    public float damageToReceive = 0;

    public delegate void receiveDamage();
    public event receiveDamage takeDamageAfterInvulnTime;


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
        if (!firing)
        {
            StartCoroutine(FireAt(aimDirection));
        }
    }

    IEnumerator FireAt(Vector2 aimDirection)
    {
        firing = true;
        GameObject projectile = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = aimDirection;
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
        Debug.Log("player hp = " + hp);

        if(hp<0)
        {
            morreu();
        }

        isInvulnerable = true;
        StartCoroutine(InvulnerabilityCooldown());
    }

    public void morreu()
    {
        //faz o urro
    }

    IEnumerator InvulnerabilityCooldown()
    {
        yield return new WaitForSeconds(invulnerableTime);
        isInvulnerable = false;
        if(damageToReceive != 0)
        {
            TakeDamage(damageToReceive);
        }
    }
}
