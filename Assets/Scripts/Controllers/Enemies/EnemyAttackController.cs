using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    public void Attack()
    {
        Debug.Log("Attacking player...");
    }
}
