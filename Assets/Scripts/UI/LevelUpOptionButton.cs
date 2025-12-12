using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpOptionButton : MonoBehaviour
{
    public TMP_Text optionText;
    public Image iconImage;  // Thêm Image component để hiển thị icon

    private UpgradeData upgradeData;
    private LevelUpManager manager;

    public void Setup(UpgradeData data, LevelUpManager mgr)
    {
        upgradeData = data;
        manager = mgr;

        if (data == null)
        {
            Debug.LogError("LevelUpOptionButton.Setup: data is null!");
            return;
        }

        // Debug để kiểm tra
        Debug.Log($"Setup button: {data.displayName}, Icon: {(data.icon != null ? "YES" : "NO")}");

        // Gán text
        if (optionText != null)
        {
            optionText.text = data.displayName;
            Debug.Log($"Text set to: {data.displayName}");
        }
        else
        {
            Debug.LogWarning("optionText is not assigned!");
        }

        // Gán icon từ UpgradeData nếu có
        if (iconImage != null)
        {
            if (data.icon != null)
            {
                iconImage.sprite = data.icon;
                Debug.Log($"Icon set successfully!");
            }
            else
            {
                Debug.LogWarning($"UpgradeData '{data.displayName}' has no icon assigned!");
            }
        }
        else
        {
            Debug.LogWarning("iconImage is not assigned on button!");
        }
    }

    public void OnClick()
    {
        manager.OnChooseUpgrade(upgradeData);
    }
}
