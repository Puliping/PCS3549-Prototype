using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float hp;
    public float movSpeed;
    public GameObject arrowPrefab;

    void OnFire(InputValue value)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        StartCoroutine(fireAt(mousePosition));
    }

    IEnumerator fireAt(Vector2 mousePosition)
    {
        Vector3 worldPos;

        GameObject projectile = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, 0, mousePosition.y));
        worldPos.Normalize();
        projectile.GetComponent<Rigidbody2D>().velocity = worldPos;
        yield return new WaitForSeconds(1);
    }
}
