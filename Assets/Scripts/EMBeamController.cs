using UnityEngine;

public class EMBeamController : MonoBehaviour
{
    public float damageMin = 5.0f;
    public float damageMax = 25.0f;
    public float moveSpeed = 100.0f;

    private GameObject target;
    private Vector3 startPoint;
    private bool hasHit = false;
    private bool isRecord = false;

    void Update()
    {
        if (hasHit) return;
        if (target != null && !isRecord)
        {
            startPoint = target.transform.position + new Vector3(0.0f, 5.0f, 0.0f);
            isRecord = true;
            Vector3 initialDirection = (startPoint - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(initialDirection);
        }
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        Destroy(gameObject, 5.0f);
    }

    public void Initialize(GameObject attackTarget)
    {
        target = attackTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        if (other.gameObject == target || other.CompareTag("Enemy"))
        {
            hasHit = true;
            TargetHealth targetHealth = other.GetComponent<TargetHealth>();
            if (targetHealth != null)
            {
                float damageRand = Random.Range(damageMin, damageMax);
                damageRand = Mathf.Round(damageRand * 10.0f) / 10.0f;
                targetHealth.TakeDamage(damageRand);
            }
            Destroy(gameObject);
        }
    }
}