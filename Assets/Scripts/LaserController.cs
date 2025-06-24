using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float damageMin = 5.0f;
    public float damageMax = 10.0f;
    public float moveSpeed = 50.0f;
    public float lifetime = 5.0f;
    public GameObject attackPrefab;

    private TargetHealth targetHealth;

    void Start()
    {
        Destroy(gameObject, lifetime);
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) targetHealth = player.GetComponent<TargetHealth>();
    }

    void Update()
    {
        transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerPart"))
        {
            GameObject attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);
            Destroy(attack, 2.0f);
            float damageRand = Random.Range(damageMin, damageMax);
            damageRand = Mathf.Round(damageRand * 10.0f) / 10.0f;
            if (targetHealth != null) targetHealth.TakeDamage(damageRand);
            Destroy(gameObject);
        }
    }
}