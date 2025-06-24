using UnityEngine;
using System.Collections;

public class PlayerExplosion : MonoBehaviour
{
    [Header("Wobble Setting")]
    public float wobbleDuration = 2.0f;
    public float wobbleIntensity = 0.5f;
    public float wobbleSpeed = 5.0f;

    [Header("Explosion Setting")]
    public float shrinkTime = 1.0f;
    public GameObject explosionPrefab;
    public AudioSource reverseSource;
    public AudioSource explosionSource;
    public bool isExplosion = false;

    [Header("Hidden Setting")]
    public GameObject Score;
    public GameObject HighestScore;
    public GameObject boltLightningEffect;
    public GameObject chargedLightningEffect;
    public GameObject chara;

    private bool isExplosionEnd = false;
    private Vector3 originalPosition;
    private EnemySpawner enemySpawner;

    void Start()
    {
        originalPosition = transform.position;
        GameObject gameController = GameObject.FindWithTag("GameController");
        if (gameController != null) enemySpawner = gameController.GetComponent<EnemySpawner>();
    }

    void Update()
    {
        if (isExplosion && !isExplosionEnd)
        {
            isExplosionEnd = true;
            if (enemySpawner != null) enemySpawner.isGameOver = true;
            if (boltLightningEffect != null) boltLightningEffect.SetActive(false);
            if (chargedLightningEffect != null) chargedLightningEffect.SetActive(false);
            StartCoroutine(WobbleAndAct());
        }
    }

    IEnumerator WobbleAndAct()
    {
        float timer = 0f;
        while (timer < wobbleDuration)
        {
            timer += Time.deltaTime;
            float offsetX = Mathf.PerlinNoise(0.0f, Time.time * wobbleSpeed) * 2.0f - 1.0f;
            float offsetY = Mathf.PerlinNoise(1.0f, Time.time * wobbleSpeed) * 2.0f - 1.0f;
            Vector3 offset = new Vector3(offsetX, offsetY, 0.0f) * wobbleIntensity;
            transform.position = originalPosition + offset;
            yield return null;
        }
        transform.position = originalPosition;
        if (reverseSource != null) reverseSource.Play();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        if (chara != null) chara.SetActive(false);
        if (Score != null) Score.SetActive(false);
        if (HighestScore != null) HighestScore.SetActive(false);
        Vector3 originalScale = transform.localScale;
        float timer = 0.0f;
        while (timer < shrinkTime)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / shrinkTime);
            progress = Mathf.Pow(progress, 2.0f);
            transform.localScale = originalScale * (1.0f - progress);
            yield return null;
        }
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 15.0f);
        if (explosionSource != null) explosionSource.Play();
        if (enemySpawner != null) enemySpawner.isShowOver = true;
        Destroy(gameObject);
    }
}