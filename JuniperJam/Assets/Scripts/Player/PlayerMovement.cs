using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //components
    [SerializeField] private CharacterController controller;

    //settings
    [SerializeField] private float moveSpeed;
    public float moveResource;
    [SerializeField] private float resourceDrainRate;

    //input variables
    private Vector2 moveVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.moveEvent += OnMove;
    }

    // Update is called once per frame
    void Update()
    {
        if(moveResource > 0)
        {
            //if movement input being inputted by the input
            if(moveVector.magnitude > 0)
            {
                Vector3 movement = new Vector3(moveVector.x, 0, moveVector.y);
                controller.Move(movement * moveSpeed * Time.deltaTime);
                moveResource -= resourceDrainRate * Time.deltaTime;
            }
        }
    }

    private void OnMove(Vector2 input)
    {
        moveVector = input;
    }
}