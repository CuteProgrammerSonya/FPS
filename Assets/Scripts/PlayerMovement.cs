using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -20f;
    public float jumpHeight = 3f;

    private Vector3 velocity;
    private bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // Максимальная высота прыжка для проверки
    private float maxJumpHeight = 4f;

    void Update()
    {
        // Проверяем, на земле ли игрок
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Удерживаем игрока на поверхности
        }

        // Движение по горизонтали
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        // Прыжок с проверкой по высоте
        if (Input.GetButtonDown("Jump") && isGrounded && transform.position.y < maxJumpHeight)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Применяем гравитацию
        if (!isGrounded || velocity.y > 0) // Чтобы гравитация не применялась дважды на земле
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        // Визуализация проверки земли
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
