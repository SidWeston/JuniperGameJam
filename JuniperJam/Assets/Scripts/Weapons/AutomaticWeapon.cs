using System.Collections;
using UnityEngine;

public class AutomaticWeapon : Weapon
{
    private bool firing;

    [SerializeField] private WindupManager energyManager;
    [SerializeField] private CombatController combatController;
    [SerializeField] private float energyCostPerShot;
    [SerializeField] private float weaponSpread;

    private Coroutine fireRoutine;
    private Vector2 mousePos;

    private float baseFireCooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.mouseEvent += OnMouseMove;
        baseFireCooldown = fireCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        fireCooldown = baseFireCooldown * (energyManager.energyPercent > 0.5f ? 1f : energyManager.energyPercent > 0f ? 1.5f : 2.5f) / combatController.fireRateMulti;
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
        while (firing)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 worldPoint = ray.GetPoint(distance);
                Vector3 baseDirection = worldPoint - transform.position;
                baseDirection.y = 0;
                baseDirection.Normalize();

                int bulletCount = combatController.bulletsPerShot;

                for (int i = 0; i < bulletCount; i++)
                {
                    float angle;

                    if (bulletCount == 1)
                    {
                        angle = 0f;
                    }
                    else
                    {
                        float t = (float)i / (bulletCount - 1); 
                        angle = Mathf.Lerp(-(weaponSpread * (bulletCount - 1)) / 2f, (weaponSpread * (bulletCount - 1)) / 2f, t);
                    }

                    Vector3 direction = Quaternion.Euler(0, angle, 0) * baseDirection;

                    GameObject bullet = Instantiate(bulletProjectile, bulletSpawnPos.position, bulletSpawnPos.rotation * Quaternion.Euler(90, 0, 0));
                    bullet.GetComponent<Bullet>().damage = weaponDamage * combatController.damageMulti;
                    bullet.TryGetComponent(out Bullet bulletComp);
                    bulletComp.direction = direction * projectileSpeed;
                    bulletComp.hitsLeft = combatController.bulletPenetration;
                }

                energyManager.energy -= energyCostPerShot;
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