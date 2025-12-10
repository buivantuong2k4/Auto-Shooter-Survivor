using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // ===================== HP =====================
    [Header("Health")]
    [SerializeField] private int baseMaxHP = 100;
    [SerializeField] private int hpPerLevel = 20;
    [SerializeField] private int healthLevel = 0;

    public int GetMaxHP() => baseMaxHP + healthLevel * hpPerLevel;
    public int GetHealthLevel() => healthLevel;
    public void IncreaseHealthLevel() => healthLevel++;


    // ===================== SPEED =====================
    [Header("Move Speed")]
    [SerializeField] private float baseMoveSpeed = 5f;
    [SerializeField] private float moveSpeedPerLevel = 0.5f;
    [SerializeField] private int speedLevel = 0;

    public float GetMoveSpeed() => baseMoveSpeed + speedLevel * moveSpeedPerLevel;
    public int GetSpeedLevel() => speedLevel;
    public void IncreaseSpeedLevel() => speedLevel++;


    // ===================== WEAPON STATS (ONLY LEVEL) =====================
    [Header("Weapon Levels Only")]
    [SerializeField] private int damageLevel = 0;
    [SerializeField] private int fireRateLevel = 0;
    [SerializeField] private int projectileLevel = 0;

    public int GetDamageLevel() => damageLevel;
    public int GetFireRateLevel() => fireRateLevel;
    public int GetProjectileLevel() => projectileLevel;

    public void IncreaseDamageLevel() => damageLevel++;
    public void IncreaseFireRateLevel() => fireRateLevel++;
    public void IncreaseProjectileLevel() => projectileLevel++;
}
