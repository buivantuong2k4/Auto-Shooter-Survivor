using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpOptionButton : MonoBehaviour
{
    public TMP_Text optionText;

    private UpgradeData upgradeData;
    private LevelUpManager manager;

    public void Setup(UpgradeData data, LevelUpManager mgr)
    {
        upgradeData = data;
        manager = mgr;

        optionText.text = data.displayName;
    }

    public void OnClick()
    {
        manager.OnChooseUpgrade(upgradeData);
    }
}
