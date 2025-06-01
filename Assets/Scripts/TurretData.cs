using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "ScriptableObjects/TurretData", order = 2)]
public class TurretData : ScriptableObject
{
    public float damage;
    public float attackRange;
    public float attackCooldown;
    public int price;
    public float rotationSpeed;
    public float accuracy;
}
