using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float spawnRadius = 8f;

    private float timer = 0f;
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null || enemyPrefab == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        // spawn ngẫu nhiên trên vòng tròn quanh player
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = player.position + (Vector3)(randomDir * spawnRadius);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
