using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public GameObject panel;
    public Button btnMainUpgrade;
    public Button btnUnlockFireBurst;

    private PlayerStats playerStats;
    private FireBurstWeapon fireBurstWeapon;
    public static LevelUpUI Instance;

    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        panel.SetActive(false);

        // tìm Player trong scene
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerStats = playerObj.GetComponent<PlayerStats>();
            fireBurstWeapon = playerObj.GetComponent<FireBurstWeapon>(); // sau sẽ add script này
        }

        // gán sự kiện click
        btnMainUpgrade.onClick.AddListener(OnMainUpgradeClicked);
        btnUnlockFireBurst.onClick.AddListener(OnUnlockFireBurstClicked);
    }

    public void Show()
    {
        panel.SetActive(true);
        Time.timeScale = 0f; // pause game
    }

    void Hide()
    {
        panel.SetActive(false);
        Time.timeScale = 1f; // resume
    }

    void OnMainUpgradeClicked()
    {
        if (playerStats != null)
        {
            playerStats.mainWeaponDamageMultiplier += 0.2f; // +20% damage
        }
        Hide();
    }

    void OnUnlockFireBurstClicked()
    {
        if (fireBurstWeapon != null)
        {
            fireBurstWeapon.UnlockOrUpgrade();
        }
        Hide();
    }
}
