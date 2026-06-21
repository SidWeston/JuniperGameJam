using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float lifeTime = 4.0f;

    [Tooltip("will be set/overridden by the weapon when fired")]
    public float damage; 

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

    private void OnTriggerEnter(Collider other)
    {        
        if(other.gameObject.tag == "Enemy")
        {            
            other.gameObject.TryGetComponent(out AIEnemy enemy);
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
