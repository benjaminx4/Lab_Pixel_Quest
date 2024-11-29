using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed = 5;
    public Transform[] patrolPoints;
    public int patrolIndex = 0;

    public float waitTime = 1;
    private float currentTime = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = patrolPoints[patrolIndex].position;
        currentTime = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[patrolIndex].position, speed * Time.deltaTime);
       
        if(transform.position != patrolPoints[patrolIndex].position) {return;}

        if(currentTime < 0)
        {
            patrolIndex++;
            if(patrolIndex >= patrolPoints.Length)
            {
                patrolIndex = 0;
            }
            currentTime = waitTime;
        }
        else
        {
            currentTime -= Time.deltaTime;
        }
    }
}
