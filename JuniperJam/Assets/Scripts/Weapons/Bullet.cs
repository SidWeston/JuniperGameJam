using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float lifeTime = 4.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * Time.deltaTime;
    }
}
