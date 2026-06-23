using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    public static WaveSystem instance;

    public int currentWave = 0;

    public float zombieHealth;
    public int numOfZombies;
    public int currentZombies; //number of zombies currently spawned
    public int maxCurrentZombies; //curreent zombies cant exceed this number
    public float minSpeed;
    public float maxSpeed;

    [SerializeField] private float healthGrowRate;
    [SerializeField] private float maxHealth;
    [SerializeField] private float speedGrowRate;
    [SerializeField] private float maxOverallSpeed;
    [SerializeField] private float countGrowth;

    private int zombiesToSpawn;
    private int maxZombiesToSpawn = 2, minZombiesToSpawn = 1; //the amount of zombies that can spawn at once
    private float spawnInterval = 1.5f;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private List<GameObject> spawnPoints;    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!instance)
        {
            instance = this;
        }        

        NextWave();
    }
    
    // Update is called once per frame
    void Update()
    {      
    }

    public void KillZombie()
    {
        currentZombies--;
        if (zombiesToSpawn == 0 && currentZombies == 0)
        {
            Invoke("NextWave", 4.0f);
        }        
    }

    public void NextWave()
    {        
        currentWave++;
        //manually set the first wave, then the next ones can increase algorithmically
        if(currentWave == 1)
        {
            zombieHealth = 100f;
            numOfZombies = 7;
            zombiesToSpawn = numOfZombies;
            minSpeed = 1f;
            maxSpeed = 2f;
            StartCoroutine(SpawnZombies());
            return;
        }

        numOfZombies = CalculateNumOfZombies();
        zombiesToSpawn = numOfZombies; //zombies to spawn decrements when a zombie is spawned, num of zombies is the total number in the wave, and will be used to calculate the next wave
        zombieHealth = CalculateZombieHealth();
        CalculateZombieSpeedRange();

        StartCoroutine(SpawnZombies());
    }

    public int CalculateNumOfZombies()
    {        
        return numOfZombies + Mathf.RoundToInt(countGrowth * (currentWave - 1));
    }

    public float CalculateZombieHealth()
    {
        return (zombieHealth * 1.1f) + 10;
    }

    public void CalculateZombieSpeedRange()
    {
        float growth = speedGrowRate * (currentWave - 1);

        float min = minSpeed + growth;
        float max = maxSpeed + growth;

        //soft cap on speed
        min = Mathf.Min(min, maxOverallSpeed - 0.5f);
        max = Mathf.Min(max, maxOverallSpeed);

        minSpeed = min;
        maxSpeed = max;
    }

    private IEnumerator SpawnZombies()
    {        
        while(zombiesToSpawn > 0)
        {            
            int amount = Random.Range(minZombiesToSpawn, maxZombiesToSpawn);
            List<GameObject> points = new List<GameObject>(spawnPoints);            

            for(int i = 0; i < amount; i++)
            {
                GameObject chosenPoint = points[Random.Range(0, points.Count)];
                GameObject spawnedZombie = Instantiate(zombiePrefab, chosenPoint.transform.position, Quaternion.identity);
                spawnedZombie.TryGetComponent(out AIEnemy enemy);
                enemy.SetStats(zombieHealth, Random.Range(minSpeed, maxSpeed));
                points.Remove(chosenPoint);
                zombiesToSpawn--;
                currentZombies++;
                if(zombiesToSpawn == 0)
                {
                    break;
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
        yield return null;
    }
}