using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HealthBar : MonoBehaviour
{
    [Range(0, 1)]
    public float health = 1.0f;
    public float smoothTime = 0.1f;

    private Material material;
    private float currentClipValue;
    private float smoothVelocity;

    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        currentClipValue = health;
    }

    void Update()
    {
        currentClipValue = Mathf.SmoothDamp(
            currentClipValue,
            health,
            ref smoothVelocity,
            smoothTime
        );
        material.SetFloat("_ClipRight", currentClipValue);
    }

    public void SetHealth(float newHealth)
    {
        health = Mathf.Clamp01(newHealth);
    }
}