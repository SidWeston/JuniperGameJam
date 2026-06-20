using UnityEngine;

public class CombatController : MonoBehaviour
{
    //components
    [SerializeField] private GameObject body;

    [SerializeField] private float rotationSpeed;

    private Plane groundPlane;

    //inputs
    private Vector2 mousePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundPlane = new Plane(Vector3.up, transform.position);
        InputManager.instance.mouseEvent += OnMouseMove;
    }

    // Update is called once per frame
    void Update()
    {
        //look towards mouse pos
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if(groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);
            Vector3 direction = worldPoint - transform.position;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }        
    }

    private void OnMouseMove(Vector2 input)
    {
        mousePos = input;
    }
}