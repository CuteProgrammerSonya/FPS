using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Проверяем, имеет ли объект тег "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Уничтожаем врага
            Destroy(collision.gameObject);
        }

        // Уничтожаем пулю при любом столкновении
        Destroy(gameObject);
    }
}
