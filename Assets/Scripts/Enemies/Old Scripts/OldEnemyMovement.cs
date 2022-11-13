using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldEnemyMovement : MonoBehaviour
{

    public Vector3 target;
    public float movespeed;
    private float speed_modifier = 1f;
    private float nextWaypointDistance = 0.4f;

    Path path;
    private int currentWaypoint=0;
    private bool reachedendofpath = false;

    Seeker seeker;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.4f);
    }

    private void UpdatePath()
    {
        if(target != null)
            seeker.StartPath(rb.position, target, OnPathComplete);
    }

    public void SetTargetPosition(Vector3 target)
    {
        this.target = target;
        this.speed_modifier = 1f;
    }

    public void SetTargetPosition(Vector3 target,float speed_modifier)
    {
        this.target = target;
        this.speed_modifier = speed_modifier;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }


    private void SetSpeed(Vector2 dir, float speed)
    {
        rb.velocity = dir * speed;
    }

    private void ChangeWaypoint()
    {
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

    // Update is called once per frame
    void Update()
    {

        if (path == null)
        {
            SetSpeed(Vector2.zero, 0);
            return;
        }
        
        reachedendofpath = currentWaypoint >= path.vectorPath.Count;
        if (reachedendofpath)
        {
            if(Vector2.Distance(rb.position, path.vectorPath[currentWaypoint-1])<0.02f)
                SetSpeed(Vector2.zero, 0);
            return;
        }
        
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint]-rb.position).normalized;
        SetSpeed(direction, movespeed*speed_modifier);
        ChangeWaypoint();

    }
}
