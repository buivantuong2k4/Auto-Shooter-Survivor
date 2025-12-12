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
            return;
        }

        // Gán text
        if (optionText != null)
        {
            int currentLvl = manager.GetCurrentLevel(data.type);
            optionText.text = $"{data.displayName} (Lv.{currentLvl})";
        }

        // Gán icon từ UpgradeData nếu có
        if (iconImage != null)
        {
            if (data.icon != null)
            {
                iconImage.sprite = data.icon;
            }
        }
    }

    public void OnClick()
    {
        manager.OnChooseUpgrade(upgradeData);
    }
}
