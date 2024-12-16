using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // ���������, ����� �� ������ ��� "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // ���������� �����
            Destroy(collision.gameObject);
        }

        // ���������� ���� ��� ����� ������������
        Destroy(gameObject);
    }
}
