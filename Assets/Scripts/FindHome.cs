using UnityEngine;
using UnityEngine.AI;

public class FindHome : MonoBehaviour
{
    public Transform Destination;
    public EnemyData enemyData;
    float stoppingDistance = 1f;
    NavMeshAgent ai;
    bool hasReachedDestination = false;
    int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ai = GetComponent<NavMeshAgent>();
        ai.speed = enemyData.speed;
        currentHealth = enemyData.maxHealth;
        ai.SetDestination(Destination.position);
        Debug.Log($"Agent started moving to: {Destination.position}");
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we've actually reached the destination
        if (!hasReachedDestination &&
            ai.remainingDistance <= stoppingDistance &&
            ai.hasPath)
        {
            LevelManager.EnemyKilled();
            ai.ResetPath();
            Destroy(gameObject, 0.1f);
        }
    }
}
