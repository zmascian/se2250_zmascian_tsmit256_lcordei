using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    public override void Move()
    {

        Vector3 tempPos = pos;
        if (Random.Range(1,2) == 2)
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
