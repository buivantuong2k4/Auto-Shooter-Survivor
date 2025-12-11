using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator anim;

    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int DieHash = Animator.StringToHash("Die");

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.LogError("Enemy Animator not found!");
    }

    public void SetRunning(bool isRunning)
    {
        anim.SetBool(IsRunningHash, isRunning);
    }

    public void PlayAttack()
    {
        anim.SetTrigger(AttackHash);
    }

    public void PlayDeath()
    {
        anim.SetTrigger(DieHash);
    }
}
