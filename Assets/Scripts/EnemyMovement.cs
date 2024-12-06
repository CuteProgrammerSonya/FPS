using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] points; // Массив точек, к которым будет двигаться враг
    public GameObject bulletPrefab;  // Префаб пули
    public Transform shootPoint;  // Точка, из которой будет вылетать пуля
    public float shootingRange = 10f;  // Расстояние для стрельбы
    public float shootingInterval = 2f;  // Интервал между выстрелами
    public float detectionRange = 15f;  // Радиус обнаружения игрока

    private NavMeshAgent agent;  // Ссылка на NavMeshAgent
    private Transform player;  // Ссылка на игрока
    private int currentPoint = 0;  // Индекс текущей цели
    private float nextShotTime = 0f;  // Время следующего выстрела

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Находим игрока по тегу
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Устанавливаем первую цель
        MoveToNextPoint();
    }

    void Update()
    {
        // Проверяем, достиг ли враг цели
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Если достиг, переходим к следующей точке
            MoveToNextPoint();
        }

        // Проверка на обнаружение игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Враг смотрит на игрока
            transform.LookAt(player);

            // Если враг в пределах радиуса для стрельбы
            if (distanceToPlayer <= shootingRange && Time.time >= nextShotTime)
            {
                Shoot();
                nextShotTime = Time.time + shootingInterval;
            }
        }
    }

    void MoveToNextPoint()
    {
        if (points.Length == 0)
            return;

        agent.destination = points[currentPoint].position;
        currentPoint = (currentPoint + 1) % points.Length;  // Выбираем следующую точку
    }

    void Shoot()
    {
        // Поворачиваем врага на игрока
        transform.LookAt(player);

        // Направление выстрела (используем forward, чтобы пулю направить в сторону врага)
        Vector3 shootingDirection = shootPoint.forward;

        // Создаем пулю на точке выстрела с нужной ориентацией
        Instantiate(bulletPrefab, shootPoint.position, Quaternion.LookRotation(shootingDirection));
    }


}
