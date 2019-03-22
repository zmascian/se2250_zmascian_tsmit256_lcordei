using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck _bndCheck;

    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();
    }

    void Update()
    {
        if (_bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }
}
