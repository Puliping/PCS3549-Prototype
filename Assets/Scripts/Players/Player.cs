using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float hp;
    public float movSpeed;
    public GameObject arrowPrefab;
    Vector2 aimDirection;
    bool firing;

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
}
