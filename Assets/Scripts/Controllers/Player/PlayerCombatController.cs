using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float damage = 3.5f;
    [SerializeField] private Transform sword;
    [SerializeField] private PlayerAnimator playerAnimator;

    public void Attack()
    {
        playerAnimator.SetAttackAnimation(true);

        Debug.Log($"Attacking for {damage} damage");

        playerAnimator.SetAttackAnimation(false);
    }
}
