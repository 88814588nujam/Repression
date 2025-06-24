using UnityEngine;
using System.Collections;

public class EnemyExplosion : MonoBehaviour
{
    public GameObject explosionPrefab;
    public AudioSource explosionSource;
    public bool isExplosion = false;

    private float addScore = 1150.0f;
    private bool isExplosionEnd = false;
    private EnemySpawner enemySpawner;
    private ScoreDisplay scoreDisplay;

    void Start()
    {
        GameObject gameController = GameObject.FindWithTag("GameController");
        if (gameController != null)
        {
            enemySpawner = gameController.GetComponent<EnemySpawner>();
            scoreDisplay = gameController.GetComponent<ScoreDisplay>();
        }
    }

    void Update()
    {
        if (isExplosion && !isExplosionEnd)
        {
            isExplosionEnd = true;
            if (scoreDisplay != null) scoreDisplay.changeNowScore(addScore);
            CursorManager.SetNormalCursor();
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            foreach (Transform child in transform)
                if(!child.CompareTag("Health")) child.gameObject.SetActive(false);
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2.0f);
            if (explosionSource != null) explosionSource.Play();
            StartCoroutine(DestroyAfterSound());
        }
    }

    IEnumerator DestroyAfterSound()
    {
        if (enemySpawner != null) enemySpawner.enemyCount--;
        yield return new WaitForSeconds(0.5f);
        foreach (Transform child in transform)
            if (child.CompareTag("Health")) child.gameObject.SetActive(false);
        if (explosionSource != null) yield return new WaitWhile(() => explosionSource.isPlaying);
        Destroy(gameObject);
    }
}