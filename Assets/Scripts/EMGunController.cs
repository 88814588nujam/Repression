using UnityEngine;

public class EMGunController : MonoBehaviour
{
    [Header("EMGun Setting")]
    public float range = 500.0f;
    public float fireRate = 1.0f;
    public LayerMask targetMask;

    [Header("Visual Effect")]
    public GameObject emBeamPrefab;
    public Transform firePoint;
    public AudioSource fireSource;
    
    private float nextTimeToFire = 0.0f;
    private EnemySpawner playerExplosion;

    void Start()
    {
        GameObject gameController = GameObject.FindWithTag("GameController");
        if (gameController != null) playerExplosion = gameController.GetComponent<EnemySpawner>();
    }

    void Update()
    {
        bool isGameOver = false;
        if (playerExplosion != null) isGameOver = playerExplosion.isGameOver;
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire && !isGameOver) TryFireAtTarget();
    }

    private void TryFireAtTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range, targetMask))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Vector3 direction = (hit.point - firePoint.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                FireEMBeam(direction, hit.collider.gameObject);
            }
        }
    }

    private void FireEMBeam(Vector3 direction, GameObject target)
    {
        nextTimeToFire = Time.time + 1.0f / fireRate;
        if (fireSource != null) fireSource.Play();
        if (emBeamPrefab != null && firePoint != null)
        {
            GameObject beam = Instantiate(emBeamPrefab, firePoint.position, Quaternion.LookRotation(direction));
            EMBeamController projectile = beam.GetComponent<EMBeamController>();
            if (projectile != null) projectile.Initialize(target);
        }
    }
}