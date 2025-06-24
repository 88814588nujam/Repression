using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject disableObject;
    public Transform target;
    public float distance = 200.0f;
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public float yMinLimit = -20.0f;
    public float yMaxLimit = 80.0f;
    public float mouseSpeed = 60.0f;
    public float distanceMin = 40.0f;
    public float distanceMax = 350.0f;

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().freezeRotation = true;
    }

    void LateUpdate()
    {
        if (target)
        {
            if (Input.GetMouseButton(1))
            {
                if (disableObject != null) disableObject.SetActive(false);
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }
            distance -= Input.GetAxis("Mouse ScrollWheel") * mouseSpeed;
            distance = Mathf.Clamp(distance, distanceMin, distanceMax);
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;
            transform.rotation = rotation;
            transform.position = position;
            if (Input.GetMouseButtonUp(1) && disableObject != null) disableObject.SetActive(true);
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F) angle += 360F;
        if (angle > 360F) angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}