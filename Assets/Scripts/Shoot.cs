using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;
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
                if (Random.Range(0, 100) < turretData.accuracy)
                {
                    currentTargetScript.TakeDamage(turretData.damage);
                }
            }
        }
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

        if (gunTransform.localRotation.eulerAngles.x > 10 && gunTransform.localRotation.eulerAngles.x < 170)
        {
            ShootTarget();
        }
    }
}
