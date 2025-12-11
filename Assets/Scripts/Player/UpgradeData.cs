using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Game/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName; // Tên hiển thị (VD: Tăng Sát Thương)
    [TextArea] public string description; // Mô tả
    public Sprite icon; // Ảnh minh họa
    
    // ID QUY ĐỊNH:
    // 0 = Damage, 1 = HP, 2 = Speed, 10 = FireBurst (Kỹ năng Lửa)
    public int upgradeID; 
}