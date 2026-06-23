using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float healthRegenRate; //amount regenerated per second
    [SerializeField] private float healthRegenDelay;
    private bool regenHealth;

    [SerializeField] private Slider healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = currentHealth / maxHealth;
        if(regenHealth && currentHealth < maxHealth)
        {
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, currentHealth, maxHealth);
            if(currentHealth >= maxHealth)
            {
                regenHealth = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        CancelInvoke("StartHealthRegen");
        regenHealth = false;
        if(currentHealth <= 0)
        {
            KillPlayer();
            return;
        }
        Invoke("StartHealthRegen", healthRegenDelay);
    }

    public void StartHealthRegen()
    {
        regenHealth = true;
    }

    public void KillPlayer()
    {
        //end the game, display round reached, enemies killed, other stats?
    }

    public void IncreaseMaxHealth(float increase)
    {
        maxHealth += increase;
        currentHealth = maxHealth;
    }
}
