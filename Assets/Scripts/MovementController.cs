using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    Vector2 moveDirection;
    [SerializeField]
    private Player player;
    private float movspeed;
    [SerializeField]
    private Rigidbody2D rb;

    private void Start()
    {
        movspeed = player.movSpeed;
    }
    void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();        
    }
    public void UpdatePlayerSpeed()
    {
        movspeed = player.movSpeed;
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
