using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public enum UpgradeType
    {
        Health,
        Energy,
        FireRate,
        Damage,
        BulletPenetration,
        NumOfBullets
    }

    public CombatController combatController;
    public WindupManager windupManager;
    public PlayerHealth playerHealth;

    public int upgradeTokens = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TryUpgrade();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryUpgrade()
    {
        if(upgradeTokens > 0)
        {
            upgradeTokens--;
            UpgradeType chosenUpgrade = (UpgradeType)Random.Range(0, 5);
            switch(chosenUpgrade)
            {
                case UpgradeType.Health:
                    {
                        playerHealth.IncreaseMaxHealth(20.0f);
                        break;
                    }
                case UpgradeType.Energy:
                    {
                        windupManager.AddMaxEnergy(20.0f);
                        break;
                    }
                case UpgradeType.FireRate:
                    {
                        combatController.IncreaseFireRate(0.2f);
                        break;
                    }
                case UpgradeType.Damage:
                    {
                        combatController.IncreaseDamageMulti(0.2f);
                        break;
                    }
                case UpgradeType.BulletPenetration:
                    {
                        combatController.IncreaseBulletPen(1);
                        break;
                    }
                case UpgradeType.NumOfBullets:
                    {
                        combatController.IncreaseBulletsPerShot(1);
                        break;
                    }
            }
        }
    }
}