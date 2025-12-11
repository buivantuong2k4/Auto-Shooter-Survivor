using UnityEngine;
using Unity.Cinemachine;   // QUAN TRỌNG

public class PlayerSpawner : MonoBehaviour
{
    [Header("Danh sách prefab 3 nhân vật")]
    public GameObject[] playerPrefabs;   // 0,1,2

    [Header("Vị trí spawn")]
    public Transform spawnPoint;

    [Header("Cinemachine Camera sẽ follow player")]
    public CinemachineCamera cineCamera;

    private GameObject currentPlayer;

    void Start()
    {
        SpawnSelectedCharacter();
    }

    private void SpawnSelectedCharacter()
    {
        int index = CharacterSelectionData.SelectedCharacterIndex;

        if (playerPrefabs == null || playerPrefabs.Length == 0)
        {
            Debug.LogError("PlayerSpawner: Chưa gán playerPrefabs!");
            return;
        }

        if (index < 0 || index >= playerPrefabs.Length)
        {
            Debug.LogWarning("PlayerSpawner: Index ngoài range, set về 0");
            index = 0;
        }

        Vector3 pos = (spawnPoint != null) ? spawnPoint.position : Vector3.zero;
        Quaternion rot = Quaternion.identity;

        currentPlayer = Instantiate(playerPrefabs[index], pos, rot);
        LevelUpManager.Instance.RegisterPlayer(currentPlayer);


        // 🔥 GÁN TARGET CHO CINEMACHINE
        if (cineCamera != null && currentPlayer != null)
        {
            // Target là struct, phải copy ra rồi gán lại
            var t = cineCamera.Target;
            t.TrackingTarget = currentPlayer.transform;
            cineCamera.Target = t;
        }
        else
        {
            Debug.LogWarning("Chưa gán cineCamera hoặc currentPlayer null");
        }
    }
}
