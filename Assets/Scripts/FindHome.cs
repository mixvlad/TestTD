using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FindHome : MonoBehaviour
{
    public Transform Destination;
    public EnemyData enemyData;
    public Slider healthBarPrefab;
    Slider healthBar;
    float stoppingDistance = 1f;
    NavMeshAgent ai;
    bool hasReachedDestination = false;
    float currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ai = GetComponent<NavMeshAgent>();
        ai.speed = enemyData.speed;
        currentHealth = enemyData.maxHealth;
        ai.SetDestination(Destination.position);
        healthBar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        healthBar.transform.SetParent(GameObject.Find("Canvas").transform);
        healthBar.maxValue = enemyData.maxHealth;
        healthBar.value = enemyData.maxHealth;
    }

    public bool TakeDamageAndCheckIfDead(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        if (currentHealth <= 0)
        {
            LevelManager.totalMoney += enemyData.reward;
            LevelManager.EnemyKilled(transform.position + Vector3.up * 1f);
            Destroy(healthBar.gameObject);
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we've actually reached the destination
        if (!hasReachedDestination &&
            ai.remainingDistance <= stoppingDistance &&
            ai.hasPath)
        {
            LevelManager.EnemyReachedHome(transform.position + Vector3.up * 1f);
            ai.ResetPath();
            Destroy(healthBar.gameObject);
            Destroy(gameObject, 0.1f);
        }

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
            healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
        }
    }
}
