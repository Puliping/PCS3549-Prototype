using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    private bool idlePathCooldown=true;
    private Vector3 spawn_position;
    [SerializeField]
    protected GameObject arrowPrefab;

    [SerializeField]
    protected float arrowSpeed;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        spawn_position = this.transform.position;
    }


    public override void Patrol()
    {
        /* While Patroling, slimes move slowly from one position to another close to the spawn*/
        base.Patrol();
        if (moviment.ReachedEndOfPath() && idlePathCooldown)
        {
            moviment.SetTargetPosition(Random.insideUnitCircle * 2 + (Vector2)spawn_position, .1f);
            StartCoroutine(PathCooldown());
        };
    }

    public override void Follow()
    {
        base.Follow();
        if (aggroTarget)
        {
            float dist = Vector2.Distance(transform.position, aggroTarget.transform.position);
            Vector2 dir = (transform.position - aggroTarget.transform.position).normalized;
            if (dist < 2.5f)
            {
                moviment.SetTargetPosition(transform.position+(Vector3)dir, 1f);
            }
            else if (dist > 4f )
            {
                moviment.SetTargetPosition(transform.position-(Vector3)dir, 1f);
            }
            else
            {
                moviment.SetTargetPosition(transform.position, 0f);
            }

        }
    }

    public override void Attack()
    {
        if (!CanAttack()) return;
        base.Attack();
        GameObject projectile = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Vector2 aimDirection = aggroTarget.transform.position - this.transform.position;
        projectile.GetComponent<Rigidbody2D>().velocity = aimDirection * arrowSpeed;
        projectile.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg);

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
    // Update is called once per frame

}
