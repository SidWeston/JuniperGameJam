using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float healthRegenRate; //amount regenerated per second
    [SerializeField] private float healthRegenDelay;
    private bool regenHealth;

    [SerializeField] private Slider healthBar;

    public AudioSource audioSource;

    public GameObject gameUI;
    public GameObject gameOverUI;
    public TextMeshProUGUI roundText;
    public WaveSystem waveSystem;
    public UpgradeManager upgradeManager;
    public PlayerAnimationController animController;

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
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
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
        gameUI.SetActive(false);
        gameOverUI.SetActive(true);
        roundText.SetText("Round Reached: " + waveSystem.currentWave);

        UnSubInputs();

        gameObject.TryGetComponent(out PlayerMovement movement);
        movement.enabled = false;
        gameObject.TryGetComponent(out CombatController combat);
        combat.enabled = false;        
    }
    
    private void UnSubInputs()
    {
        gameObject.TryGetComponent(out PlayerMovement movement);
        movement.UnSubInput();
        gameObject.TryGetComponent(out CombatController combat);
        combat.UnSubInput();
        gameObject.TryGetComponent(out WindupManager windup);
        windup.UnSubInput();
        upgradeManager.UnSubInput();
        animController.UnSubInputs();
    }

    public void ReturnToMenu()
    {

        SceneManager.LoadScene(0);
    }

    public void IncreaseMaxHealth(float increase)
    {
        maxHealth += increase;
        currentHealth = maxHealth;
    }
}
