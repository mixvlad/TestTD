using UnityEngine;
using UnityEngine.AI;

public class FindHome : MonoBehaviour
{
    public Transform Destination;
    float stoppingDistance = 1f;
    NavMeshAgent ai;
    bool hasReachedDestination = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ai = GetComponent<NavMeshAgent>();
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
