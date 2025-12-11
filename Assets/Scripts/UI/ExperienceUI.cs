using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceUI : MonoBehaviour
{
    [Header("Player Reference")]
    public PlayerLevel playerLevel; // Sẽ tự tìm, không cần kéo nữa

    [Header("UI Elements")]
    public Slider xpSlider;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;

    // --- HÀM MỚI THÊM: TỰ TÌM NHÂN VẬT ---
    void Start()
    {
        // Nếu chưa kéo thả thì tự đi tìm script PlayerLevel trong game
        if (playerLevel == null)
        {
            playerLevel = FindObjectOfType<PlayerLevel>();
        }
    }

    void Update()
    {
        // Nếu tìm thấy rồi thì mới cập nhật
        if (playerLevel != null)
        {
            UpdateXPBar();
        }
        else
        {
            // Cố tìm lại nếu lỡ nhân vật chết đi sống lại
            playerLevel = FindObjectOfType<PlayerLevel>();
        }
    }

    void UpdateXPBar()
    {
        float progress = 0f;
        // Kiểm tra tránh lỗi chia cho 0
        if (playerLevel.xpToNextLevel > 0)
        {
            progress = (float)playerLevel.currentXP / playerLevel.xpToNextLevel;
        }

        if (xpSlider != null) xpSlider.value = progress;

        if (currentLevelText != null)
            currentLevelText.text = playerLevel.level.ToString();

        if (nextLevelText != null)
            nextLevelText.text = (playerLevel.level + 1).ToString();
    }
}