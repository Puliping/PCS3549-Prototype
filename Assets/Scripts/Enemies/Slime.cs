using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    // Start is called before the first frame update
    private Vector2 spawn_position;

    private bool idlePathCooldown = true;

    public Animator animator;

    public override void Attack()
    {
        base.Attack();
        animator.Play("SlimeAttackAnimation");

    }
    public override void Start()
    {
        spawn_position = this.transform.position;
        moviment = GetComponent<EnemyMoviment>();
        StartCoroutine(LineOfSightLoop());
    }
    public override void Patrol()
    {
        /* While Patroling, slimes move slowly from one position to another close to the spawn*/
        enemyState = States.Patrol;
        if (moviment.ReachedEndOfPath() && idlePathCooldown)
        {
            moviment.SetTargetPosition(Random.insideUnitCircle * 2 + spawn_position, .1f);
            StartCoroutine(PathCooldown());
        }; 
        if (aggroTarget) enemyState = States.Follow;



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
