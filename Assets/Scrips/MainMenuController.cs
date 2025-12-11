using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện để chuyển cảnh

public class MainMenuController : MonoBehaviour
{
    [Header("--- Kéo thả các Panel vào đây ---")]
    public GameObject mainPanel;            // Menu chính
    public GameObject charSelectPanel;      // Bảng chọn tướng
    public GameObject settingsPanel;        // Bảng cài đặt
    public GameObject leaderboardPanel;     // Bảng xếp hạng

    [Header("--- Tên Scene Game ---")]
    public string gameSceneName = "GameScene"; // Nhớ đổi tên này giống hệt tên Scene chơi game của bạn

    // --------------------------------------------------------
    // PHẦN 1: CÁC NÚT Ở MENU CHÍNH (MAIN MENU)
    // --------------------------------------------------------

    public void OnClick_Play()
    {
        // Tắt menu chính, Mở bảng chọn tướng
        mainPanel.SetActive(false);
        charSelectPanel.SetActive(true);
    }

    public void OnClick_Settings()
    {
        // Tắt menu chính, Mở bảng cài đặt
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnClick_Leaderboard()
    {
        // Tắt menu chính, Mở bảng xếp hạng
        mainPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
    }

    // --------------------------------------------------------
    // PHẦN 2: CHỨC NĂNG TRONG CÁC BẢNG CON
    // --------------------------------------------------------

    // Hàm chung cho nút Back/Close để quay về Menu chính
    public void OnClick_BackToMain()
    {
        // Tắt tất cả các bảng con
        charSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);
        leaderboardPanel.SetActive(false);

        // Bật lại menu chính
        mainPanel.SetActive(true);
    }

    // Hàm chọn nhân vật (Gán vào 3 nút nhân vật)
    // index = 0 (Tướng 1), index = 1 (Tướng 2), index = 2 (Tướng 3)
    public void OnClick_SelectCharacter(int index)
    {
        // Lưu lại lựa chọn của người chơi vào CharacterSelectionData
        CharacterSelectionData.SelectedCharacterIndex = index;

        // Lưu vào PlayerPrefs để lần sau mở lại
        PlayerPrefs.SetInt("SelectedCharacterID", index);
        PlayerPrefs.Save();

        Debug.Log("Đã chọn nhân vật số: " + index);
        
        // Vào game luôn
        SceneManager.LoadScene(gameSceneName);
    }
}