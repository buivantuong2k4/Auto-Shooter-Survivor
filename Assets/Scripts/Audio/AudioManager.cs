using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    public string name;           // Tên để gọi (ví dụ: "BossShoot", "Jump")
    public AudioClip clip;        // File âm thanh
    [Range(0f, 1f)]
    public float volume = 1f;     // Âm lượng riêng của từng file
    [Range(0.1f, 3f)]
    public float pitch = 1f;      // Độ cao (1 là bình thường)
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton để gọi từ nơi khác

    [Header("Audio Sources")]
    public AudioSource musicSource; // Dùng phát nhạc nền (Loop)
    public AudioSource sfxSource;   // Dùng phát hiệu ứng (OneShot)

    [Header("Audio Clips Lists")]
    public Sound[] musicSounds;     // Danh sách nhạc nền
    public Sound[] sfxSounds;       // Danh sách hiệu ứng (bắn, nổ, nhảy...)

    void Awake()
    {
        // Thiết lập Singleton (Đảm bảo chỉ có 1 AudioManager tồn tại)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi chuyển Scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic("Theme");
    }

    // Hàm gọi nhạc nền
    public void PlayMusic(string name)
    {
        if (musicSource == null)
        {
            return;
        }

        if (musicSounds == null || musicSounds.Length == 0)
        {
            return;
        }

        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            return;
        }

        musicSource.clip = s.clip;
        musicSource.volume = s.volume;
        musicSource.pitch = s.pitch;
        musicSource.Play();
    }

    // Hàm gọi hiệu ứng âm thanh (SFX)
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            return;
        }

        // PlayOneShot cho phép nhiều âm thanh đè lên nhau
        sfxSource.PlayOneShot(s.clip, s.volume);
    }

    // ===== FUNCTIONS FOR UI EVENTS =====
    public void SetMusicState(bool isOn)
    {
        if (musicSource != null)
        {
            if (isOn)
            {
                musicSource.enabled = true;
                PlayMusic("Nen");
            }
            else
            {
                musicSource.enabled = false;
            }
        }
    }

    public void SetSFXState(bool isOn)
    {
        if (sfxSource != null)
        {
            sfxSource.enabled = isOn;
        }
    }

    public bool IsMusicOn()
    {
        return musicSource != null && musicSource.enabled;
    }

    public bool IsSFXOn()
    {
        return sfxSource != null && sfxSource.enabled;
    }
}