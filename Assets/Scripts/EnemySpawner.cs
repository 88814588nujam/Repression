using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject showOver;
    public GameObject showOverWords;
    public GameObject EnemySpaceShipPrefab;
    public Transform PlayerPlanetPoint;
    public float minSpawnDistance = 50.0f;
    public float maxSpawnDistance = 100.0f;
    public float initialSpawnInterval = 5.0f;
    public float minSpawnInterval = 1.0f;
    public float currentSpawnInterval = 0.0f;
    public float intervalReduction = 1.0f;
    public int SpawnSpeedCount = 5;
    public int maxEnemies = 20;
    public int enemyCount = 0;
    public bool isGameOver = false;
    public bool isShowOver = false;

    private bool isShowOverEnd = false;
    private int shipsSpawned = 0;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        if (!isGameOver)
            InvokeRepeating("SpawnShip", 0.0f, currentSpawnInterval);
    }

    void Update()
    {
        if (isShowOver && !isShowOverEnd)
        {
            isShowOverEnd = true;
            StartCoroutine(ShowGameOver());
        }
    }

    private void SpawnShip()
    {
        if (isGameOver || enemyCount >= maxEnemies)
            return;
        if (PlayerPlanetPoint != null)
        {
            Vector3 randomDirection = Random.onUnitSphere;
            float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 targetPosition = PlayerPlanetPoint.position + randomDirection * randomDistance;
            GameObject shipInstance = Instantiate(EnemySpaceShipPrefab, transform.position, Quaternion.identity);
            enemyCount++;
            EnemyHyperspace enemyHyperspace = shipInstance.GetComponent<EnemyHyperspace>();
            if (enemyHyperspace != null)
            {
                enemyHyperspace.arrivalTarget = targetPosition;
                enemyHyperspace.attackTarget = PlayerPlanetPoint.transform.position;
            }
            shipsSpawned++;
            if (shipsSpawned % SpawnSpeedCount == 0)
            {
                currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - intervalReduction);
                CancelInvoke("SpawnShip");
                InvokeRepeating("SpawnShip", currentSpawnInterval, currentSpawnInterval);
            }
        }
    }

    IEnumerator ShowGameOver() {
        yield return new WaitForSeconds(2.0f);
        if (showOver != null) showOver.SetActive(true);
        if (showOverWords != null) showOverWords.SetActive(true);
    }
}