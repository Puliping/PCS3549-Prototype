using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    // Start is called before the first frame update
    private Vector2 spawn_position;

    private bool idlePathCooldown = true;
    [SerializeField]
    private GameObject slimeWeapon;
    

    public Animator animator;

    [SerializeField]
    private float idleAnimationSpeed = 1f;
    
    public override void Start()
    {
        base.Start();
        spawn_position = this.transform.position;
        animator.SetFloat("AttackDuration", 1f/attackDuration);
        animator.SetFloat("IdleSpeed", idleAnimationSpeed);
    }

    private Vector3 dashDirection;
    public override void Attack()
    {
        
        if (!CanAttack()) return;
        //base.Attack();
        
        slimeWeapon.SetActive(true);
        animator.Play("SlimeAttackAnimation");
        onAttackCooldown = true;
        StartCoroutine(AttackDash());
        StartCoroutine(AttackDuration());
    }


    [SerializeField]
    private float dashRatio = 0.7f;
    [SerializeField]
    private float dashspeed = 7f;
    IEnumerator AttackDash()
    {
        dashDirection = aggroTarget.transform.position - this.transform.position;
        yield return new WaitForSeconds(attackDuration*dashRatio);
        moviment.ManualControl(dashDirection, dashspeed, attackDuration * (1-dashRatio));
        yield return new WaitForSeconds(attackDuration * (1-dashRatio));
        StartCoroutine(AttackCooldown());
    }

    public override void Patrol()
    {
        /* While Patroling, slimes move slowly from one position to another close to the spawn*/
        base.Patrol();
        if (moviment.ReachedEndOfPath() && idlePathCooldown)
        {
            moviment.SetTargetPosition(Random.insideUnitCircle * 2 + spawn_position, .1f);
            StartCoroutine(PathCooldown());
        }; 
    }

    public override void Follow()
    {
        base.Follow();
        if (aggroTarget) moviment.SetTargetPosition(aggroTarget.transform.position + Vector3.zero, 1f);
    }

    private IEnumerator PathCooldown()
    {
        idlePathCooldown = false;
        yield return new WaitForSeconds(1f);
        while (!moviment.ReachedEndOfPath())
        {
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(4f);
        idlePathCooldown = true;
    }


}
