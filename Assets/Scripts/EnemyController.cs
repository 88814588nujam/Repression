using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    public float laserSpeed = 50.0f;
    public AudioSource attackSource;

    private float nextFireTime = 0.0f;
    private float endSpeed = 500.0f;
    private bool isLeave = false;
    private Vector3 velocity;
    private EnemySpawner playerExplosion;

    void Start()
    {
        velocity = transform.forward * endSpeed;
        GameObject gameController = GameObject.FindWithTag("GameController");
        if(gameController != null) playerExplosion = gameController.GetComponent<EnemySpawner>();
    }

    void Update()
    {
        EnemyHyperspace enemyHyperspace = GetComponent<EnemyHyperspace>();
        if (enemyHyperspace != null)
        {
            if (enemyHyperspace.isArriving && Time.time >= nextFireTime)
            {
                bool isGameOver = false;
                if (playerExplosion != null) isGameOver = playerExplosion.isGameOver;
                if (!isGameOver)
                {
                    Attack();
                    nextFireTime = Time.time + fireRate;
                } else
                {
                    if (!isLeave) {
                        isLeave = true;
                        StartCoroutine(GameOverBehavior());
                    }
                }
            }
        } 
    }

    private void Attack()
    {
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f));
        LaserController laserBeam = laser.GetComponent<LaserController>();
        if (laserBeam != null) laserBeam.moveSpeed = laserSpeed;
        if (attackSource != null) attackSource.Play();
    }

    IEnumerator GameOverBehavior()
    {
        yield return new WaitForSeconds(Random.Range(5.0f, 5.5f));
        float duration = 5.0f;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.position += velocity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}