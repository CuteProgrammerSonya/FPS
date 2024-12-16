using UnityEngine;
using UnityEngine.UI; // Подключаем пространство имён для работы с UI

public class PrimaryWeapon : MonoBehaviour
{
    public GameObject bulletPrefab; // Префаб пули
    public Transform firePoint; // Точка, откуда вылетает пуля
    public float bulletSpeed = 20f; // Скорость пули
    public float fireRate = 0.5f; // Задержка между выстрелами
    public float maxRange = 50f; // Максимальная дальность выстрела
    public Text fireModeText; // UI текст для отображения режима огня

    private float lastFireTime;
    private AudioSource audioSource; // Источник звука
    private bool isAutomatic = false; // Режим стрельбы (false — ручной, true — автоматический)

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateFireModeText(); // Установим начальное значение текста
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isAutomatic = !isAutomatic;
            Debug.Log("Режим стрельбыы: " + (isAutomatic ? "Автоматический" : "Ручной"));
            UpdateFireModeText(); // Обновляем текст при переключении режима
        }

        if (isAutomatic)
        {
            if (Input.GetButton("Fire1") && Time.time > lastFireTime + fireRate)
            {
                Shoot();
                lastFireTime = Time.time;
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time > lastFireTime + fireRate)
            {
                Shoot();
                lastFireTime = Time.time;
            }
        }
    }

    void Shoot()
    {
        audioSource.PlayOneShot(audioSource.clip); // Проигрываем звук выстрела

        // Получаем центр экрана
        Camera mainCamera = Camera.main; // Основная камера
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Луч из центра экрана
        RaycastHit hit;

        Vector3 targetPoint;

        // Проверяем, попадает ли луч в объект
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point; // Точка попадания
        }
        else
        {
            // Если луч никуда не попал, пуля полетит вдоль направления камеры
            targetPoint = ray.GetPoint(100); // Точка на расстоянии 100 единиц
        }

        // Создаём пулю и направляем её
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Направляем пулю в сторону цели
        Vector3 direction = (targetPoint - firePoint.position).normalized;
        rb.velocity = direction * bulletSpeed;

        // Уничтожаем пулю после определённого времени
        Destroy(bullet, maxRange / bulletSpeed);
    }


    // Метод для обновления текста режима стрельбы
    void UpdateFireModeText()
    {
        if (fireModeText != null)
        {
            string mode = isAutomatic ? "автоматический" : "ручной";
            fireModeText.text = "Режим: " + mode;
            Debug.Log("Текст обновлён: " + fireModeText.text);
        }
        else
        {
            Debug.LogWarning("fireModeText не установлен!");
        }
    }

}
