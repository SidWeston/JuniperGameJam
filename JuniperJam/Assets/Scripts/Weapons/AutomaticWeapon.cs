using System.Collections;
using UnityEngine;

public class AutomaticWeapon : Weapon
{
    private bool firing;

    private Coroutine fireRoutine;
    private Vector2 mousePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.mouseEvent += OnMouseMove;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void FireWeapon(Plane groundPlane)
    {
        firing = true;
        fireRoutine = StartCoroutine(Fire(groundPlane));
    }

    public override void UnFire()
    {
        firing = false;
        StopCoroutine(fireRoutine);
    }

    public virtual IEnumerator Fire(Plane groundPlane)
    {
        while(firing)
        {
            GameObject bullet = Instantiate(bulletProjectile, bulletSpawnPos.position, bulletSpawnPos.rotation * Quaternion.Euler(90, 0, 0));
            bullet.GetComponent<Bullet>().damage = weaponDamage;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 worldPoint = ray.GetPoint(distance);
                Vector3 direction = worldPoint - transform.position;
                direction.y = 0;
                bullet.TryGetComponent(out Bullet bulletComp);
                bulletComp.direction = (direction.normalized) * projectileSpeed;
                yield return new WaitForSeconds(fireCooldown);
            }
        }
        yield return null;
    }

    private void OnMouseMove(Vector2 input)
    {
        //messy but i dont care
        mousePos = input;
    }
}