using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform destination;
    public float spawnRate = 1f;
    public float spawnDelay = 1f;

    public int maxSpawns = 10;
    private int currentSpawns = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnObject", spawnDelay, spawnRate);
    }

    public void SpawnObject()
    {
        if (currentSpawns < maxSpawns)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            FindHome findHome = spawnedObject.GetComponent<FindHome>();
            findHome.Destination = destination;
            currentSpawns++;
        }
    }

}
