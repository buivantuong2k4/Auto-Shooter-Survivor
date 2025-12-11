using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectUI : MonoBehaviour
{
    // Gọi hàm này khi bấm nút chọn nhân vật 1
    public void SelectCharacter1()
    {
        CharacterSelectionData.SelectedCharacterIndex = 0;
        LoadGameScene();
    }

    // Nhân vật 2
    public void SelectCharacter2()
    {
        CharacterSelectionData.SelectedCharacterIndex = 1;
        LoadGameScene();
    }

    // Nhân vật 3
    public void SelectCharacter3()
    {
        CharacterSelectionData.SelectedCharacterIndex = 2;
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        // Đổi "GameScene" thành đúng tên scene chơi game của bạn
        SceneManager.LoadScene("GameScene");
    }
}
