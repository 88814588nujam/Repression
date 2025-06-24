using UnityEngine;

public class RotateRandom : MonoBehaviour
{
    private Vector3 rotationSpeed;

    void Start()
    {
        rotationSpeed = new Vector3(
            Random.Range(-30.0f, 30.0f),
            Random.Range(-30.0f, 30.0f),
            Random.Range(-30.0f, 30.0f)
        );
    }

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
