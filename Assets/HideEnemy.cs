using UnityEngine;
using UnityEngine.AI;

public class HidingEnemy : MonoBehaviour
{
    public Transform player;  // ������ �� ������
    public float coverChangeDistance = 5f;  // ���������� ��� ����� �������
    public float attackRange = 10f;  // ��������� �����
    public float shootingRange = 15f;  // ���������� ��� ��������
    public float shootingInterval = 2f;  // �������� ����� ����������
    public float lowDamage = 10f;  // ����
    public LayerMask coverMask;  // ����� �������
    public GameObject bulletPrefab;  // ������ ����
    public Transform shootPoint;  // �����, �� ������� ����� �������� ����

    private NavMeshAgent agent;
    private Transform currentCover;
    private float shootingCooldown;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindNewCover();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // ����� ������� ��� ����������� ������
        if (distanceToPlayer < coverChangeDistance)
        {
            FindNewCover();
        }

        // ����� ������, ���� �� � �������� ��������� �����
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer(distanceToPlayer);
        }
    }

    void FindNewCover()
    {
        Transform closestCover = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject cover in GameObject.FindGameObjectsWithTag("Cover"))
        {
            float distance = Vector3.Distance(transform.position, cover.transform.position);
            if (distance < closestDistance && cover.transform != currentCover)
            {
                closestDistance = distance;
                closestCover = cover.transform;
            }
        }

        if (closestCover != null)
        {
            currentCover = closestCover;
            agent.SetDestination(currentCover.position);
        }
    }

    void AttackPlayer(float distanceToPlayer)
    {
        shootingCooldown -= Time.deltaTime;

        // ������� �� ������
        transform.LookAt(player);

        // ��������, ���� ����� � �������� ������� ��������
        if (distanceToPlayer <= shootingRange && shootingCooldown <= 0f)
        {
            Shoot();
            shootingCooldown = shootingInterval;
        }
    }

    void Shoot()
    {
        // ����������� ��������
        Vector3 shootingDirection = shootPoint.forward;

        // ������� ���� �� ����� �������� � ������������ �� ������
        Instantiate(bulletPrefab, shootPoint.position, Quaternion.LookRotation(shootingDirection));

        Debug.Log("Enemy shot a bullet!");
    }

    void DealDamage(float damage)
    {
        // ����� ������ ��������� ����� ������, ���� �����
        Debug.Log($"Player takes {damage} damage.");
    }
}
