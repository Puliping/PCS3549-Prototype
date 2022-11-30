using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    Vector2 moveDirection;
    private Player player;
    private float movSpeed;
    [SerializeField]
    private Rigidbody2D rb;
    public bool dashing = false;
    public Vector2 dashDirection;

    private void Start()
    {
        player = LevelManager.Instance.localManager.playerRef;
        movSpeed = player.movSpeed;
    }
    void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();        
    }
    public void UpdatePlayerSpeed()
    {
        movSpeed = player.movSpeed;
    }

    private void Update()
    {
        //transform.Translate(new Vector3(moveDirection.x, moveDirection.y, 0) * Time.deltaTime*movspeed);
    }
    private void FixedUpdate()
    {
        if (!dashing)
            rb.velocity = moveDirection * movSpeed;
        else
            rb.velocity = dashDirection * movSpeed * 4f;
    }

    public void Dash(Vector2 aimPosition)
    {
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }
}
