using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_45DegMove : Enemy
{
    public override void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
}
