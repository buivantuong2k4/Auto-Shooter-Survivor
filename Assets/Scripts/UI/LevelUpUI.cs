using UnityEngine;
using UnityEngine.UI;
using TMPro; // Dùng cho TextMeshPro
using System.Collections.Generic; // Dùng cho List

public class LevelUpUI : MonoBehaviour
{
    public static LevelUpUI Instance;

    [Header("Dữ Liệu")]
    public List<UpgradeData> allUpgrades; // Kéo các thẻ bài vào đây
    private List<UpgradeData> currentChoices = new List<UpgradeData>(); 

    [Header("Giao Diện")]
    public GameObject panel;            // Panel LevelUp đen mờ
    public Button[] optionButtons;      // 3 Nút chọn (Option1, 2, 3)
    public TextMeshProUGUI[] optionTexts; // 3 Text (TMP) bên trong nút

    // Tham chiếu đến các script trên người Player
    private PlayerStats playerStats;
    private FireBurstWeapon fireBurstWeapon; 

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // --- DÒNG CẤP CỨU QUAN TRỌNG ---
        Time.timeScale = 1f; // Bắt buộc game phải chạy ở tốc độ bình thường
        // --------------------------------

        if (panel != null)
            panel.SetActive(false);

        // ... (Code cũ tìm Player giữ nguyên) ...
        panel.SetActive(false);

        // Tự động tìm Player và lấy script PlayerStats
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerStats = playerObj.GetComponent<PlayerStats>();
            // Tìm luôn script súng lửa (nếu có)
            fireBurstWeapon = playerObj.GetComponent<FireBurstWeapon>();
        }
    }

    // --- HÀM HIỆN BẢNG ---
    public void Show()
    {
        panel.SetActive(true);
        Time.timeScale = 0f; // Dừng game
        RandomizeUpgrades(); // Tráo bài lấy 3 món mới
    }

    // --- LOGIC RANDOM ---
    void RandomizeUpgrades()
    {
        List<UpgradeData> tempPool = new List<UpgradeData>(allUpgrades);
        currentChoices.Clear();

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (tempPool.Count > 0)
            {
                int randomIndex = Random.Range(0, tempPool.Count);
                UpgradeData selected = tempPool[randomIndex];
                
                currentChoices.Add(selected);
                
                // Cập nhật giao diện nút
                if (optionTexts[i] != null) 
                    optionTexts[i].text = selected.upgradeName;

                // Xóa khỏi danh sách tạm để không chọn trùng
                tempPool.RemoveAt(randomIndex);
            }
        }
    }

    // --- SỰ KIỆN CLICK (Gắn vào nút) ---
    public void OnOptionClicked(int index)
    {
        if (index < currentChoices.Count)
        {
            ApplyUpgrade(currentChoices[index]);
        }
        Hide();
    }

    // --- LOGIC CỘNG CHỈ SỐ (Khớp với PlayerStats của bạn) ---
    void ApplyUpgrade(UpgradeData data)
    {
        switch (data.upgradeID)
        {
            case 0: // Tăng Sát Thương (Damage)
                if (playerStats != null)
                {
                    playerStats.IncreaseDamageLevel(); // Gọi hàm trong PlayerStats
                    Debug.Log("Đã tăng Damage Level lên: " + playerStats.GetDamageLevel());
                }
                break;

            case 1: // Tăng Máu (HP)
                if (playerStats != null)
                {
                    playerStats.IncreaseHealthLevel(); // Gọi hàm trong PlayerStats
                    Debug.Log("Đã tăng HP Level lên: " + playerStats.GetHealthLevel());
                }
                break;

            case 2: // Tăng Tốc Độ (Speed)
                if (playerStats != null)
                {
                    playerStats.IncreaseSpeedLevel(); // Gọi hàm trong PlayerStats
                    Debug.Log("Đã tăng Speed Level lên: " + playerStats.GetSpeedLevel());
                }
                break;

            case 10: // Kỹ Năng Lửa (Fire Burst)
                if (fireBurstWeapon != null)
                {
                    fireBurstWeapon.UnlockOrUpgrade();
                    Debug.Log("Đã nâng cấp Fire Burst");
                }
                break;
        }
    }

    void Hide()
    {
        panel.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục game
    }
}