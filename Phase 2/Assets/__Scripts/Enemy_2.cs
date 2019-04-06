using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    public float lifeTime;
    public Vector3[] points; //Points are used as references for the enemy's movement from one point to another in swoop
    public float birthTime;

    void Start()
    {
        points = new Vector3[3];

        points[0] = pos;

        //The largest and smallest x values must be within the screen area
        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;

        Vector3 v;
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax); //Assigns a random x value between bounds stated above
        v.y = -bndCheck.camHeight * Random.Range(2.75f, 2); //Assigns a random factor to the y dipping point of the enemy swoop
        points[1] = v;

        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;

        birthTime = Time.time; //Initializes the birth time to be the current time
    }

    //Move overrides the parent move and in this function, it is determined how low the enemy swoops
    //and how wide the swoop is horizontally
    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 p01, p12;
        u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
        p01 = (1 - u) * points[0] + u * points[1];
        p12 = (1 - u) * points[1] + u * points[2];
        pos = (1 - u) * p01 + u * p12;
    }

    //Each Enemy_2 is only worth 2 points to add to score
    public override void AddToScore()
    {
        ScoreManager.ADD_POINTS(1);
    }
}
