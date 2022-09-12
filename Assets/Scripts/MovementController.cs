using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    Vector2 moveDirection;
    private float movspeed;
    private Rigidbody2D rb;

    private void Start()
    {
        movspeed = this.GetComponent<Player>().movSpeed;
        rb = GetComponent<Rigidbody2D>();
    }
    void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
        
    }

    private void Update()
    {
        //transform.Translate(new Vector3(moveDirection.x, moveDirection.y, 0) * Time.deltaTime*movspeed);
    }
    private void FixedUpdate()
    {
        rb.velocity = moveDirection* movspeed;
    }
}
