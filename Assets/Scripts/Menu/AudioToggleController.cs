using UnityEngine;
using UnityEngine.UI;

public class AudioToggleController : MonoBehaviour
{
    public Toggle musicToggle;
    public Toggle sfxToggle;

    void Start()
    {
        // Kiểm tra AudioManager
        if (AudioManager.Instance == null)
        {
            return;
        }

        // Khởi tạo trạng thái toggle theo AudioManager
        if (musicToggle != null)
        {
            bool musicOn = AudioManager.Instance.IsMusicOn();
            musicToggle.isOn = musicOn;

            // Thêm listener cho On Value Changed event
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        }

        if (sfxToggle != null)
        {
            bool sfxOn = AudioManager.Instance.IsSFXOn();
            sfxToggle.isOn = sfxOn;

            // Thêm listener cho On Value Changed event
            sfxToggle.onValueChanged.AddListener(OnSFXToggleChanged);
        }
    }

    public void OnMusicToggleChanged(bool isOn)
    {
        AudioManager.Instance.SetMusicState(isOn);
    }

    public void OnSFXToggleChanged(bool isOn)
    {
        AudioManager.Instance.SetSFXState(isOn);
    }
}
