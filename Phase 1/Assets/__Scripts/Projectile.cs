using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck _bndCheck;
    private Renderer rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    private WeaponType _type;

    //This property masks the field_type and takes action when it is set
    public WeaponType type
    {
        get
        {
            return _type;
        }
        set
        {
            SetType(value);
        }
    }

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

    public void SetType(WeaponType eType)
    {
        //Set the _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
