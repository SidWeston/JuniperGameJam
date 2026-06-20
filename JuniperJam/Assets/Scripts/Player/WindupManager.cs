using UnityEngine;
using UnityEngine.UI;

public class WindupManager : MonoBehaviour
{
    public float energy = 100.0f;
    public float energyGainRate = 2f;
    public float maxEnergy = 100.0f;

    //components
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Slider energySlider;

    //tuning
    [SerializeField] private float minRadius = 5f; //ignrore input too close to the center, but keep it small for people with small     
    
    private float lastAngle;
    private bool hasLastAngle = false;
    private Vector2 windupCenter = Vector2.zero;

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

        if(!movement)
        {
            movement = GetComponent<PlayerMovement>();
        }

        windupCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        //UI 
        energySlider.value = energy / maxEnergy;

        if(winding)
        {
            if(holdingPivot)
            {
                //windupCenter should be the center of the screen
                Vector2 offset = mousePos - windupCenter;

                if (offset.magnitude < minRadius)
                {
                    return;
                }

                //I still have no idea what an Atan2 is. thanks google.
                float currentAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

                //check if its the first time through
                if(!hasLastAngle)
                {
                    lastAngle = currentAngle;
                    hasLastAngle = true;
                    //cant compare them if they're the same, so return for now
                    return;
                }

                //calculate the angle to see how far the mouse has turned
                float delta = Mathf.DeltaAngle(lastAngle, currentAngle);
                lastAngle = currentAngle;
                
                //clamp it to 0, otherwise turning the other way reduces energy
                delta = Mathf.Clamp(delta, delta, 0);
                //finally add the energy on
                energy += (-delta * energyGainRate) * Time.deltaTime;
                energy = Mathf.Clamp(energy, 0, maxEnergy);
            }
        }
    }

    private void OnEnterWindup(bool input)
    {
        if (!input) return;

        winding = !winding;
        holdingPivot = false;
        hasLastAngle = false;        
    }

    private void OnMouseClick(bool input)
    {        
        if(input)
        {
            holdingPivot = true;            
        }
        else
        {
            holdingPivot = false;            
        }
        hasLastAngle = false;
    }

    private void OnMouseMove(Vector2 input)
    {
        mousePos = input;
    }   
}