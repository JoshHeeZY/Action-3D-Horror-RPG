using UnityEngine;

public class Weapon : MonoBehaviour
{
    public ObjectPool bulletPool;
    public Transform muzzle;

    public int currentAmmo;
    public int maxAmmo;
    public bool infiniteAmmo;

    public float bulletSpeed;
    public float shootRate;
    private float lastShootTime;

    public bool isPlayer;

    void Awake()
    {
        // If this object also has a Player component, mark it as player-controlled
        if (GetComponent<Player>())
        {
            isPlayer = true;
        }
    }

    public bool CanShoot()
    {
        if (Time.time - lastShootTime >= shootRate)
        {
            if (currentAmmo > 0 || infiniteAmmo)
            {
                return true;
            }
        }

        return false;
    }

    public void Shoot()
    {
        lastShootTime = Time.time;

        if (!infiniteAmmo)
        {
            currentAmmo -= 1;
        }

        GameObject bullet = bulletPool.GetObject();
        if (bullet == null) return;

        // Make sure pooled bullet is active
        bullet.SetActive(true);

        // Spawn slightly in front of muzzle so it doesn't instantly hit shooter
        bullet.transform.position = muzzle.position + (muzzle.forward * 0.5f);
        bullet.transform.rotation = muzzle.rotation;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = muzzle.forward * bulletSpeed;
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetOwner(isPlayer);
        }
    }
}