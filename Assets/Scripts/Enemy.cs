using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int CurrentHP = 15;
    public int MaxHP = 15;
    public int ScoreToGive = 1;

    [Header("Combat")]
    public float attackRange = 2f;

    private NavMeshAgent agent;
    private GameObject target;
    private Weapon weapon;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        weapon = GetComponent<Weapon>();

        Player playerScript = FindObjectOfType<Player>();
        if (playerScript != null)
        {
            target = playerScript.gameObject;
        }
    }

    void Update()
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.transform.position);

        if (dist <= attackRange)
        {
            if (weapon != null && weapon.CanShoot())
            {
                weapon.Shoot();
            }
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
        }

        RotateTowardsTarget(); // NEW: ensure enemy always looks at player
    }

    void RotateTowardsTarget()
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        dir.y = 0f; // prevent tilting up/down
        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }
}
