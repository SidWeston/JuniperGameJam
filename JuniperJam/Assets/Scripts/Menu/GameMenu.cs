using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public WaveSystem waveSystem;

    public TextMeshProUGUI roundText;

    public Button menuButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame()
    {
        roundText.SetText("Round Reached: " + waveSystem.currentWave);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
