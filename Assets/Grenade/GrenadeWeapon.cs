using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Подключаем для работы с Text UI
// using TMPro; // Используйте это, если хотите использовать TextMeshPro

public class GrenadeWeapon : MonoBehaviour
{
    public GameObject grenadePrefab;
    public Transform throwPoint;
    public float throwForce = 10f;
    public int grenadeCount = 10;
    public int maxGrenadeCount = 10; // Максимальное количество гранат
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float explosionDelay = 2f;
    public GameObject explosionEffectPrefab;
    public AudioClip throwSound;
    public AudioClip explosionSound;

    private AudioSource audioSource;

    // === ДЛЯ ВЫВОДА ТЕКСТА НА ЭКРАН ===
    public Text grenadeText; // Ссылка на текст из UI (если Text)
    // public TMP_Text grenadeText; // Если используете TextMeshPro

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateGrenadeUI(); // Обновляем UI при старте
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && grenadeCount > 0)
        {
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {
        grenadeCount--;
        audioSource.PlayOneShot(throwSound);

        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, Quaternion.identity);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        Vector3 throwDirection = transform.forward + Vector3.up;
        rb.AddForce(throwDirection * throwForce, ForceMode.VelocityChange);

        StartCoroutine(ExplodeAfterDelay(grenade));
        UpdateGrenadeUI(); // Обновляем UI после броска гранаты
    }

    IEnumerator ExplodeAfterDelay(GameObject grenade)
    {
        yield return new WaitForSeconds(explosionDelay);

        audioSource.PlayOneShot(explosionSound);

        GameObject explosionEffect = Instantiate(explosionEffectPrefab, grenade.transform.position, Quaternion.identity);
        Destroy(explosionEffect, 0.9f);

        Collider[] hitColliders = Physics.OverlapSphere(grenade.transform.position, explosionRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Destroy(hitCollider.gameObject);
            }
        }

        Destroy(grenade);
    }

    public void RefillGrenades()
    {
        grenadeCount = maxGrenadeCount;
        UpdateGrenadeUI(); // Обновляем UI после пополнения боезапаса
    }

    void UpdateGrenadeUI()
    {
        if (grenadeText != null)
        {
            grenadeText.text = grenadeCount + "/" + maxGrenadeCount;
        }
    }
}
