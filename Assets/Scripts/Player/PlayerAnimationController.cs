using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;

    // Dùng hash cho nhanh (không bắt buộc)
    private static readonly int ShootHash = Animator.StringToHash("Shoot");
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    private static readonly int DieHash = Animator.StringToHash("Die");

    void Awake()
    {
        // TỰ TÌM Animator trên cùng GameObject
        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogError("PlayerAnimationController: Không tìm thấy Animator trên " + gameObject.name);
        }
    }

    public void PlayShoot()
    {
        if (anim == null) return;
        anim.SetTrigger(ShootHash);
    }
    public void SetRunning(bool isRunning)
    {
        anim.SetBool(IsRunningHash, isRunning);
    }
    public void PlayDeath()
    {
        anim.SetTrigger(DieHash);
    }
}
