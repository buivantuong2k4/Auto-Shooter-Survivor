using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpUI : MonoBehaviour
{

    private PlayerLevel playerLevel; // Sẽ tự tìm, không cần kéo nữa

    [Header("UI Elements")]
    public Slider xpSlider;
    public TextMeshProUGUI currentLevelText;

    // --- HÀM MỚI THÊM: TỰ TÌM NHÂN VẬT ---
    void Start()
    {
        // Nếu chưa kéo thả thì tự đi tìm script PlayerLevel trong game
        if (playerLevel == null)
        {
            playerLevel = FindFirstObjectByType<PlayerLevel>();
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
            playerLevel = FindFirstObjectByType<PlayerLevel>();
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
            currentLevelText.text = "LVL: " + playerLevel.level.ToString();

    }
}