using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHealth;
    public int damage;
    public int speed;
    public int attackRange;
    public int attackCooldown;
    public int attackDamage;
}
