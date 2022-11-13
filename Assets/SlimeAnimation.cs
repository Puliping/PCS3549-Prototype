using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Enemy enemy;
    [SerializeField]
    private Animator animator;

    void Start()
    {
        if (!enemy) enemy = GetComponent<Enemy>();
        if (!animator) animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemy.enemyState)
        {
            case Enemy.States.Patrol:
                animator.speed = 0.3f;
                break;
            case Enemy.States.Follow:
                animator.speed = 1f;
                break;
            case Enemy.States.Attack:
                animator.speed=1;
                animator.SetTrigger("Attack Trigger");
                break;

        }
    }
}