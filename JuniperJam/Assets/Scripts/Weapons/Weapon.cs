using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    [SerializeField] protected float weaponDamage;
    [SerializeField] protected float fireCooldown; //cooldown in seconds
    [SerializeField] protected float reloadTime;
    [SerializeField] protected float projectileSpeed;
    protected bool canFire = true;

    //animations
    public AnimationClip aimAnim;
    public AnimationClip fireAnim;
    public AnimationClip reloadAnim;

    public GameObject bulletProjectile;
    public Transform bulletSpawnPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void FireWeapon(Plane groundPlane)
    {

    }

    public virtual void UnFire()
    {

    }

    protected virtual void ResetCanFire()
    {
        canFire = true;
    }
}