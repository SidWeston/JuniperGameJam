using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuStuff;
    public GameObject howToPlayStuff;

    public MenuAnimator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPressed()
    {
        animator.PlayWakeUp();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void HowToPlay()
    {
        howToPlayStuff.SetActive(true);
        menuStuff.SetActive(false);
    }

    public void BackButton()
    {
        howToPlayStuff.SetActive(false);
        menuStuff.SetActive(true); 
    }
}
