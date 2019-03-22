using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
  
    float random = 0;
    
   void Start()
    {
        random = gameObject.transform.position.x;
    }
    public override void Move()
    {
        Vector3 tempPos = pos;

        if (random % 2 > 1)
        {
            tempPos.x -=  speed * Time.deltaTime;
        }
        else
        {
            tempPos.x += speed * Time.deltaTime;
        }
        tempPos.y -= speed * Time.deltaTime;
        
        pos = tempPos;
       
    }

    public override void AddToScore()
    {
        ScoreManager.ADD_POINTS(2);
    }
}
