using UnityEngine;
using System.Collections;

public class EnemyHyperspace : MonoBehaviour
{
    public Vector3 arrivalTarget;
    public Vector3 attackTarget;
    public float entryDistance = 500.0f;
    public float entryMoveSpeed = 500.0f;
    public float slowDownDistance = 50.0f;
    public float finalMoveSpeed = 5.0f;
    public AudioSource hyperspaceSource;
    public bool isArriving = false;

    private Vector3 velocity;

    void Start()
    {
        Vector3 direction = (arrivalTarget - attackTarget).normalized;
        transform.position = arrivalTarget + direction * entryDistance;
        transform.forward = -direction;
        velocity = transform.forward * entryMoveSpeed;
        StartCoroutine(ArrivalSequence());
    }

    IEnumerator ArrivalSequence()
    {
        while (Vector3.Distance(transform.position, arrivalTarget) > slowDownDistance)
        {
            transform.position += velocity * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        if (hyperspaceSource != null) hyperspaceSource.Play();
        yield return new WaitForSeconds(0.5f);
        isArriving = true;
        velocity = transform.forward * finalMoveSpeed;
        while (Vector3.Distance(transform.position, arrivalTarget) > 1.0f)
        {
            transform.position += velocity * Time.deltaTime;
            yield return null;
        }
    }
}