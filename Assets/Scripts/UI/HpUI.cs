using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpUI : MonoBehaviour
{
    private PlayerHealth playerHealth; // Sẽ tự tìm, không cần kéo nữa

    [Header("UI Elements")]
    public Slider hpSlider;
    public TextMeshProUGUI currentHpText;

    // --- HÀM MỚI THÊM: TỰ TÌM NHÂN VẬT ---
    void Start()
    {
        // Nếu chưa kéo thả thì tự đi tìm script PlayerHealth trong game
        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }
    }

    void Update()
    {
        // Nếu tìm thấy rồi thì mới cập nhật
        if (playerHealth != null)
        {
            UpdateHpBar();
        }
        else
        {
            // Cố tìm lại nếu lỡ nhân vật chết đi sống lại
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }
    }

    void UpdateHpBar()
    {
        float progress = 0f;
        float currentHp = playerHealth.GetCurrentHP();
        float maxHp = playerHealth.GetMaxHP();

        // Kiểm tra tránh lỗi chia cho 0
        if (maxHp > 0)
        {
            progress = currentHp / maxHp;
        }

        if (hpSlider != null) hpSlider.value = progress;

        if (currentHpText != null)
            currentHpText.text = "HP: " + Mathf.RoundToInt(currentHp) + "/" + Mathf.RoundToInt(maxHp);
    }
}
