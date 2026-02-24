using UnityEngine;

public enum PickupType
{
    Health,
    Ammo
}

public class Pickup : MonoBehaviour
{
    [Header("Pickup")]
    public PickupType type;
    public int value = 10;

    [Header("Moving")]
    public float rotateSpeed = 90f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.25f;

    private Vector3 startPosition;
    private bool bobbingUp = true;

    private void Start()
    {
        // Save starting position so it bobs around this point
        startPosition = transform.position;
    }

    private void Update()
    {
        // Rotate around Y
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        // Bob up/down
        Vector3 offset = bobbingUp
            ? new Vector3(0f, bobHeight / 2f, 0f)
            : new Vector3(0f, -bobHeight / 2f, 0f);

        Vector3 targetPos = startPosition + offset;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            bobSpeed * Time.deltaTime
        );

        // Switch direction when close enough
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            bobbingUp = !bobbingUp;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();
        if (player == null) return;

        switch (type)
        {
            case PickupType.Health:
                player.GiveHealth(value);
                break;

            case PickupType.Ammo:
                player.GiveAmmo(value);
                break;
        }

        // Disable pickup after collecting
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // Reset bobbing each time object is enabled (useful if you later pool pickups)
        startPosition = transform.position;
        bobbingUp = true;
    }
}