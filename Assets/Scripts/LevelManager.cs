using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] spawners;
    public GameObject[] enemies;
    public int maxEnemies = 10;
    private static int currentEnemies = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawners = GameObject.FindGameObjectsWithTag("spawn");
        foreach (GameObject spawner in spawners)
        {
            currentEnemies += spawner.GetComponent<Spawn>().maxSpawns;
        }
    }

    public static void EnemyKilled()
    {
        currentEnemies--;
        if (currentEnemies <= 0)
        {
            Debug.Log("Level Complete");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
