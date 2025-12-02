using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;

    public AudioClip shootSound; // оставляем только звук
    public ParticleSystem muzzleFlash; // эффект выстрела

    private AudioSource audioSource; 
    private float nextFireTime = 0f;

    void Awake()
    {
        // AudioSource как и раньше
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        AimAtMouse();
        Shoot();
    }

    void AimAtMouse()
    {
        var cam = Camera.main;
        if (cam == null) return;

        float zDist = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDist));
        Vector2 direction = (mousePos - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            // Particle System 
            if (muzzleFlash != null)
                muzzleFlash.Play(); // запускаем эффект

            //Логика пули 
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            var cam = Camera.main;
            Vector3 mousePos;
            if (cam != null)
            {
                float zDist = Mathf.Abs(cam.transform.position.z - firePoint.position.z);
                mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDist));
            }
            else
            {
                mousePos = Input.mousePosition;
            }

            Vector2 direction = (mousePos - firePoint.position).normalized;

            var b = bullet.GetComponent<Bullet>();
            if (b != null)
                b.SetDirection(direction);

            // Звук выстрела
            if (shootSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
        }
    }
}
