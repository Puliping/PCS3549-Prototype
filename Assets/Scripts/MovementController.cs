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
        rb.velocity = moveDirection* movSpeed;
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }
}
