using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Common")]
    public float spawnRadius = 8f;
    public Transform player;
    public Timecount timeCounter;

    [Header("Normal Enemies")]
    public GameObject[] easyEnemies;
    public GameObject[] normalEnemies;
    public GameObject[] hardEnemies;

    public int maxEasyEnemies = 25;
    public int maxNormalEnemies = 80;
    public int maxHardEnemies = 100;

    [Header("Spawn Intervals (seconds between each spawn)")]
    public float easySpawnInterval = 2f;      // Đợt dễ - mỗi 2s spawn 1 con
    public float normalSpawnInterval = 1.5f;  // Đợt thường - mỗi 1.5s spawn 1 con
    public float hardSpawnInterval = 0.8f;    // Đợt khó - mỗi 0.8s spawn 1 con

    [Header("Boss")]
    public GameObject boss1Prefab;
    public GameObject boss2Prefab;

    public float boss1Time = 5f * 60f;
    public float boss2Time = 10f * 60f;

    // --- Runtime state ---
    private bool isBossActive = false;
    private bool boss1Spawned = false;
    private bool boss2Spawned = false;

    private float normalSpawnTimer = 0f;
    private float currentNormalSpawnInterval = 1.2f;

    void Start()
    {
        TryFindPlayer();
        currentNormalSpawnInterval = 1.2f;
    }

    void Update()
    {
        // Nếu player chưa có thì thử tìm lại
        if (player == null)
        {
            TryFindPlayer();
            if (player == null) return;
        }

        if (isBossActive)
            return;

        float gameTime = timeCounter.GetElapsedTime();

        UpdateNormalSpawnInterval(gameTime);

        normalSpawnTimer += Time.deltaTime;
        if (normalSpawnTimer >= currentNormalSpawnInterval)
        {
            TrySpawnNormalEnemy(gameTime);
            normalSpawnTimer = 0f;
        }

        CheckBossSpawn(gameTime);
    }

    // 👇 HÀM MỚI: tự tìm player theo tag
    void TryFindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
    // ----- phần dưới giữ nguyên code của bạn -----
    void UpdateNormalSpawnInterval(float gameTime)
    {
        // Đặt spawn interval theo thời gian game
        if (gameTime < 240f)
            currentNormalSpawnInterval = easySpawnInterval;
        else if (gameTime < 480f)
            currentNormalSpawnInterval = normalSpawnInterval;
        else
            currentNormalSpawnInterval = hardSpawnInterval;
    }

    void TrySpawnNormalEnemy(float gameTime)
    {
        int totalNormal = CountByTag("Enemy");

        GameObject prefabToSpawn = null;

        if (gameTime < 240f)
        {
            if (totalNormal >= maxEasyEnemies) return;
            prefabToSpawn = GetRandomFromArray(easyEnemies);
        }
        else if (gameTime < 480f)
        {
            if (totalNormal >= maxNormalEnemies) return;

            if (gameTime < 300f)
            {
                float r = Random.value;
                if (r < 0.6f)
                    prefabToSpawn = GetRandomFromArray(normalEnemies);
                else
                    prefabToSpawn = GetRandomFromArray(easyEnemies);
            }
            else
            {
                prefabToSpawn = GetRandomFromArray(normalEnemies);
            }
        }
        else
        {
            if (totalNormal >= maxHardEnemies) return;

            float r = Random.value;
            if (r < 0.7f)
                prefabToSpawn = GetRandomFromArray(hardEnemies);
            else
                prefabToSpawn = GetRandomFromArray(normalEnemies);
        }

        if (prefabToSpawn == null) return;

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = player.position + (Vector3)(randomDir * spawnRadius);

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }

    void CheckBossSpawn(float gameTime)
    {
        if (!boss1Spawned && gameTime >= boss1Time && boss1Prefab != null)
        {
            SpawnBoss(boss1Prefab);
            boss1Spawned = true;
            return;
        }

        if (!boss2Spawned && gameTime >= boss2Time && boss2Prefab != null)
        {
            SpawnBoss(boss2Prefab);
            boss2Spawned = true;
            return;
        }
    }

    void SpawnBoss(GameObject bossPrefab)
    {
        isBossActive = true;

        // Dừng đến khiểm thời gian khi boss xuất hiện
        if (timeCounter != null)
        {
            timeCounter.Stop();
        }

        Vector3 spawnPos = player.position + new Vector3(0f, 5f, 0f);
        GameObject bossObj = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
    }

    public void OnBossDied()
    {
        isBossActive = false;


        if (timeCounter != null)
        {
            timeCounter.Resume();
        }
    }

    GameObject GetRandomFromArray(GameObject[] arr)
    {
        if (arr == null || arr.Length == 0) return null;
        int index = Random.Range(0, arr.Length);
        return arr[index];
    }

    int CountByTag(string tag)
    {
        return GameObject.FindGameObjectsWithTag(tag).Length;
    }
}
