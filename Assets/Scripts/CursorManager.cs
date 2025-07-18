using UnityEngine;
using System.Collections;

public class CursorManager : MonoBehaviour
{
    [Header("Normal Cursor Frames")]
    public Texture2D[] normalFrames;
    [Header("Attack Cursor Frames")]
    public Texture2D[] attackFrames;
    [Header("Cursor Setting")]
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;
    public float animationSpeed = 0.05f;

    private Coroutine currentAnimation;
    private static CursorManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            StartNormalAnimation();
        }
        else Destroy(gameObject);
    }

    public static void SetNormalCursor()
    {
        if (instance != null) instance.StartNormalAnimation();
    }

    public static void SetAttackCursor()
    {
        if (instance != null) instance.StartAttackAnimation();
    }

    private void StartNormalAnimation()
    {
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimateCursor(normalFrames));
    }

    private void StartAttackAnimation()
    {
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimateCursor(attackFrames));
    }

    private IEnumerator AnimateCursor(Texture2D[] frames)
    {
        int currentFrame = 0;
        while (true)
        {
            Cursor.SetCursor(frames[currentFrame], hotSpot, cursorMode);
            currentFrame = (currentFrame + 1) % frames.Length;
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}