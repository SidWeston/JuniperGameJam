using UnityEngine;
using Animancer;
using UnityEngine.Animations;
using System.Collections;

public class MenuAnimator : MonoBehaviour
{

    [SerializeField] private AnimancerComponent animancer;
    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip wakeUpAnimation;

    [SerializeField] private GameObject keyObj;
    private bool started;
    private bool firstFrame = true;

    [SerializeField] private MainMenu menu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animancer.Play(idleAnimation);
        StartCoroutine(StartRotatingKeyAfterFrame());
    }

    // Update is called once per frame
    void Update()
    {
        if(firstFrame)
        {            
            return;
        }

        if(!started)
        {
            keyObj.transform.Rotate(-100 * Time.deltaTime, 0, 0);
        }
    }

    private IEnumerator StartRotatingKeyAfterFrame()
    {

        while (firstFrame)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            keyObj.TryGetComponent(out ParentConstraint constraint);
            constraint.constraintActive = false;
            firstFrame = false;
        }

        yield return null;
    }

    public void PlayWakeUp()
    {
        animancer.Stop();
        animancer.Play(wakeUpAnimation);
        Invoke("StartGame", wakeUpAnimation.length + 0.15f);
    }

    public void StartGame()
    {
        menu.PlayGame();
    }
}
