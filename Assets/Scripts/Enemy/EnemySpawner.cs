using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Common")]
    public float spawnRadius = 8f;
    public Transform player;   // sẽ tự tìm object có tag "Player"

    [Header("Normal Enemies")]
    public GameObject[] easyEnemies;
    public GameObject[] normalEnemies;
    public GameObject[] hardEnemies;

    public int maxEasyEnemies = 25;
    public int maxNormalEnemies = 80;
    public int maxHardEnemies = 100;

    [Header("Spawn Speed by Wave")]
    public float easyWaveSpawnSpeed = 1f;      // Đợt dễ (tốc độ 1 = baseline)
    public float normalWaveSpawnSpeed = 1.3f;  // Đợt thường (30% nhanh hơn)
    public float hardWaveSpawnSpeed = 1.6f;    // Đợt khó (60% nhanh hơn)

    [Header("Boss")]
    public GameObject boss1Prefab;
    public GameObject boss2Prefab;

    public float boss1Time = 5f * 60f;
    public float boss2Time = 10f * 60f;

    // --- Runtime state ---
    private float gameTime = 0f;
    private bool isBossActive = false;
    private bool boss1Spawned = false;
    private bool boss2Spawned = false;

    private float normalSpawnTimer = 0f;
    private float currentNormalSpawnInterval = 1.2f;

    void Start()
    {
        TryFindPlayer();              // 🔥 thử tìm player lúc bắt đầu
        currentNormalSpawnInterval = 1.2f;
    }

    void Update()
    {
        // Nếu player chưa có (VD: spawn chậm hơn EnemySpawner) thì thử tìm lại
        if (player == null)
        {
            TryFindPlayer();
            if (player == null) return;   // vẫn chưa có player → chưa spawn gì
        }

        if (isBossActive)
            return;

        gameTime += Time.deltaTime;

        UpdateNormalSpawnInterval();

        normalSpawnTimer += Time.deltaTime;
        if (normalSpawnTimer >= currentNormalSpawnInterval)
        {
            TrySpawnNormalEnemy();
            normalSpawnTimer = 0f;
        }

        CheckBossSpawn();
    }

    // 👇 HÀM MỚI: tự tìm player theo tag
    void TryFindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            // Debug.Log("EnemySpawner: Found player = " + player.name);
        }
    }

    // ----- phần dưới giữ nguyên code của bạn -----
    void UpdateNormalSpawnInterval()
    {
        // Tốc độ base theo thời gian
        float baseInterval = 1.2f;
        if (gameTime < 120f)
            baseInterval = 1.2f;
        else if (gameTime < 240f)
            baseInterval = 0.9f;
        else if (gameTime < 300f)
            baseInterval = 0.7f;
        else if (gameTime < 390f)
            baseInterval = 0.6f;
        else if (gameTime < 480f)
            baseInterval = 0.45f;
        else if (gameTime < 540f)
            baseInterval = 0.35f;
        else
            baseInterval = 0.25f;

        // Áp dụng tốc độ spawn theo từng đợt
        float spawnSpeedMultiplier = 1f;

        if (gameTime < 240f)
            spawnSpeedMultiplier = easyWaveSpawnSpeed;
        else if (gameTime < 480f)
            spawnSpeedMultiplier = normalWaveSpawnSpeed;
        else
            spawnSpeedMultiplier = hardWaveSpawnSpeed;

        currentNormalSpawnInterval = baseInterval / spawnSpeedMultiplier;
    }

    void TrySpawnNormalEnemy()
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

    void CheckBossSpawn()
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

        Vector3 spawnPos = player.position + new Vector3(0f, 5f, 0f);
        GameObject bossObj = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
    }

    public void OnBossDied()
    {
        isBossActive = false;
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
