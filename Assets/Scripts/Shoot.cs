using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    public GameObject core;
    public GameObject gun;
    public float rotationSpeed = 5f;

    private GameObject currentTarget;
    private Transform coreTransform;
    private Transform gunTransform;
    private Transform myTransform;
    private Vector3 targetDirection;
    private Vector3 coreTargetDirection;
    private Quaternion coreTargetRotation;
    private float angle;
    private Quaternion gunTargetRotation;
    private float deltaTime;

    void Start()
    {
        // Кэшируем компоненты при старте
        coreTransform = core.transform;
        gunTransform = gun.transform;
        myTransform = transform;

        // Предварительно создаем векторы
        targetDirection = Vector3.zero;
        coreTargetDirection = Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy") && currentTarget == null)
        {
            currentTarget = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (currentTarget == other.gameObject)
        {
            currentTarget = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget == null) return;

        deltaTime = Time.deltaTime * rotationSpeed;

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
    }
}
