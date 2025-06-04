using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public List<ParticleSystem> particleSystems;
    public int particleCount = 100;
    public GameObject core;
    public GameObject gun;
    public TurretData turretData;
    public AudioSource shootSound;
    private GameObject currentTarget;
    private FindHome currentTargetScript;
    private Transform coreTransform;
    private Transform gunTransform;
    private Transform myTransform;
    private Vector3 targetDirection;
    private Vector3 coreTargetDirection;
    private Quaternion coreTargetRotation;
    private float angle;
    private Quaternion gunTargetRotation;
    private float deltaTime;
    private Quaternion coreStartRotation;
    private Quaternion gunStartRotation;
    private float aimToleranceDegrees = 100f;

    void Start()
    {
        // Кэшируем компоненты при старте
        coreTransform = core.transform;
        gunTransform = gun.transform;
        myTransform = transform;
        coreStartRotation = coreTransform.rotation;
        gunStartRotation = gunTransform.localRotation;

        // Предварительно создаем векторы
        targetDirection = Vector3.zero;
        coreTargetDirection = Vector3.zero;
        foreach (var particleSystem in particleSystems)
        {
            particleSystem.Stop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy") && currentTarget == null)
        {
            currentTarget = other.gameObject;
            currentTargetScript = currentTarget.GetComponent<FindHome>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (currentTarget == other.gameObject)
        {
            currentTarget = null;
            currentTargetScript = null;
            FindBestTarget();
        }
    }

    bool onCooldown = false;

    void ResetCooldown()
    {
        onCooldown = false;
    }

    void ShootTarget()
    {
        if (currentTarget != null)
        {
            if (!onCooldown)
            {
                onCooldown = true;
                Invoke("ResetCooldown", turretData.attackCooldown);
                shootSound.Play();
                foreach (var particleSystem in particleSystems)
                {
                    particleSystem.Emit(particleCount);
                }
                if (Random.Range(0, 100) < turretData.accuracy)
                {
                    if (currentTargetScript.TakeDamageAndCheckIfDead(turretData.damage))
                    {
                        FindBestTarget();
                    }
                }
            }
        }
    }

    void FindBestTarget()
    {
        Debug.Log("FindBestTarget");
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("enemy");
        GameObject bestTarget = null;
        float minAngle = float.MaxValue;
        float radius = GetComponent<SphereCollider>().radius * transform.localScale.x; // если радиус масштабируется

        foreach (var enemy in allEnemies)
        {
            if (currentTarget != null && enemy == currentTarget)
                continue;
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= radius)
            {
                Vector3 directionToEnemy = enemy.transform.position - transform.position;
                float angle = Vector3.Angle(coreTransform.forward, directionToEnemy);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    bestTarget = enemy;
                }
            }
        }

        Debug.Log("bestTarget: " + bestTarget);
        if (bestTarget != null)
        {
            currentTarget = bestTarget;
            currentTargetScript = currentTarget.GetComponent<FindHome>();
        }
        else
        {
            currentTarget = null;
            currentTargetScript = null;
        }
    }

    bool IsGunAimedAtTarget()
    {
        if (currentTarget == null) return false;

        Vector3 directionToTarget = (currentTarget.transform.position - gunTransform.position).normalized;
        float angle = Vector3.Angle(gunTransform.forward, directionToTarget);
        return angle <= aimToleranceDegrees;
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime = Time.deltaTime * turretData.rotationSpeed;

        if (currentTarget == null)
        {
            coreTransform.rotation = Quaternion.Slerp(coreTransform.rotation, coreStartRotation, deltaTime);
            gunTransform.localRotation = Quaternion.Slerp(gunTransform.localRotation, gunStartRotation, deltaTime);
            return;
        }

        // Вычисляем направление к цели
        targetDirection = currentTarget.transform.position - myTransform.position;

        // Core rotation (Y-axis only)
        coreTargetDirection.x = targetDirection.x;
        coreTargetDirection.z = targetDirection.z;

        if (coreTargetDirection.sqrMagnitude > 0.001f)
        {
            coreTargetRotation = Quaternion.LookRotation(coreTargetDirection);
            coreTransform.rotation = Quaternion.Slerp(coreTransform.rotation, coreTargetRotation, deltaTime);
        }

        // Gun rotation (X-axis only)
        if (targetDirection.sqrMagnitude > 0.001f)
        {
            angle = Vector3.SignedAngle(coreTransform.forward, targetDirection, coreTransform.right);
            gunTargetRotation = Quaternion.Euler(angle, 0, 0);
            gunTransform.localRotation = Quaternion.Slerp(gunTransform.localRotation, gunTargetRotation, deltaTime);
        }

        if (IsGunAimedAtTarget())
        {
            ShootTarget();
        }
    }
}
