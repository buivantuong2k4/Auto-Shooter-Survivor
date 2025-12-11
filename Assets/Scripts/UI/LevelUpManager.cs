using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager Instance;

    [Header("UI")]
    public GameObject levelUpPanel;
    public LevelUpOptionButton[] optionButtons; // Option1_Btn, Option2_Btn, Option3_Btn

    [Header("Danh sách tất cả lựa chọdwdn upgrade")]
    public List<UpgradeData> allUpgrades = new List<UpgradeData>();

    // --------- Stats & Weapons (lấy từ Player sau khi spawn) ----------
    private PlayerStats playerStats;

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
                    bow.weaponLevel = Mathf.Max(bow.weaponLevel, 1);
                    // nếu trong script có bool unlocked thì mở:
                    // bow.unlocked = true;
                }
                break;

            case 1: // char 2: Fireball
                if (fireball != null)
                {
                    fireball.weaponLevel = Mathf.Max(fireball.weaponLevel, 1);
                    // fireball.unlocked = true;
                }
                break;

            case 2: // char 3: Sword
                if (sword != null)
                {
                    sword.weaponLevel = Mathf.Max(sword.weaponLevel, 1);
                    // sword.unlocked = true;
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

        foreach (var u in allUpgrades)
        {
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
        switch (data.type)
        {
            // ----- STATS -----
            case UpgradeType.MaxHP:
                playerStats?.IncreaseHealthLevel();
                break;

            case UpgradeType.MoveSpeed:
                playerStats?.IncreaseSpeedLevel();
                break;

            case UpgradeType.GlobalDamage:
                playerStats?.IncreaseDamageLevel();
                break;

            case UpgradeType.GlobalFireRate:
                playerStats?.IncreaseFireRateLevel();
                break;

            case UpgradeType.GlobalProjectile:
                playerStats?.IncreaseProjectileLevel();
                break;

            // ----- WEAPONS -----
            case UpgradeType.BowWeapon:
                if (bow != null)
                {
                    // nếu có biến unlocked thì mở vũ khí ở lần đầu
                    // if (!bow.unlocked) { bow.unlocked = true; bow.weaponLevel = 1; }
                    // else bow.UpgradeWeapon();
                    bow.UpgradeWeapon();
                }
                break;

            case UpgradeType.FireBallWeapon:
                if (fireball != null)
                {
                    // if (!fireball.unlocked) { fireball.unlocked = true; fireball.weaponLevel = 1; }
                    // else fireball.UpgradeWeapon();
                    fireball.UpgradeWeapon();
                }
                break;

            case UpgradeType.KnifeWeapon:
                if (knife != null)
                {
                    // if (!knife.unlocked) { knife.unlocked = true; knife.weaponLevel = 1; }
                    // else knife.UpgradeWeapon();
                    knife.UpgradeWeapon();
                }
                break;

            case UpgradeType.ShurikenWeapon:
                if (shuriken != null)
                {
                    // if (!shuriken.unlocked) { shuriken.unlocked = true; shuriken.weaponLevel = 1; }
                    // else shuriken.UpgradeWeapon();
                    shuriken.UpgradeWeapon();
                }
                break;

            case UpgradeType.SwordWeapon:
                if (sword != null)
                {
                    // if (!sword.unlocked) { sword.unlocked = true; sword.weaponLevel = 1; }
                    // else sword.UpgradeWeapon();
                    sword.UpgradeWeapon();
                }
                break;
        }
    }
}
