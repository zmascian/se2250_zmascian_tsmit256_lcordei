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

    //Overrides the parent move function
    public override void Move()
    {
        Vector3 tempPos = pos;

        //Chooses left or right randomly and moves that horizontal direction
        if (random % 2 > 1)
        {
            tempPos.x -=  speed * Time.deltaTime;
        }
        else
        {
            tempPos.x += speed * Time.deltaTime;
        }
        tempPos.y -= speed * Time.deltaTime; //Constant vertical component downwards
        
        pos = tempPos;
       
    }

    //Each Enemy_1 will add 2 points to the sum of the score
    public override void AddToScore()
    {
        ScoreManager.ADD_POINTS(2);
    }
}
