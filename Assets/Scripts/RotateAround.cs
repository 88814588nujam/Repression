using UnityEngine;
using static TargetHealth;

public class RotateAround : MonoBehaviour
{
    public enum Axis
    {
        x,
        y,
        z
    }
    public float rotationSpeed = 30.0f;
    public Axis axis = Axis.x;

    void Update()
    {
        switch (axis)
        {
            case Axis.x: transform.Rotate(rotationSpeed * Time.deltaTime, 0.0f, 0.0f); break;
            case Axis.y: transform.Rotate(0.0f, rotationSpeed * Time.deltaTime, 0.0f); break;
            case Axis.z: transform.Rotate(0.0f, 0.0f, rotationSpeed * Time.deltaTime); break;
        }
    }
}