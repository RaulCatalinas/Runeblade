using UnityEngine;

public enum EnemyType
{
    Basic,
    Medium,
    Hard,
    Boss
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemies/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("General")]
    public EnemyType type;

    [Header("Stats")]
    public float maxHealth;
    public float attackDamage;
    public float moveSpeed;
    public float attackSpeed;

    [Header("Behavior")]
    public bool chasePlayer;
    public float detectionRange;
    public float attackRange;
    public float patrolRange;

    [Header("UI")]
    public bool showHealthBar;
}
