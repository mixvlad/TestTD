using UnityEngine;
using UnityEngine.Pool;

public class LevelManager : MonoBehaviour
{
    Spawn[] spawnPoints;

    static int numberOfWaves = 3;
    static int wavesEmitted = 0;
    static bool levelComplete = false;
    public static int playerHealth = 10;
    static bool nextWaveReady = false;
    public ParticleSystem deathParticlePrefab;
    public static IObjectPool<ParticleSystem> deathParticlePool;
    private static int totalEnemies = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 20;
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("spawn");
        spawnPoints = new Spawn[spawners.Length];
        for (int i = 0; i < spawners.Length; i++)
        {
            spawnPoints[i] = spawners[i].GetComponent<Spawn>();
            totalEnemies += spawnPoints[i].maxEnemiesToSpawn;
        }

        deathParticlePool = new ObjectPool<ParticleSystem>(
            () => Instantiate(deathParticlePrefab),
            (obj) => obj.gameObject.SetActive(true),
            (obj) => obj.gameObject.SetActive(false),
            null,
            true, 10, 20
        );
    }

    public static void CreateDeathExplosion(Vector3 position)
    {
        ParticleSystem explosion = deathParticlePool.Get();
        explosion.transform.position = position;
        explosion.Play();
    }

    public static void EnemyKilled(Vector3 position)
    {
        totalEnemies--;
        if (totalEnemies <= 0)
        {
            wavesEmitted++;
            nextWaveReady = true;

            if (wavesEmitted >= numberOfWaves)
            {
                levelComplete = true;
                nextWaveReady = false;
                Debug.Log("Level Complete");
            }
        }
        CreateDeathExplosion(position);
    }

    public static void EnemyReachedHome(Vector3 position)
    {
        totalEnemies--;
        playerHealth--;
        if (playerHealth <= 0)
        {
            Debug.Log("Game Over");
        }

        if (totalEnemies <= 0)
        {
            wavesEmitted++;
            nextWaveReady = true;

            if (wavesEmitted >= numberOfWaves)
            {
                levelComplete = true;
                nextWaveReady = false;
                Debug.Log("Level Complete");
            }
        }
    }

    void Update()
    {
        if (nextWaveReady)
        {
            nextWaveReady = false;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                totalEnemies += spawnPoints[i].maxEnemiesToSpawn;
                spawnPoints[i].ReStart();
            }

            Debug.Log("Next wave ready");
        }
    }
}
