using UnityEngine;
using UnityEngine.UI;

public class LevelUpCardUI : MonoBehaviour
{
    public Image iconImage;
    public Text titleText;
    public Text descriptionText;
    public Button button;

    private UpgradeData upgradeData;
    private LevelUpManager manager;

    public void Setup(UpgradeData data, LevelUpManager mgr)
    {
        upgradeData = data;
        manager = mgr;

        if (iconImage != null) iconImage.sprite = data.icon;
        if (titleText != null) titleText.text = data.displayName;
        if (descriptionText != null) descriptionText.text = data.description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (manager != null && upgradeData != null)
        {
            manager.OnChooseUpgrade(upgradeData);
        }
    }
}
