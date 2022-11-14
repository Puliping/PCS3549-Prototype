using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoviment : MonoBehaviour
{

    //Public variables, subject to changes.
    
    public float nextWaypointDistance = 0.4f; //How close the enemy must be to a waypoint, before changing it's direction to the next waypoint.

    //Internal variables. Don't change them.
    private float movespeed;
    private Vector3 target;
    private int currentWaypoint = 0;
    private bool reachedendofpath = true;
    private float speed_modifier = 1f;
    public bool manual_control = false;

    Path path;
    Seeker seeker;
    Rigidbody2D rb;

    void Start()
    {
        Enemy enemy = GetComponent<Enemy>();
        movespeed = enemy.base_speed;
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        StartCoroutine(UpdatePath(0.2f)); //Updates path every .2s.
    }
    IEnumerator UpdatePath(float time_between_updates)
    {
        /* Every [time_between_updates] seconds, run the pathfinding script again (if target is not null).*/
        while (true)
        {
            yield return new WaitForSeconds(time_between_updates);
            if (target != null)
                seeker.StartPath(rb.position, target, OnPathComplete);
        }
    }

    public void SetTargetPosition(Vector3 target)
    {
        /* Receives a Vector3 target location. Sets the moviment target to the received location, at full speed.*/
        this.target = target;
        this.speed_modifier = 1f;
    }

    public void SetTargetPosition(Vector3 target, float speed_modifier)
    {
        /* Receives a Vector3 target location and a flot speed_modifier. Sets the moviment target to the received location, at [speed_modifier]*base_speed.*/
        this.target = target;
        this.speed_modifier = speed_modifier;
    }

    public void SetSpeedModifier(float speed_modifier)
    {
        /* Receives a flot speed_modifier. Enemy will start moving at [speed_modifier]*base_speed towards target. A value of 1 sets the enemy speed to it's default value.*/
        this.speed_modifier = speed_modifier;
    }

    private void OnPathComplete(Path p)
    {
        /* When the seeker finishis calculating a new path, start following the new calculated path.*/
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }


    private void SetSpeed(Vector2 dir, float speed)
    {
        /* Receives a vector2 direction and a float speed. Sets the RigidBody speed to their product.*/
        rb.velocity = dir.normalized * speed;
    }

    private void ChangeWaypoint()
    {
        /* If the next waypoint is too close, start going to the one after.*/
        float dist = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    public bool ReachedEndOfPath()
    {
        return reachedendofpath;
    }

    private Coroutine manualCoroutine;
    public void ManualControl(Vector2 direction, float speed_modifier, float time)
    {
        /* Manually sets the enemy speed in a given direction. This state is kept for [time] seconds */
        SetSpeed(direction, movespeed*speed_modifier);
        if (manual_control) StopCoroutine(manualCoroutine);
        manualCoroutine = StartCoroutine(ManualController(time));
    }

    private IEnumerator ManualController(float time)
    {

        manual_control = true;
        yield return new WaitForSeconds(time);
        manual_control = false;
    }
            
    void Update()
    {
        if (manual_control) return;

        if (path == null)
        {
            SetSpeed(Vector2.zero, 0);
            return;
        }

        reachedendofpath = currentWaypoint >= path.vectorPath.Count;
        if (reachedendofpath)
        {
            if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint - 1]) < 0.02f)
                SetSpeed(Vector2.zero, 0);
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        SetSpeed(direction, movespeed * speed_modifier);
        ChangeWaypoint();

    }
}
