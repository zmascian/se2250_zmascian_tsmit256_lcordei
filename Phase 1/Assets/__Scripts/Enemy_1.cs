using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    float random = 0;

   void Start()
    {
        random = Time.time;
    }
    public override void Move()
    {
        Vector3 tempPos = pos;
       
        if (random % 2 > 1)
        {
            tempPos.x -= speed * Time.deltaTime;
        }
        else
        {
            tempPos.x += speed * Time.deltaTime;
        }
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
       
    }
}
