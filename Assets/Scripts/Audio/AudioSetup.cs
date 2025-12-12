using UnityEngine;
using UnityEngine.UI;

public class AudioSetup : MonoBehaviour
{
    public Toggle musicToggle;
    public Toggle sfxToggle;

    void Start()
    {
        if (AudioManager.Instance == null)
            return;

        // Khởi tạo trạng thái toggle
        if (musicToggle != null)
        {
            musicToggle.isOn = AudioManager.Instance.IsMusicOn();
            musicToggle.onValueChanged.AddListener(OnMusicChanged);
        }

        if (sfxToggle != null)
        {
            sfxToggle.isOn = AudioManager.Instance.IsSFXOn();
            sfxToggle.onValueChanged.AddListener(OnSFXChanged);
        }
    }

    void OnMusicChanged(bool isOn)
    {
        AudioManager.Instance.SetMusicState(isOn);
    }

    void OnSFXChanged(bool isOn)
    {
        AudioManager.Instance.SetSFXState(isOn);
    }
}
