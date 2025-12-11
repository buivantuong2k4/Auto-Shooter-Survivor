using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Common")]
    public float spawnRadius = 8f;
    public Transform player;

    [Header("Normal Enemies")]
    public GameObject[] easyEnemies;   // quái dễ (0-4 phút)
    public GameObject[] normalEnemies; // quái thường (4-8 phút)
    public GameObject[] hardEnemies;   // quái khó (8-10 phút)

    public int maxEasyEnemies = 25;
    public int maxNormalEnemies = 80;
    public int maxHardEnemies = 100;

    [Header("Elite Enemies")]
    public GameObject eliteType1;      // tinh anh dạng 1 (từ phút 4)
    public GameObject eliteType2;      // tinh anh dạng 2 (từ phút 8)
    public int maxEliteCount = 4;

    public float elite1StartTime = 4f * 60f;  // 4 phút
    public float elite2StartTime = 8f * 60f;  // 8 phút

    public Vector2 elite1SpawnIntervalRange = new Vector2(18f, 22f);
    public Vector2 elite2SpawnIntervalRange = new Vector2(25f, 30f);

    [Header("Boss")]
    public GameObject boss1Prefab;     // boss phút 5
    public GameObject boss2Prefab;     // boss phút 10

    public float boss1Time = 5f * 60f; // 5 phút
    public float boss2Time = 10f * 60f; // 10 phút

    // --- Runtime state ---
    private float gameTime = 0f;              // thời gian logic (dừng khi boss xuất hiện)
    private bool isBossActive = false;
    private bool boss1Spawned = false;
    private bool boss2Spawned = false;

    private float normalSpawnTimer = 0f;
    private float currentNormalSpawnInterval = 1.2f;

    private float eliteSpawnTimer = 0f;
    private float currentEliteSpawnInterval = 999f;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        // bắt đầu với quái dễ, spawn chậm
        currentNormalSpawnInterval = 1.2f;
        ResetEliteInterval();
    }

    void Update()
    {
        if (player == null) return;

        // Nếu boss đang sống: không tăng gameTime và không spawn quái nhỏ
        if (isBossActive)
            return;

        // Tăng thời gian logic
        gameTime += Time.deltaTime;

        // Cập nhật spawn interval theo gameTime
        UpdateNormalSpawnInterval();

        // Spawn quái thường
        normalSpawnTimer += Time.deltaTime;
        if (normalSpawnTimer >= currentNormalSpawnInterval)
        {
            TrySpawnNormalEnemy();
            normalSpawnTimer = 0f;
        }

        // Spawn quái tinh anh (từ 4 phút trở đi)
        eliteSpawnTimer += Time.deltaTime;
        if (eliteSpawnTimer >= currentEliteSpawnInterval)
        {
            TrySpawnEliteEnemy();
            ResetEliteInterval();
        }

        // Kiểm tra spawn boss
        CheckBossSpawn();
    }

    // -------------------------
    // NORMAL ENEMIES
    // -------------------------
    void UpdateNormalSpawnInterval()
    {
        // gameTime tính theo giây
        // 0–120s: 1.2s / con (rất dễ)
        // 120–240s: 0.9s / con
        // 240–300s: 0.7s / con
        // 300–390s: 0.6s / con
        // 390–480s: 0.45s / con
        // 480–540s: 0.35s / con
        // 540–600s: 0.25s / con

        if (gameTime < 120f) // 0–2p
            currentNormalSpawnInterval = 1.2f;
        else if (gameTime < 240f) // 2–4p
            currentNormalSpawnInterval = 0.9f;
        else if (gameTime < 300f) // 4–5p
            currentNormalSpawnInterval = 0.7f;
        else if (gameTime < 390f) // 5–6.5p
            currentNormalSpawnInterval = 0.6f;
        else if (gameTime < 480f) // 6.5–8p
            currentNormalSpawnInterval = 0.45f;
        else if (gameTime < 540f) // 8–9p
            currentNormalSpawnInterval = 0.35f;
        else // 9–10p
            currentNormalSpawnInterval = 0.25f;
    }

    void TrySpawnNormalEnemy()
    {
        int totalNormal = CountByTag("Enemy"); // quái thường dùng tag "Enemy"

        // Chọn loại quái theo thời gian
        GameObject prefabToSpawn = null;

        if (gameTime < 240f) // 0–4 phút: chỉ quái dễ
        {
            if (totalNormal >= maxEasyEnemies) return;
            prefabToSpawn = GetRandomFromArray(easyEnemies);
        }
        else if (gameTime < 480f) // 4–8 phút: quái thường
        {
            if (totalNormal >= maxNormalEnemies) return;

            // có thể cho xen kẽ 1 ít quái dễ
            if (gameTime < 300f) // 4–5p: chuyển dần
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
        else // 8–10 phút: quái khó
        {
            if (totalNormal >= maxHardEnemies) return;

            float r = Random.value;
            // 70% khó, 30% thường
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

    // -------------------------
    // ELITE ENEMIES
    // -------------------------
    void ResetEliteInterval()
    {
        eliteSpawnTimer = 0f;

        // trước phút 4: không spawn tinh anh
        if (gameTime < elite1StartTime)
        {
            currentEliteSpawnInterval = 999f;
            return;
        }

        // từ phút 8 trở đi: có cả 2 dạng
        if (gameTime >= elite2StartTime)
        {
            // tinh anh dồi dào hơn tí
            currentEliteSpawnInterval = Random.Range(15f, 25f);
        }
        else
        {
            // chỉ dạng 1
            currentEliteSpawnInterval = Random.Range(elite1SpawnIntervalRange.x, elite1SpawnIntervalRange.y);
        }
    }

    void TrySpawnEliteEnemy()
    {
        if (gameTime < elite1StartTime) return; // chưa đến 4p

        int eliteCount = CountByTag("Elite");
        if (eliteCount >= maxEliteCount) return;

        GameObject eliteToSpawn = null;

        if (gameTime >= elite2StartTime && eliteType2 != null)
        {
            // từ phút 8 trở đi: random dạng 1 hoặc 2
            float r = Random.value;
            if (r < 0.6f && eliteType1 != null)
                eliteToSpawn = eliteType1;
            else
                eliteToSpawn = eliteType2;
        }
        else
        {
            // chỉ dạng 1
            eliteToSpawn = eliteType1;
        }

        if (eliteToSpawn == null) return;

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = player.position + (Vector3)(randomDir * spawnRadius);

        Instantiate(eliteToSpawn, spawnPos, Quaternion.identity);
    }

    // -------------------------
    // BOSS
    // -------------------------
    void CheckBossSpawn()
    {
        // Boss 1 ở 5 phút
        if (!boss1Spawned && gameTime >= boss1Time && boss1Prefab != null)
        {
            SpawnBoss(boss1Prefab);
            boss1Spawned = true;
            return;
        }

        // Boss 2 ở 10 phút
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

        // spawn boss gần player (hoặc giữa map tuỳ bạn)
        Vector3 spawnPos = player.position + new Vector3(0f, 5f, 0f);
        GameObject bossObj = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        // Gợi ý: trong script Boss, khi chết hãy gọi:
        // FindObjectOfType<EnemySpawner>().OnBossDied();
    }

    // Hàm public để Boss gọi khi chết
    public void OnBossDied()
    {
        isBossActive = false;

        // sau khi boss chết: gameTime tiếp tục tăng, spawn quái trở lại
    }

    // -------------------------
    // Helper
    // -------------------------
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
