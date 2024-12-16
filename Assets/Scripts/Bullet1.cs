using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    public float speed = 20f;  // Скорость пули
    public float lifetime = 3f;  // Время жизни пули
    public float damage = 10f;  // Урон пули

    void Start()
    {
        // Уничтожить пулю через заданное время
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Перемещение пули вперёд
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
