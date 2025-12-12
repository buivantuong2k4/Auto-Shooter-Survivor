using UnityEngine;

public enum UpgradeType
{
    // Player Stats
    MaxHP,
    MoveSpeed,
    GlobalDamage,
    GlobalFireRate,
    GlobalProjectile,

    // Weapon Upgrades
    BowWeapon,
    FireBallWeapon,
    KnifeWeapon,
    ShurikenWeapon,
    SwordWeapon
}

[System.Serializable]
[CreateAssetMenu(fileName = "New UpgradeData", menuName = "UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string displayName;      // Tên hiển thị trên UI
    [TextArea] public string description;
    public Sprite icon;             // Nếu bạn muốn có icon sau này

    public UpgradeType type;
    public int maxLevel = 5;        // Cấp tối đa
}
