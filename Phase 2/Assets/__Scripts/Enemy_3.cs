using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    private Vector3 p0, p1;
    private float _timeStart;
    private float _direction;
    private float _duration = 4;



    void Start()
    {
        p0 = p1 = pos;

        InitMovement();
    }
    void InitMovement()
    {
        p0 = p1;
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-widMinRad / 2.0f, widMinRad / 2.0f);
        p1.y = Random.Range(-hgtMinRad / 3.0f, hgtMinRad / 3.0f);

        _timeStart = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - _timeStart) / _duration;

        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * p0 + u * p1;
    }

    public override void AddToScore()
    {
        ScoreManager.ADD_POINTS(7);
    }
}
