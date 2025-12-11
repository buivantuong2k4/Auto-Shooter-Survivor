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

    [Header("Elite Enemies")]
    public GameObject eliteType1;
    public GameObject eliteType2;
    public int maxEliteCount = 4;

    public float elite1StartTime = 4f * 60f;
    public float elite2StartTime = 8f * 60f;

    public Vector2 elite1SpawnIntervalRange = new Vector2(18f, 22f);
    public Vector2 elite2SpawnIntervalRange = new Vector2(25f, 30f);

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

    private float eliteSpawnTimer = 0f;
    private float currentEliteSpawnInterval = 999f;

    void Start()
    {
        TryFindPlayer();              // 🔥 thử tìm player lúc bắt đầu
        currentNormalSpawnInterval = 1.2f;
        ResetEliteInterval();
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

        eliteSpawnTimer += Time.deltaTime;
        if (eliteSpawnTimer >= currentEliteSpawnInterval)
        {
            TrySpawnEliteEnemy();
            ResetEliteInterval();
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
        if (gameTime < 120f)
            currentNormalSpawnInterval = 1.2f;
        else if (gameTime < 240f)
            currentNormalSpawnInterval = 0.9f;
        else if (gameTime < 300f)
            currentNormalSpawnInterval = 0.7f;
        else if (gameTime < 390f)
            currentNormalSpawnInterval = 0.6f;
        else if (gameTime < 480f)
            currentNormalSpawnInterval = 0.45f;
        else if (gameTime < 540f)
            currentNormalSpawnInterval = 0.35f;
        else
            currentNormalSpawnInterval = 0.25f;
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

    void ResetEliteInterval()
    {
        eliteSpawnTimer = 0f;

        if (gameTime < elite1StartTime)
        {
            currentEliteSpawnInterval = 999f;
            return;
        }

        if (gameTime >= elite2StartTime)
        {
            currentEliteSpawnInterval = Random.Range(15f, 25f);
        }
        else
        {
            currentEliteSpawnInterval = Random.Range(elite1SpawnIntervalRange.x, elite1SpawnIntervalRange.y);
        }
    }

    void TrySpawnEliteEnemy()
    {
        if (gameTime < elite1StartTime) return;

        int eliteCount = CountByTag("Elite");
        if (eliteCount >= maxEliteCount) return;

        GameObject eliteToSpawn = null;

        if (gameTime >= elite2StartTime && eliteType2 != null)
        {
            float r = Random.value;
            if (r < 0.6f && eliteType1 != null)
                eliteToSpawn = eliteType1;
            else
                eliteToSpawn = eliteType2;
        }
        else
        {
            eliteToSpawn = eliteType1;
        }

        if (eliteToSpawn == null) return;

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = player.position + (Vector3)(randomDir * spawnRadius);

        Instantiate(eliteToSpawn, spawnPos, Quaternion.identity);
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
