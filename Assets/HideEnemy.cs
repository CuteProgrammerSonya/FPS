using UnityEngine;
using UnityEngine.AI;

public class HidingEnemy : MonoBehaviour
{
    public Transform player;  // Ссылка на игрока
    public float coverChangeDistance = 5f;  // Расстояние для смены укрытия
    public float attackRange = 10f;  // Дальность атаки
    public float shootingRange = 15f;  // Расстояние для стрельбы
    public float shootingInterval = 2f;  // Интервал между выстрелами
    public float lowDamage = 10f;  // Урон
    public LayerMask coverMask;  // Маска укрытий
    public GameObject bulletPrefab;  // Префаб пули
    public Transform shootPoint;  // Точка, из которой будет вылетать пуля

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

        // Смена укрытия при приближении игрока
        if (distanceToPlayer < coverChangeDistance)
        {
            FindNewCover();
        }

        // Атака игрока, если он в пределах дальности атаки
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

        // Смотрим на игрока
        transform.LookAt(player);

        // Стреляем, если игрок в пределах радиуса стрельбы
        if (distanceToPlayer <= shootingRange && shootingCooldown <= 0f)
        {
            Shoot();
            shootingCooldown = shootingInterval;
        }
    }

    void Shoot()
    {
        // Направление выстрела
        Vector3 shootingDirection = shootPoint.forward;

        // Создаем пулю на точке выстрела с направлением на игрока
        Instantiate(bulletPrefab, shootPoint.position, Quaternion.LookRotation(shootingDirection));

        Debug.Log("Enemy shot a bullet!");
    }

    void DealDamage(float damage)
    {
        // Здесь логика нанесения урона игроку, если нужно
        Debug.Log($"Player takes {damage} damage.");
    }
}
