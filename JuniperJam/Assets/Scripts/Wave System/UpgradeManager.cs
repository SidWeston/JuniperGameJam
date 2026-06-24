using UnityEngine;
using TMPro;
using System.Collections;

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

    public GameObject upgradeText;    
    public GameObject spawnPoint;

    public GameObject wheelObj;
    private bool spinning;

    public int upgradeTokens = 1;
    public TextMeshProUGUI tokenText;

    private bool playerOverlapping = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        InputManager.instance.interactKey.keyPress += OnInteract;
    }

    // Update is called once per frame
    void Update()
    {
        if(spinning)
        {
            wheelObj.transform.Rotate(0, 0, 360 * Time.deltaTime);
        }
    }

    public void TryUpgrade()
    {
        if(upgradeTokens > 0)
        {
            upgradeTokens--;
            tokenText.SetText("Upgrade Tokens: " + upgradeTokens);
            spinning = true;
            UpgradeType chosenUpgrade = (UpgradeType)Random.Range(0, 6);
            GameObject textPopup = Instantiate(upgradeText, spawnPoint.transform.position, spawnPoint.transform.rotation);
            textPopup.TryGetComponent(out TMP_Text tmText);

            switch(chosenUpgrade)
            {
                case UpgradeType.Health:
                    {
                        playerHealth.IncreaseMaxHealth(20.0f);
                        tmText.SetText("Health Up!");
                        break;
                    }
                case UpgradeType.Energy:
                    {
                        windupManager.AddMaxEnergy(20.0f);
                        tmText.SetText("Energy Up!");
                        break;
                    }
                case UpgradeType.FireRate:
                    {
                        combatController.IncreaseFireRate(0.2f);
                        tmText.SetText("Fire Rate Up!");
                        break;
                    }
                case UpgradeType.Damage:
                    {
                        combatController.IncreaseDamageMulti(0.2f);
                        tmText.SetText("Damage Up!");
                        break;
                    }
                case UpgradeType.BulletPenetration:
                    {
                        combatController.IncreaseBulletPen(1);
                        tmText.SetText("Bullet Pen Up!");
                        break;
                    }
                case UpgradeType.NumOfBullets:
                    {
                        combatController.IncreaseBulletsPerShot(1);
                        tmText.SetText("Bullets Per Shot Up!");
                        break;
                    }
            }
            Destroy(textPopup, 4.0f);
            Invoke("StopSpinning", 2.0f);
        }
    }

    private void StopSpinning()
    {
        spinning = false;
    }

    private void OnInteract(bool input)
    {
        if (!input) return;
        if(playerOverlapping)
        {
            if(upgradeTokens > 0)
            {
                TryUpgrade();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerOverlapping = false;
        }
    }
}