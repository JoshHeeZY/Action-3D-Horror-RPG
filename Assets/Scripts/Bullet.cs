using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 2f;

    private float shootTime;

    // Who fired this bullet?
    // true = player bullet, false = enemy bullet
    private bool firedByPlayer;

    public void SetOwner(bool isPlayerBullet)
    {
        firedByPlayer = isPlayerBullet;
    }

    private void OnEnable()
    {
        shootTime = Time.time;
    }

    private void Update()
    {
        // Disable the bullet after lifetime
        if (Time.time - shootTime >= lifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore triggers if needed (optional)
        // if (other.isTrigger) return;

        // Ignore self hits based on who fired the bullet
        if (firedByPlayer && other.CompareTag("Player"))
            return;

        if (!firedByPlayer && other.CompareTag("Enemy"))
            return;

        // Player bullet hits enemy
        if (firedByPlayer && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            gameObject.SetActive(false);
            return;
        }

        // Enemy bullet hits player
        if (!firedByPlayer && other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            gameObject.SetActive(false);
            return;
        }

        // Hit anything else (walls/ground/etc.)
        gameObject.SetActive(false);
    }
}