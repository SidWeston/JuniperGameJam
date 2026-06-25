using UnityEngine;
using Animancer;

public class PlayerAnimationController : MonoBehaviour
{
    public enum MovementState
    {
        Idle,
        Walking,
        Sprinting
    }

    public AnimancerComponent animancer;
    public PlayerMovement characterMovement;
    public CombatController combatController;

    private DirectionalAnimationSet8 currentAnimSet;
    [SerializeField] private DirectionalAnimationSet8 walkAnimSet;
    [SerializeField] private DirectionalAnimationSet8 sprintAnimSet;
    public AnimationClip idleAnim;

    public MovementState currentMovementState = MovementState.Idle; //assume player starts idle

    private Vector2 moveVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.moveEvent += OnMove;

        if (!animancer)
        {
            animancer = GetComponent<AnimancerComponent>();
        }

        currentAnimSet = walkAnimSet;

        animancer.Layers[0].Play(idleAnim);
        animancer.Layers[1].Play(combatController.currentWeapon.aimAnim);
    }

    public void UnSubInputs()
    {
        InputManager.instance.moveEvent -= OnMove;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 WorldToLocalFacing(Vector2 worldMoveVector, float facingYRotationDegrees)
    {
        float rad = -facingYRotationDegrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector2(worldMoveVector.x * cos - worldMoveVector.y * sin, worldMoveVector.x * sin + worldMoveVector.y * cos);
    }

    private void OnMove(Vector2 input)
    {
        moveVector = input;
        if(moveVector == Vector2.zero)
        {
            currentMovementState = MovementState.Idle;
            animancer.Layers[0].Play(idleAnim, 0.25f);
        }
        else
        {
            currentMovementState = MovementState.Walking;
            Vector2 relativeMove = WorldToLocalFacing(moveVector, transform.eulerAngles.y);
            animancer.Layers[0].Play(currentAnimSet.Get(relativeMove), 0.25f);
        }
    }
}
