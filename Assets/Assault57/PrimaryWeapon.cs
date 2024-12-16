using UnityEngine;
using UnityEngine.UI; // ���������� ������������ ��� ��� ������ � UI

public class PrimaryWeapon : MonoBehaviour
{
    public GameObject bulletPrefab; // ������ ����
    public Transform firePoint; // �����, ������ �������� ����
    public float bulletSpeed = 20f; // �������� ����
    public float fireRate = 0.5f; // �������� ����� ����������
    public float maxRange = 50f; // ������������ ��������� ��������
    public Text fireModeText; // UI ����� ��� ����������� ������ ����

    private float lastFireTime;
    private AudioSource audioSource; // �������� �����
    private bool isAutomatic = false; // ����� �������� (false � ������, true � ��������������)

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateFireModeText(); // ��������� ��������� �������� ������
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isAutomatic = !isAutomatic;
            Debug.Log("����� ���������: " + (isAutomatic ? "��������������" : "������"));
            UpdateFireModeText(); // ��������� ����� ��� ������������ ������
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
        audioSource.PlayOneShot(audioSource.clip); // ����������� ���� ��������

        // �������� ����� ������
        Camera mainCamera = Camera.main; // �������� ������
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // ��� �� ������ ������
        RaycastHit hit;

        Vector3 targetPoint;

        // ���������, �������� �� ��� � ������
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point; // ����� ���������
        }
        else
        {
            // ���� ��� ������ �� �����, ���� ������� ����� ����������� ������
            targetPoint = ray.GetPoint(100); // ����� �� ���������� 100 ������
        }

        // ������ ���� � ���������� �
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // ���������� ���� � ������� ����
        Vector3 direction = (targetPoint - firePoint.position).normalized;
        rb.velocity = direction * bulletSpeed;

        // ���������� ���� ����� ������������ �������
        Destroy(bullet, maxRange / bulletSpeed);
    }


    // ����� ��� ���������� ������ ������ ��������
    void UpdateFireModeText()
    {
        if (fireModeText != null)
        {
            string mode = isAutomatic ? "��������������" : "������";
            fireModeText.text = "�����: " + mode;
            Debug.Log("����� �������: " + fireModeText.text);
        }
        else
        {
            Debug.LogWarning("fireModeText �� ����������!");
        }
    }

}
