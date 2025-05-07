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
            if (currentAmmo > 0 || infiniteAmmo == true)
            {
                return true;
            }
        }

        return false;
    }

    public void Shoot()
    {
        lastShootTime = Time.time;
        currentAmmo -= 1;

        GameObject bullet = bulletPool.GetObject();

                // Reposition and rotate the pooled bullet
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = muzzle.rotation;

        bullet.GetComponent<Rigidbody>().linearVelocity = muzzle.forward * bulletSpeed;
    }
}
