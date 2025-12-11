using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager Instance;

    [Header("UI")]
    public GameObject levelUpPanel;
    public LevelUpOptionButton[] optionButtons; // Option1_Btn, Option2_Btn, Option3_Btn

    [Header("Danh sách tất cả lựa chọn upgrade")]
    public List<UpgradeData> allUpgrades = new List<UpgradeData>();

    // --------- Stats & Weapons (lấy từ Player sau khi spawn) ----------
    private PlayerStats playerStats;
    private PlayerHealth playerHealth;

    // đảm bảo các script weapon có: public int weaponLevel;  (và nếu được thêm public bool unlocked;)
    private BowWeapon bow;
    private FireBallWeapon fireball;
    private KnifeWeapon knife;
    private ShurikenWeapon shuriken;
    private SwordWeapon sword;

    private bool isChoosing = false;

    //==================================================================
    void Awake()
    {
        Instance = this;
        if (levelUpPanel != null)
            levelUpPanel.SetActive(false);
    }

    //==================================================================
    //      HÀM NÀY ĐƯỢC PlayerSpawner GỌI SAU KHI Instantiate xong
    //==================================================================
    public void RegisterPlayer(GameObject player)
    {
        if (player == null)
        {
            Debug.LogError("LevelUpManager.RegisterPlayer: player == null");
            return;
        }

        // Lấy stats + vũ khí từ prefab (tìm cả trong con)
        playerStats = player.GetComponent<PlayerStats>();
        playerHealth = player.GetComponent<PlayerHealth>();

        bow = player.GetComponentInChildren<BowWeapon>();
        fireball = player.GetComponentInChildren<FireBallWeapon>();
        knife = player.GetComponentInChildren<KnifeWeapon>();
        shuriken = player.GetComponentInChildren<ShurikenWeapon>();
        sword = player.GetComponentInChildren<SwordWeapon>();

        // Set vũ khí khởi đầu theo nhân vật chọn
        SetupStartingWeapon();
    }

    // char1: bow lv1, char2: fireball lv1, char3: sword lv1
    void SetupStartingWeapon()
    {
        int idx = CharacterSelectionData.SelectedCharacterIndex;

        switch (idx)
        {
            case 0: // char 1: Bow
                if (bow != null)
                {
                    bow.UnlockWeapon();
                }
                break;

            case 1: // char 2: Fireball
                if (fireball != null)
                {
                    fireball.UnlockWeapon();
                }
                break;

            case 2: // char 3: Sword
                if (sword != null)
                {
                    sword.UnlockWeapon();
                }
                break;
        }
    }

    //==================================================================
    //                      HIỆN UI LEVEL UP
    //==================================================================
    public void ShowLevelUp()
    {
        if (isChoosing) return;

        // lấy những upgrade chưa max level
        List<UpgradeData> available = GetAvailableUpgrades();
        if (available.Count == 0)
        {
            // không còn gì để nâng
            return;
        }

        isChoosing = true;
        Time.timeScale = 0f;
        levelUpPanel.SetActive(true);

        // random 3 lựa chọn từ available
        List<UpgradeData> picks = GetRandomChoices(available, optionButtons.Length);

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(i < picks.Count);

            if (i < picks.Count)
            {
                optionButtons[i].Setup(picks[i], this);
            }
        }
    }

    //==================================================================
    //                GỌI KHI CLICK 1 LỰA CHỌN
    //==================================================================
    public void OnChooseUpgrade(UpgradeData upgrade)
    {
        ApplyUpgrade(upgrade);

        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
        isChoosing = false;
    }

    //==================================================================
    //                    RANDOM & GIỚI HẠN LEVEL
    //==================================================================
    List<UpgradeData> GetAvailableUpgrades()
    {
        List<UpgradeData> result = new List<UpgradeData>();

        // Kiểm tra allUpgrades có dữ liệu không
        if (allUpgrades == null || allUpgrades.Count == 0)
        {
            Debug.LogWarning("LevelUpManager: allUpgrades is null or empty! Assign UpgradeData in Inspector!");
            return result;
        }

        foreach (var u in allUpgrades)
        {
            if (u == null) continue;  // Skip null entries

            int currentLvl = GetCurrentLevel(u.type);
            if (currentLvl < u.maxLevel)
                result.Add(u);
        }

        return result;
    }

    List<UpgradeData> GetRandomChoices(List<UpgradeData> source, int count)
    {
        List<UpgradeData> pool = new List<UpgradeData>(source);
        List<UpgradeData> result = new List<UpgradeData>();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return result;
    }

    int GetCurrentLevel(UpgradeType type)
    {
        switch (type)
        {
            // ----- STATS -----
            case UpgradeType.MaxHP: return playerStats != null ? playerStats.GetHealthLevel() : 0;
            case UpgradeType.MoveSpeed: return playerStats != null ? playerStats.GetSpeedLevel() : 0;
            case UpgradeType.GlobalDamage: return playerStats != null ? playerStats.GetDamageLevel() : 0;
            case UpgradeType.GlobalFireRate: return playerStats != null ? playerStats.GetFireRateLevel() : 0;
            case UpgradeType.GlobalProjectile: return playerStats != null ? playerStats.GetProjectileLevel() : 0;

            // ----- WEAPONS -----
            case UpgradeType.BowWeapon: return bow != null ? bow.weaponLevel : 0;
            case UpgradeType.FireBallWeapon: return fireball != null ? fireball.weaponLevel : 0;
            case UpgradeType.KnifeWeapon: return knife != null ? knife.weaponLevel : 0;
            case UpgradeType.ShurikenWeapon: return shuriken != null ? shuriken.weaponLevel : 0;
            case UpgradeType.SwordWeapon: return sword != null ? sword.weaponLevel : 0;
        }

        return 0;
    }

    //==================================================================
    //                     ÁP DỤNG NÂNG CẤP
    //==================================================================
    void ApplyUpgrade(UpgradeData data)
    {
        Debug.Log("ApplyUpgrade: " + data.type);

        switch (data.type)
        {
            // ----- STATS -----
            case UpgradeType.MaxHP:
                if (playerStats != null)
                {
                    playerStats.IncreaseHealthLevel();

                    // Cập nhật maxHP và currentHP trong PlayerHealth
                    if (playerHealth != null)
                    {
                        playerHealth.RefreshMaxHP();
                    }
                }
                break;

            case UpgradeType.MoveSpeed:
                if (playerStats != null)
                {
                    playerStats.IncreaseSpeedLevel();
                }
                break;

            case UpgradeType.GlobalDamage:
                if (playerStats != null)
                {
                    playerStats.IncreaseDamageLevel();
                }
                break;

            case UpgradeType.GlobalFireRate:
                if (playerStats != null)
                {
                    playerStats.IncreaseFireRateLevel();
                }
                break;

            case UpgradeType.GlobalProjectile:
                if (playerStats != null)
                {
                    playerStats.IncreaseProjectileLevel();
                }
                break;

            // ----- WEAPONS -----
            case UpgradeType.BowWeapon:
                if (bow != null)
                {
                    bow.UpgradeWeapon();
                }
                break;

            case UpgradeType.FireBallWeapon:
                if (fireball != null)
                {
                    fireball.UpgradeWeapon();
                }
                break;

            case UpgradeType.KnifeWeapon:
                if (knife != null)
                {
                    knife.UpgradeWeapon();
                }
                break;

            case UpgradeType.ShurikenWeapon:
                if (shuriken != null)
                {
                    shuriken.UpgradeWeapon();
                }
                break;

            case UpgradeType.SwordWeapon:
                if (sword != null)
                {
                    sword.UpgradeWeapon();
                }
                break;
        }
    }
}
