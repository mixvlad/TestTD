using UnityEngine;
using UnityEngine.Pool;

public class LevelManager : MonoBehaviour
{
    Spawn[] spawnPoints;

    public static int numberOfWaves = 3;
    public static int wavesEmitted = 0;
    public static int totalMoney = 200;
    public static bool levelEnded = false;
    public static int playerHealth = 10;
    public static int playerMaxHealth = 10;
    public static bool nextWaveReady = false;
    public static int totalEnemies = 0;
    public GameObject gameOverPanel;
    public ParticleSystem deathParticlePrefab;
    public static IObjectPool<ParticleSystem> deathParticlePool;

    int timeBetweenWaves = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Time.timeScale = 20;
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

    public static void OnSpeedChanged(int speed)
    {
        Time.timeScale = speed;
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
                levelEnded = true;
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
            levelEnded = true;
            Debug.Log("Game Over");
        }

        if (totalEnemies <= 0)
        {
            wavesEmitted++;
            nextWaveReady = true;

            if (wavesEmitted >= numberOfWaves)
            {
                levelEnded = true;
                nextWaveReady = false;
                Debug.Log("Level Complete");
            }
        }
    }

    void ResetSpawners()
    {
        foreach (Spawn spawnPoint in spawnPoints)
        {
            totalEnemies += spawnPoint.maxEnemiesToSpawn;
            spawnPoint.ReStart();
        }
        OnSpeedChanged(1);
    }
    void Update()
    {
        if (nextWaveReady)
        {
            nextWaveReady = false;
            Invoke("ResetSpawners", timeBetweenWaves);
            Debug.Log("Next wave ready");
        }
        if (levelEnded)
        {
            OnSpeedChanged(0);
            gameOverPanel.SetActive(true);
            gameOverPanel.transform.SetAsLastSibling();
        }
    }
}
