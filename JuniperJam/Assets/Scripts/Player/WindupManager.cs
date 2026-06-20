using UnityEngine;

public class WindupManager : MonoBehaviour
{
    //tuning
    [SerializeField] private float minRadius = 20f;
    


    //inputs
    private bool winding = false;
    private bool holdingPivot = false;
    private Vector2 mousePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.windupKey.keyPress += OnEnterWindup;
        InputManager.instance.shootKey.keyPress += OnMouseClick;
        InputManager.instance.mouseEvent += OnMouseMove;
    }

    // Update is called once per frame
    void Update()
    {
        if(winding)
        {

        }
    }

    private void OnEnterWindup(bool input)
    {
        winding = input;
    }

    private void OnMouseClick(bool input)
    {

    }

    private void OnMouseMove(Vector2 input)
    {
        mousePos = input;
    }   
}