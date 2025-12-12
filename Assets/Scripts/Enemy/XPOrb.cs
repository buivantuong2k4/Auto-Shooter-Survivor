using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public int xpValue = 5; // lượng XP cho mỗi orb
    private float destroyTime = 60f; // tự động destroy sau 1 phút

    private void Start()
    {
        // Destroy orb sau 60 giây nếu không được pick up
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("PLAYER xu");
        PlayerLevel playerLevel = other.GetComponent<PlayerLevel>();
        if (playerLevel != null)
        {
            AudioManager.Instance.PlaySFX("XP");
            playerLevel.AddXP(xpValue);
            Destroy(gameObject);
        }
    }
}
